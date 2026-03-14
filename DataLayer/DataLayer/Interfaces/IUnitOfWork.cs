namespace DataLayer.DataLayer.Interfaces;
public interface IUnitOfWork
{
    IListsRepository listsRepository { get; }

    ISharedListsRepository sharedListsRepository { get; }

    ITagsRepository tagsRepository { get; }

    ITaskCommentsRepository tasksCommentsRepository { get; }

    ITaskRepository tasksRepository { get; }

    ITaskStatusesRepository taskStatusesRepository { get; }

    ITaskTagsRepository taskTagsRepository { get; }

    IUserRepository userRepository { get; }

    Task SaveAsync();
}
