namespace PairUpScraper;

public abstract class BaseWebScraper : IWebScraper
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    
    protected BaseWebScraper(IServiceProvider serviceProvider, ILogger logger, HttpClient httpClient)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _httpClient = httpClient;
    }
    
    protected ChromeDriver CreateDriver()
    {
        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        var options = new ChromeOptions();
        options.AddArguments("--headless", "--disable-gpu", "--no-sandbox", "--disable-dev-shm-usage");
        return new ChromeDriver(service, options);
    }
    
    public async Task ScrapeActivitiesAsync()
    {
        var allLinks = await CollectLinksAsync();
        _logger.LogInformation($"Found {allLinks.Count} links to process.");

        var newLinks = await FilterNewLinksAsync(allLinks);
        if (!newLinks.Any())
        {
            _logger.LogInformation("No new links to process. Exiting.");
            return;
        }

        await ProcessLinksInParallel(newLinks);
    }

    protected abstract Task<List<string>> CollectLinksAsync();

    private async Task<List<string>> FilterNewLinksAsync(List<string> allLinks)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var existingUrls = await dbContext.Activities
                .Select(activity => activity.Url)
                .ToListAsync();

            var newLinks = allLinks
                .Where(link => !existingUrls.Contains(link))
                .ToList();

            _logger.LogInformation($"Filtered {allLinks.Count - newLinks.Count} existing links. {newLinks.Count} new links to process.");
            return newLinks;
        }
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

    protected abstract Task ProcessProductPageAsync(string productLink, DataContext dbContext);

    protected async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
    {
        try
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&addressdetails=1&limit=1";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            requestMessage.Headers.Add("Email", "youremail@domain.com");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            var place = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (place != null && place.Count > 0)
            {
                return (place[0].lat, place[0].lon);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to fetch coordinates for address: {address}");
        }

        return (0.0, 0.0);
    }
}