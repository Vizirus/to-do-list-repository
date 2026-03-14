using DataLayer.DataLayer.Interfaces;
using DataLayer.DataLayer.Repositories;

namespace DataLayer.DataLayer.ContextData;
public class UnitOfWork : IUnitOfWork
{
    private readonly ToDoListAppDbContext context;
    public UnitOfWork(ToDoListAppDbContext context)
    {
        this.context = context;
        this.listsRepository = new ListsRepository(context);
        this.sharedListsRepository = new SharedListsRepository(context);
        this.tagsRepository = new TagsRepository(context);
        this.tasksCommentsRepository = new TaskCommentsRepository(context);
        this.tasksRepository = new TasksRepository(context);
        this.taskStatusesRepository = new TaskStatusesRepository(context);
        this.taskTagsRepository = new TaskTagsRepository(context);
        this.userRepository = new UserRepository(context);
    }
    public IListsRepository listsRepository { get; }

    public ISharedListsRepository sharedListsRepository { get; }

    public ITagsRepository tagsRepository { get; }

    public ITaskCommentsRepository tasksCommentsRepository { get; }

    public ITaskRepository tasksRepository { get; }

    public ITaskStatusesRepository taskStatusesRepository { get; }

    public ITaskTagsRepository taskTagsRepository { get; }

    public IUserRepository userRepository { get; }

    public async Task SaveAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }
}
