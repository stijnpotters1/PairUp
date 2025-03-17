namespace PairUpScraper.Scrapers.DagjeWegScraper;

public class DagjeWegActivityScraper : BaseWebScraper
{
    public DagjeWegActivityScraper(
        IServiceProvider serviceProvider,
        ILogger<DagjeWegActivityScraper> logger,
        HttpClient httpClient
    ) : base(serviceProvider, logger, httpClient)
    { }
    
    protected override async Task<List<string>> CollectLinksAsync()
    {
        var allLinks = new List<string>();
        int page = 1;
        
        using (var driver = CreateDriver())
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            while (true)
            {
                var baseUrl = $"https://www.dagjeweg.nl/zoeken?page={page}";
                driver.Navigate().GoToUrl(baseUrl);
                
                await Task.Delay(2000);

                var links = driver
                    .FindElements(By.CssSelector("a.listImage"))
                    .Select(link => link.GetAttribute("href"))
                    .Where(href => !string.IsNullOrEmpty(href))
                    .Distinct()
                    .ToList();
                
                if (links.Count < 15)
                    break;
                
                Console.WriteLine("added new links to the link list");
                
                allLinks.AddRange(links);
                page++;
            }
        }
        
        Console.WriteLine("out of while loop");
        return allLinks;
    }

    protected override async Task ProcessProductPageAsync(string productLink, DataContext dbContext)
    {
        using var driver = CreateDriver();
        driver.Navigate().GoToUrl(productLink);

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("header.pageTitle")));

        var subLevelCategoryNames = driver.FindElements(By.CssSelector("ul.sidebar#dugerelateerd li.title a"))
                    .Select(element => element.Text.Trim())
                    .Where(text => !string.IsNullOrEmpty(text))
                    .Distinct()
                    .ToList();

        List<SubLevelCategory> subLevelCategories = await CollectCategoryNamesAsync(dbContext, subLevelCategoryNames);

        // Extract name
        var activityName = driver.FindElement(By.CssSelector("header.pageTitle h1")).Text;
        Console.WriteLine($"Activity Name: {activityName}");

        var imageUrl = driver.FindElement(By.CssSelector("div.figure picture img.listImage"))
            .GetAttribute("src");
        
        Console.WriteLine($"Image URL: {imageUrl}");

        var rawDescription = driver.FindElement(By.CssSelector("article.redactietekst p"))?.Text 
                             ?? string.Empty;

        const int maxLength = 255;
        var description = rawDescription.Length > maxLength 
            ? rawDescription.Substring(0, maxLength - 3) + "..." 
            : rawDescription;

        Console.WriteLine($"Description: {description}");

        var priceElements = driver.FindElements(By.CssSelector("section.uitstapje div.padding15 p"));
        var price = priceElements.Count > 1 ? priceElements[1].Text.Replace("?", "€").Trim() : string.Empty;
        Console.WriteLine($"Price: {price}");

        // Extract full address
        var streetAddress = driver.FindElement(By.CssSelector("meta[itemprop='streetAddress']")).GetAttribute("content");
        var postalCode = driver.FindElement(By.CssSelector("meta[itemprop='postalCode']")).GetAttribute("content");
        var locality = driver.FindElement(By.CssSelector("meta[itemprop='addressLocality']")).GetAttribute("content");
        var region = driver.FindElement(By.CssSelector("meta[itemprop='addressRegion']")).GetAttribute("content");

        var fullAddress = $"{streetAddress}, {postalCode} {locality}, {region}";
        Console.WriteLine($"Full Address: {fullAddress}");
        
        var coordinates = await GetCoordinatesAsync(fullAddress);
        Console.WriteLine($"Latitude: {coordinates.latitude}, Longitude: {coordinates.longitude}");

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
            TopLevelCategory = TopLevelCategory.Activity
        };

        Console.WriteLine($"Activity created: {activity.Name}, {activity.Url}");

        activity.ActivitySubLevelCategories = subLevelCategories.Select(subLevelCategory =>
            new ActivitySubLevelCategory
            {
                ActivityId = activity.Id,
                SubLevelCategoryId = subLevelCategory.Id
            }).ToList();

        Console.WriteLine($"Saving Activity: {activity.Name} to the database...");

        await dbContext.Activities.AddAsync(activity);
        await dbContext.SaveChangesAsync();
    }
    
    private async Task<List<SubLevelCategory>> CollectCategoryNamesAsync(DataContext dbContext, List<String> subLevelCategoryNames)
    {
        var categoryList = new List<SubLevelCategory>();

        foreach (var categoryName in subLevelCategoryNames)
        {
            var sublevelCategory = await dbContext.SubLevelCategories
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            if (sublevelCategory == null)
            {
                sublevelCategory = new SubLevelCategory
                {
                    Id = Guid.NewGuid(),
                    Name = categoryName
                };
                await dbContext.SubLevelCategories.AddAsync(sublevelCategory);
                await dbContext.SaveChangesAsync();
            }
            categoryList.Add(sublevelCategory);
        }

        return categoryList;
    }
}