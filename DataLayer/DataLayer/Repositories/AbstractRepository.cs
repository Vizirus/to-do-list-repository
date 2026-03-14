using DataLayer.DataLayer.ContextData;

namespace DataLayer.DataLayer.Repositories;
public abstract class AbstractRepository
{
    protected AbstractRepository(ToDoListAppDbContext context)
    {
        this.toDoListAppDbContext = context;
    }

    protected ToDoListAppDbContext toDoListAppDbContext { get; set; }
}
