namespace PairUpCore.Interfaces;

public interface ISubLevelCategoryRepository
{
    Task<IEnumerable<SubLevelCategory>> GetSubLevelCategoriesAsync();
}