namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubLevelCategoryController : ControllerBase
{
    private readonly ISubLevelCategoryRepository _repository;
    private readonly IService<SubLevelCategory, SubLevelCategoryResponse> _service;

    public SubLevelCategoryController(ISubLevelCategoryRepository repository, IService<SubLevelCategory, SubLevelCategoryResponse> service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubLevelCategoryResponse>>> GetSubLevelCategories()
    {
        IEnumerable<SubLevelCategory> subLevelCategories = await _repository.GetSubLevelCategoriesAsync();
        IEnumerable<SubLevelCategoryResponse> subLevelCategoryResponses = _service.ConvertToResponse(subLevelCategories);
        return Ok(subLevelCategoryResponses);
    }
}