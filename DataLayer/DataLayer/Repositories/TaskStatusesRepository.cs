using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class TaskStatusesRepository : AbstractRepository, ITaskStatusesRepository
{
    public TaskStatusesRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(TaskStatuses entity)
    {
        _ = await this.toDoListAppDbContext.taskStatuses.AddAsync(entity);
    }

    public void Delete(TaskStatuses entity)
    {
        _ = this.toDoListAppDbContext.taskStatuses.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.taskStatuses.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<TaskStatuses>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.taskStatuses.ToListAsync();
        return value ?? null!;
    }

    public async Task<TaskStatuses?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.taskStatuses.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(TaskStatuses entity)
    {
        _ = this.toDoListAppDbContext.taskStatuses.Update(entity);
    }
}
