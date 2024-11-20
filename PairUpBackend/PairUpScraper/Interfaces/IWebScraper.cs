namespace PairUpScraper.Interfaces;

public interface IWebScraper
{
    Task<List<Activity>> ScrapeActivitiesAsync();
    Task SaveActivitiesToDatabaseAsync(List<Activity> activities);
}