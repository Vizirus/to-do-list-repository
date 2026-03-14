using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public class UserRepository : AbstractRepository, IUserRepository
{
    public UserRepository(ToDoListAppDbContext context)
        : base(context)
    {
    }

    public async System.Threading.Tasks.Task AddAsync(User entity)
    {
        _ = await this.toDoListAppDbContext.users.AddAsync(entity);
    }

    public void Delete(User entity)
    {
        _ = this.toDoListAppDbContext.users.Remove(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.toDoListAppDbContext.users.Remove(entity);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var value = await this.toDoListAppDbContext.users.ToListAsync();
        return value ?? null!;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var value = await this.toDoListAppDbContext.users.FirstOrDefaultAsync(x => x.Id == id);
        return value ?? null;
    }

    public void Update(User entity)
    {
        _ = this.toDoListAppDbContext.users.Update(entity);
    }
}
