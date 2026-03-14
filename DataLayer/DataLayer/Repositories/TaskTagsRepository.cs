using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class TaskTagsRepository : AbstractRepository, ITaskTagsRepository
{
    public TaskTagsRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(TaskTags entity)
    {
        _ = await this.toDoListAppDbContext.taskTags.AddAsync(entity);
    }

    public void Delete(TaskTags entity)
    {
        _ = this.toDoListAppDbContext.taskTags.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.taskTags.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<TaskTags>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.taskTags.ToListAsync();
        return value ?? null!;
    }

    public async Task<TaskTags?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.taskTags.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(TaskTags entity)
    {
        _ = this.toDoListAppDbContext.taskTags.Update(entity);
    }
}
