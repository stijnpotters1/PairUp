﻿namespace PairUpScraper;

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
                // Also, get all the concrete classes that inherit from BaseWebScraper
                var scrapers = scope.ServiceProvider.GetServices<BaseWebScraper>().ToList();

                var tasks = scrapers.Select(webScraper => Task.Run(async () =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation requested.");
                        return;
                    }

                    Console.WriteLine($"Starting scraper: {webScraper.GetType().Name}");
                    await webScraper.ScrapeActivitiesAsync();
                })).ToList();

                await Task.WhenAll(tasks);
            }
        }
        catch (HttpRequestException httpRequestException)
        {
            Console.WriteLine($"HTTP Error while scraping activities: {httpRequestException}");
        }
        catch (TimeoutException timeoutException)
        {
            Console.WriteLine($"Timeout Error while scraping activities: {timeoutException}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An unexpected error occurred: {exception.Message}");
        }
    }
}