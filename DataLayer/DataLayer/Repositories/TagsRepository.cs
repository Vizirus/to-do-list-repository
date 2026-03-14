using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class TagsRepository : AbstractRepository, ITagsRepository
{
    public TagsRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(Tags entity)
    {
        _ = await this.toDoListAppDbContext.tags.AddAsync(entity);
    }

    public void Delete(Tags entity)
    {
        _ = this.toDoListAppDbContext.tags.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.tags.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Tags>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.tags.ToListAsync();
        return value ?? null!;
    }

    public async Task<Tags?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.tags.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(Tags entity)
    {
        _ = this.toDoListAppDbContext.tags.Update(entity);
    }
}
