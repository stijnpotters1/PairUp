namespace PairUpCore.Interfaces;

public interface IService<in TEntity, out TResponse>
{
    TResponse ConvertToResponse(TEntity entity);
    IEnumerable<TResponse> ConvertToResponse(IEnumerable<TEntity> entities);
}