using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class ListsRepository : AbstractRepository, IListsRepository
{
    public ListsRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(Lists entity)
    {
        _ = await this.toDoListAppDbContext.lists.AddAsync(entity);
    }

    public void Delete(Lists entity)
    {
        _ = this.toDoListAppDbContext.lists.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.lists.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Lists>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.lists.ToListAsync();
        return value ?? null!;
    }

    public async Task<Lists?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.lists.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(Lists entity)
    {
        _ = this.toDoListAppDbContext.lists.Update(entity);
    }
}
