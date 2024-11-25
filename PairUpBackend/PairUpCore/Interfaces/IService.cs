namespace PairUpCore.Interfaces;

public interface IService<in TEntity, out TResponse>
{
    IEnumerable<TResponse> ConvertToResponse(IEnumerable<TEntity> entities);
}