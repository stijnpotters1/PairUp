using Microsoft.Extensions.DependencyInjection;

namespace PairUpScraper.Scrapers;

public class TripAdvisorScraper : IWebScraper
{
    private readonly HttpClient _httpClient;
    private readonly DataContext _context;

    public TripAdvisorScraper(HttpClient httpClient, DataContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<List<Activity>> ScrapeActivitiesAsync()
    {
        
        
        var activities = new List<Activity>();

        string url = "https://www.tripadvisor.com/Attractions-g188553-Activities-a_allAttractions.true-The_Netherlands.html";
        var htmlContent = await _httpClient.GetStringAsync(url);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var categoryNodes = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'biGQs _P FvqAl FGgoA vvmrG')]");
        if (categoryNodes != null)
        {
            foreach (var categoryNode in categoryNodes)
            {
                var categoryName = categoryNode.InnerText.Trim();

                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryName);

                if (existingCategory == null)
                {
                    var category = new Category { Id = Guid.NewGuid(), Name = categoryName };
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();
                }
            }
        }

        var activityNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'attraction_element')]");

        if (activityNodes != null)
        {
            foreach (var activityNode in activityNodes)
            {
                var activity = new Activity
                {
                    Name = activityNode.SelectSingleNode(".//div[contains(@class, 'listing_title')]")?.InnerText.Trim(),
                    Description = activityNode.SelectSingleNode(".//div[contains(@class, 'listing_desc')]")?.InnerText.Trim(),
                    Url = activityNode.SelectSingleNode(".//a[contains(@href, '/Attraction_Review')]")?.GetAttributeValue("href", ""),
                    Price = "Not available", // Modify if available
                    Age = "All ages", // Modify if available
                    Duration = "Not specified", // Modify if available
                    FullAddress = activityNode.SelectSingleNode(".//div[contains(@class, 'address-class')]")?.InnerText.Trim(), // Modify selector for address
                };

                // Get or create the category for the activity
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == "CategoryName"); // Use the appropriate category name

                if (category == null)
                {
                    // If category doesn't exist, create and save it
                    category = new Category { Name = "CategoryName" };
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync(); // Save to database immediately
                }

                activity.CategoryId = category.Id;

                // Convert the address to latitude and longitude
                var coordinates = await GetCoordinatesAsync(activity.FullAddress);
                activity.Latitude = coordinates.latitude;
                activity.Longitude = coordinates.longitude;

                // Save the activity and place to the database
                await _context.Activities.AddAsync(activity);
                await _context.SaveChangesAsync();

                activities.Add(activity);
            }
        }

        return activities;
    }

    public async Task SaveActivitiesToDatabaseAsync(List<Activity> activities)
    {
        foreach (var activity in activities)
        {
            await _context.Activities.AddAsync(activity);
        }
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