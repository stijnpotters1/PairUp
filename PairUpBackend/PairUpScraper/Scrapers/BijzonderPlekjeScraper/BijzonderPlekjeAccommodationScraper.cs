namespace PairUpScraper.Scrapers.BijzonderPlekjeScraper;

public class BijzonderPlekjeAccommodationScraper : BaseWebScraper
{
    public BijzonderPlekjeAccommodationScraper(
        IServiceProvider serviceProvider,
        ILogger<BijzonderPlekjeAccommodationScraper> logger,
        HttpClient httpClient
    ) : base(serviceProvider, logger, httpClient)
    { }

    protected override async Task<List<string>> CollectLinksAsync()
    {
        var baseUrl = "https://bijzonderplekje.nl/overnachten/?plekje_country=nederland";
        var allLinks = new List<string>();

        using (var driver = CreateDriver())
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl(baseUrl);

            int previousHeight = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight;"));
            while (true)
            {
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                await Task.Delay(2200);

                int newHeight = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight;"));
                if (newHeight == previousHeight) break;
                previousHeight = newHeight;
            }

            allLinks = driver
                .FindElements(By.CssSelector("a.card.card--plekje"))
                .Where(link => link.GetAttribute("class")?.Trim() == "card card--plekje")
                .Select(link => link.GetAttribute("href"))
                .Where(href => !string.IsNullOrEmpty(href))
                .Distinct()
                .ToList();
        }

        return allLinks;
    }

    protected override async Task ProcessProductPageAsync(string productLink, DataContext dbContext)
    {
        using var driver = CreateDriver();
        driver.Navigate().GoToUrl(productLink);

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("h1.hero-slider__title.notranslate")));

        // Extract sublevel category
        var sublevelCategoryName = driver.FindElement(By.CssSelector("div.header__subtitle.text-script-a")).Text;
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

        // Extract name, image URL, and description
        var activityName = driver.FindElement(By.CssSelector("h1.hero-slider__title.notranslate")).Text;
        var imageUrl = driver.FindElement(By.CssSelector("li.swiper-slide.swiper-slide-active img"))
            .GetAttribute("src");
        var rawDescription = driver.FindElements(By.CssSelector("article.single-plekje__content div.block-text p"))
                             .FirstOrDefault()?.Text ??
                             driver.FindElements(By.CssSelector("article.single-plekje__content p"))
                             .FirstOrDefault()?.Text ??
                             string.Empty;

        const int maxLength = 255;
        var description = rawDescription.Length > maxLength
            ? rawDescription.Substring(0, maxLength - 3) + "..."
            : rawDescription;

        var price = driver.FindElements(By.CssSelector("div.block-meta__item"))
            .FirstOrDefault(e => e.GetAttribute("class") == "block-meta__item")?.Text
            .Replace("?", "€").Trim() ?? string.Empty;

        // Extract full address
        var addressDiv = driver.FindElement(By.CssSelector("div.article__caption"));
        var fullAddress = addressDiv.GetAttribute("innerText")
            .Replace(addressDiv.FindElement(By.CssSelector("span")).Text, "").Trim();

        // Get coordinates
        var coordinates = await GetCoordinatesAsync(fullAddress);

        // Create and save activity
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

        activity.ActivitySubLevelCategories = new List<ActivitySubLevelCategory>
        {
            new ActivitySubLevelCategory
            {
                ActivityId = activity.Id,
                SubLevelCategoryId = sublevelCategory.Id
            }
        };

        await dbContext.Activities.AddAsync(activity);
        await dbContext.SaveChangesAsync();
    }
}
