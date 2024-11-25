namespace PairUpScraper;

public class ScraperBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const int ScrapeFrequencyDays = 1;

    public ScraperBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunScraperAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(ScrapeFrequencyDays), stoppingToken);

            await RunScraperAsync(stoppingToken);
        }
    }

    private async Task RunScraperAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var webScraper = scope.ServiceProvider.GetRequiredService<IWebScraper>();
                await webScraper.ScrapeActivitiesAsync();
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
        catch (HttpRequestException httpRequestException)
        {
            Console.WriteLine($"HTTP Error while scraping activities: ${httpRequestException}");
        }
        catch (TimeoutException timeoutException)
        {
            Console.WriteLine($"Timeout Error while scraping activities: ${timeoutException}");
        }
        catch (Exception exception)
        {
            throw new Exception($"An unexpected error occurred: {exception.Message}");
        }
    }
}