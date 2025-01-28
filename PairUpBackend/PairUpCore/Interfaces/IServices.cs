namespace PairUpCore.Interfaces;

public interface IServices<in TEntity, out TResponse>
{
    IEnumerable<TResponse> ConvertToResponse(IEnumerable<TEntity> entities);
}