using System.Linq.Expressions;

namespace DAPA.DataAccess;

public interface IRepository<T>
    where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();

    public Task<T?> GetByPropertyAsync(Expression<Func<T, bool>> expression);

    public Task<bool> ExistsByPropertyAsync(Expression<Func<T, bool>> expression);

    public Task InsertAsync(T entity);

    public Task UpdateAsync(T entity);

    public Task DeleteAsync(T entity);
}
