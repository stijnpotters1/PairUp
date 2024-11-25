namespace PairUpApi.Service;

public class SubLevelCategoryService : IService<SubLevelCategory, SubLevelCategoryResponse>
{
    private readonly IMapper _mapper;

    public SubLevelCategoryService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    private SubLevelCategoryResponse ConvertToResponse(SubLevelCategory subLevelCategory)
    {
        return _mapper.Map<SubLevelCategoryResponse>(subLevelCategory);
    }

    public IEnumerable<SubLevelCategoryResponse> ConvertToResponse(IEnumerable<SubLevelCategory> subLevelCategories)
    {
        return subLevelCategories.Select(ConvertToResponse);
    }
}