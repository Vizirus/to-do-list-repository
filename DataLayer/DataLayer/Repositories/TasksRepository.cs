using DataLayer.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class TasksRepository : AbstractRepository, ITaskRepository
{
    public TasksRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Entities.Task entity)
    {
        _ = await this.toDoListAppDbContext.tasks.AddAsync(entity);
    }

    public void Delete(Entities.Task entity)
    {
        _ = this.toDoListAppDbContext.tasks.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.tasks.Remove(entity);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Entities.Task>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.tasks.ToListAsync();
        if (value != null)
        {
            return value;
        }

        return null!;
    }

    public async Task<Entities.Task?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (value != null)
        {
            return value;
        }

        return null;
    }

    public void Update(Entities.Task entity)
    {
        _ = this.toDoListAppDbContext.tasks.Update(entity);
    }
}
