using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class TaskCommentsRepository : AbstractRepository, ITaskCommentsRepository
{
    public TaskCommentsRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(TaskComments entity)
    {
        _ = await this.toDoListAppDbContext.taskComments.AddAsync(entity);
    }

    public void Delete(TaskComments entity)
    {
        _ = this.toDoListAppDbContext.taskComments.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.taskComments.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<TaskComments>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.taskComments.ToListAsync();
        return value ?? null!;
    }

    public async Task<TaskComments?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.taskComments.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(TaskComments entity)
    {
        _ = this.toDoListAppDbContext.taskComments.Update(entity);
    }
}
