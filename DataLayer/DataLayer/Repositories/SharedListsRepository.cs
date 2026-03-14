using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class SharedListsRepository : AbstractRepository, ISharedListsRepository
{
    public SharedListsRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(SharedLists entity)
    {
        _ = await this.toDoListAppDbContext.sharedLists.AddAsync(entity);
    }

    public void Delete(SharedLists entity)
    {
        _ = this.toDoListAppDbContext.sharedLists.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.sharedLists.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<SharedLists>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.sharedLists.ToListAsync();
        return value ?? null!;
    }

    public async Task<SharedLists?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.sharedLists.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(SharedLists entity)
    {
        _ = this.toDoListAppDbContext.sharedLists.Update(entity);
    }
}
