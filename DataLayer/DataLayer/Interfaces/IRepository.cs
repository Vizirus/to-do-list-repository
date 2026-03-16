using DataLayer.DataLayer.Entities;

namespace DataLayer.DataLayer.Interfaces;
public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(int id);

    System.Threading.Tasks.Task AddAsync(TEntity entity);

    void Delete(TEntity entity);

    Task<bool> DeleteByIdAsync(int id);

    void Update(TEntity entity);
}

