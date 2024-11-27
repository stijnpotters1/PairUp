using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace PairUpScraper.Scrapers;

public class TripAdvisorScraper : IWebScraper
{
    private readonly DataContext _context;
    private readonly HttpClient _httpClient;
    private static Random _random = new Random();

    public TripAdvisorScraper(DataContext context, HttpClient httpClient)
    {
        _context = context;
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
        // Uncomment for headless mode if needed
        options.AddArgument("--headless");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        // Random User-Agent to avoid detection
        var userAgents = new List<string>
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36"
        };
        options.AddArgument($"--user-agent={userAgents[_random.Next(userAgents.Count)]}");

        // // Use a proxy if needed
        // options.AddArgument("--proxy-server=http://yourproxy:port");

        return options;
    }

    public async Task ScrapeActivitiesAsync()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

        using (var driver = new ChromeDriver(CreateChromeDriverService(), GetChromeOptions()))
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            string baseUrl = "https://www.tripadvisor.nl/Attraction_Products-g188553-The_Netherlands.html";
            driver.Navigate().GoToUrl(baseUrl);

            var stack = new Stack<string>();
            var visitedUrls = new HashSet<string>();
            stack.Push(baseUrl);

            while (stack.Count > 0)
            {
                string currentUrl = stack.Pop();

                if (visitedUrls.Contains(currentUrl))
                    continue;

                visitedUrls.Add(currentUrl);
                Console.WriteLine($"Visiting: {currentUrl}");
                driver.Navigate().GoToUrl(currentUrl);

                // Simulate human scrolling
                ScrollPage(driver);

                // Collect product links
                var productLinks = driver
                    .FindElements(By.CssSelector("div.IDaDx.Iwmxp.mvTrV.cyIij.fluiI.SMjpI a[href]"))
                    .Select(link => link.GetAttribute("href"))
                    .Where(link => link.Contains("AttractionProductReview"))
                    .Distinct()
                    .ToList();

                Console.WriteLine($"Found {productLinks.Count} product links on {currentUrl}.");

                foreach (var productLink in productLinks)
                {
                    if (!visitedUrls.Contains(productLink))
                        stack.Push(productLink);

                    await ProcessProductPageAsync(driver, productLink);
                }

                // Add navigable links to stack
                var navigableLinks = driver
                    .FindElements(By.CssSelector("a[href^='https://www.tripadvisor.com/Attraction_Products-g188553-']"))
                    .Select(link => link.GetAttribute("href"))
                    .Where(link => !visitedUrls.Contains(link))
                    .Distinct()
                    .ToList();

                foreach (var link in navigableLinks)
                {
                    stack.Push(link);
                }

                // Add a random delay to avoid detection
                await Task.Delay(_random.Next(2000, 5000));
            }
        }
    }

    private void ScrollPage(IWebDriver driver)
    {
        var jsExecutor = (IJavaScriptExecutor)driver;
        for (int i = 0; i < 3; i++)
        {
            jsExecutor.ExecuteScript("window.scrollBy(0, 300);");
            Task.Delay(_random.Next(500, 1500)).Wait(); // Simulate human-like scrolling
        }
    }

    private async Task ProcessProductPageAsync(IWebDriver driver, string productLink)
    {
        driver.Navigate().GoToUrl(productLink);

        // Wait for the page to load (anti-bot behavior)
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("h1.biGQs._P.fiohW.ncFvv.EVnyE")));

        var sellerCategories = new List<string>();
        var sellerLink = driver.FindElement(By.CssSelector("a.BMQDV._F.Gv.wSSLS.SwZTJ.FGwzt.PaRlG"))
            .GetAttribute("href");

        if (!string.IsNullOrEmpty(sellerLink))
        {   
            driver.Navigate().GoToUrl(sellerLink);
            wait.Until(d => d.FindElement(By.CssSelector("div.eojVo")));

            var categoryElements = driver.FindElements(By.CssSelector("div.eojVo"))
                .Select(category => category.Text)
                .Distinct()
                .ToList();
            sellerCategories.AddRange(categoryElements);
        }

        var categoryEntities = new List<SubLevelCategory>();
        foreach (var categoryName in sellerCategories)
        {
            var existingCategory = await _context.SubLevelCategories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (existingCategory == null)
            {
                existingCategory = new SubLevelCategory() { Id = Guid.NewGuid(), Name = categoryName };
                await _context.SubLevelCategories.AddAsync(existingCategory);
            }

            categoryEntities.Add(existingCategory);
        }

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Name = driver.FindElement(By.CssSelector("h1.biGQs._P.fiohW.ncFvv.EVnyE")).Text,
            Description = driver.FindElement(By.CssSelector("div._d")).Text,
            Url = productLink,
            Price = driver.FindElement(By.CssSelector("span.price")).Text,
            Age = driver.FindElements(By.CssSelector("span.biGQs._P.pZUbB.egaXP.KxBGd")).Count > 0
                ? driver.FindElements(By.CssSelector("span.biGQs._P.pZUbB.egaXP.KxBGd"))[0].Text
                : string.Empty,
            Duration = driver.FindElements(By.CssSelector("span.biGQs._P.pZUbB.egaXP.KxBGd")).Count > 1
                ? driver.FindElements(By.CssSelector("span.biGQs._P.pZUbB.egaXP.KxBGd"))[1].Text
                : string.Empty,
            FullAddress = driver.FindElement(By.CssSelector("div.biGQs._P.fiohW.uPlAb.fOtGX")).Text,
            Latitude = (await GetCoordinatesAsync(driver
                .FindElement(By.CssSelector("div.biGQs._P.fiohW.uPlAb.fOtGX")).Text)).latitude,
            Longitude = (await GetCoordinatesAsync(driver
                .FindElement(By.CssSelector("div.biGQs._P.fiohW.uPlAb.fOtGX")).Text)).longitude
        };

        foreach (var category in categoryEntities)
        {
            activity.ActivitySubLevelCategories.Add(new ActivitySubLevelCategory()
            {
                ActivityId = activity.Id,
                SubLevelCategoryId = category.Id
            });
        }

        await _context.Activities.AddAsync(activity);
        await _context.SaveChangesAsync();
    }

    private async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
    {
        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&addressdetails=1&limit=1";

        var response = await _httpClient.GetStringAsync(url);
        if (!string.IsNullOrEmpty(response))
        {
            var results = JsonConvert.DeserializeObject<List<dynamic>>(response);
            if (results != null && results.Count > 0)
            {
                var location = results[0];
                return (location.lat, location.lon);
            }
        }

        return (0.0, 0.0);
    }
}