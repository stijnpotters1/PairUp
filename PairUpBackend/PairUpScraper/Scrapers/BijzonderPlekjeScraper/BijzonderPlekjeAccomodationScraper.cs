using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Logging;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace PairUpScraper.Scrapers.BijzonderPlekjeScraper;

public class BijzonderPlekjeAccomodationScraper : IWebScraper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BijzonderPlekjeAccomodationScraper> _logger;
    private readonly HttpClient _httpClient;
    private static readonly Random _random = new();

    public BijzonderPlekjeAccomodationScraper(IServiceProvider serviceProvider, ILogger<BijzonderPlekjeAccomodationScraper> logger, HttpClient httpClient)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _httpClient = httpClient;
    }

    private ChromeDriverService CreateChromeDriverService()
    {
        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        return service;
    }

    private ChromeOptions GetChromeOptions()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        var userAgents = new List<string>
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36"
        };
        options.AddArgument($"--user-agent={userAgents[_random.Next(userAgents.Count)]}");

        return options;
    }

    public async Task ScrapeActivitiesAsync()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

        var baseUrl = "https://bijzonderplekje.nl/overnachten/?plekje_country=nederland";
        List<string> allLinks;

        using (var driver = new ChromeDriver(CreateChromeDriverService(), GetChromeOptions()))
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl(baseUrl);
            
            int previousHeight = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight;"));
        
            while (true)
            {
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                await Task.Delay(1500);

                int newHeight = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight;"));

                if (newHeight == previousHeight)
                {
                    break;
                }
                previousHeight = newHeight;
            }

            allLinks = driver
                .FindElements(By.CssSelector("a.card.card--plekje"))
                .Where(link => link.GetAttribute("class")?.Trim() == "card card--plekje")
                .Select(link => link.GetAttribute("href"))
                .Where(href => !string.IsNullOrEmpty(href))
                .Distinct()
                .ToList();

            _logger.LogInformation($"Found {allLinks.Count} links to process.");
        }

        List<string> newLinks;
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            newLinks = await FilterNewLinksAsync(allLinks, dbContext);
        }

        if (!newLinks.Any())
        {
            _logger.LogInformation("No new links to process. Exiting.");
            return;
        }

        await ProcessLinksInParallel(newLinks);
    }

    private async Task<List<string>> FilterNewLinksAsync(List<string> allLinks, DataContext dbContext)
    {
        var existingUrls = await dbContext.Activities
            .Select(activity => activity.Url)
            .ToListAsync();

        var newLinks = allLinks
            .Where(link => !existingUrls.Contains(link))
            .ToList();

        _logger.LogInformation($"Filtered {allLinks.Count - newLinks.Count} existing links. {newLinks.Count} new links to process.");
        return newLinks;
    }

    private async Task ProcessLinksInParallel(List<string> links)
    {
        const int maxDegreeOfParallelism = 5;
        SemaphoreSlim semaphore = new(maxDegreeOfParallelism);

        var tasks = links.Select(async link =>
        {
            await semaphore.WaitAsync();
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                await ProcessProductPageAsync(link, dbContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing link: {link}");
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task ProcessProductPageAsync(string productLink, DataContext dbContext)
    {
        using var driver = new ChromeDriver(CreateChromeDriverService(), GetChromeOptions());
        driver.Navigate().GoToUrl(productLink);

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("h1.hero-slider__title.notranslate")));

        // Extract sublevel category
        var sublevelCategoryName = driver.FindElement(By.CssSelector("div.header__subtitle.text-script-a")).Text;
        Console.WriteLine($"Sublevel Category Name: {sublevelCategoryName}");

        var sublevelCategory = await dbContext.SubLevelCategories
            .FirstOrDefaultAsync(c => c.Name == sublevelCategoryName);

        if (sublevelCategory == null)
        {
            sublevelCategory = new SubLevelCategory
            {
                Id = Guid.NewGuid(),
                Name = sublevelCategoryName
            };
            await dbContext.SubLevelCategories.AddAsync(sublevelCategory);
            await dbContext.SaveChangesAsync();
        }

        // Extract name
        var activityName = driver.FindElement(By.CssSelector("h1.hero-slider__title.notranslate")).Text;
        Console.WriteLine($"Activity Name: {activityName}");

        var imageUrl = driver.FindElement(By.CssSelector("li.swiper-slide.swiper-slide-active img"))
            .GetAttribute("src");
        
        Console.WriteLine($"Image URL: {imageUrl}");

        var rawDescription = driver.FindElements(By.CssSelector("article.single-plekje__content div.block-text p"))
                                 .FirstOrDefault()?.Text 
                             ?? driver.FindElements(By.CssSelector("article.single-plekje__content p")).FirstOrDefault()?.Text 
                             ?? string.Empty;

        const int maxLength = 255;
        var description = rawDescription.Length > maxLength 
            ? rawDescription.Substring(0, maxLength - 3) + "..." 
            : rawDescription;

        Console.WriteLine($"Description: {description}");

        var price = driver.FindElements(By.CssSelector("div.block-meta__item"))
            .FirstOrDefault(e => e.GetAttribute("class") == "block-meta__item")?.Text
            .Replace("?", "€").Trim() ?? string.Empty;
        Console.WriteLine($"Price: {price}");

        // Extract full address
        var addressDiv = driver.FindElement(By.CssSelector("div.article__caption"));
        var fullAddress = addressDiv.GetAttribute("innerText")
            .Replace(addressDiv.FindElement(By.CssSelector("span")).Text, "").Trim();

        Console.WriteLine($"Full Address: {fullAddress}");

        // Extract latitude and longitude
        var coordinates = await GetCoordinatesAsync(fullAddress);
        Console.WriteLine($"Latitude: {coordinates.latitude}, Longitude: {coordinates.longitude}");

        // Prepare activity object
        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Name = activityName,
            Image = imageUrl,
            Description = description,
            Url = productLink,
            Price = price,
            FullAddress = fullAddress,
            Latitude = coordinates.latitude,
            Longitude = coordinates.longitude,
            TopLevelCategory = TopLevelCategory.Accommodation
        };

        Console.WriteLine($"Activity created: {activity.Name}, {activity.Url}");

        activity.ActivitySubLevelCategories = new List<ActivitySubLevelCategory>
        {
            new ActivitySubLevelCategory
            {
                ActivityId = activity.Id,
                SubLevelCategoryId = sublevelCategory.Id
            }
        };

        // Log activity details before saving
        Console.WriteLine($"Saving Activity: {activity.Name} to the database...");

        await dbContext.Activities.AddAsync(activity);
        await dbContext.SaveChangesAsync();
    }

    private async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
    {
        try
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&addressdetails=1&limit=1";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            requestMessage.Headers.Add("Email", "youremail@domain.com");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Raw response: " + responseContent.Substring(0, Math.Min(500, responseContent.Length)));

            // Check if the response is in HTML (starts with "<")
            if (responseContent.StartsWith("<"))
            {
                _logger.LogError("Received HTML instead of JSON. The response might be an error page.");
                return (0.0, 0.0);
            }

            // Attempt to deserialize the response as JSON
            var place = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (place != null && place.Count > 0)
            {
                _logger.LogInformation($"Coordinates found: {place[0].lat}, {place[0].lon}");
                return (place[0].lat, place[0].lon);
            }

            _logger.LogError("No valid location found in response.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to fetch coordinates for address: {address}");
        }

        return (0.0, 0.0);
    }
}