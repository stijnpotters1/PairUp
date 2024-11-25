namespace PairUpInfrastructure.Repositories;

public class SubLevelCategoryRepository : ISubLevelCategoryRepository
{
    private readonly DataContext _context;

    public SubLevelCategoryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SubLevelCategory>> GetSubLevelCategoriesAsync()
    {
        return await _context.SubLevelCategories.ToListAsync();
    }
}