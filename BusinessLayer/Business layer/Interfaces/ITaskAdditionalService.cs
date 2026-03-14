using WebApi.BusinessLayer.Models;

namespace BusinessLayer.BusinessLayer.Interfaces;
public interface ITaskAdditionalService : IModelService<TaskCommentsModel>
{
    // TaskStatuses CRUD
    Task<bool> AddTaskStatusAsync(TaskStatusesModel model);

    Task<bool> DeleteTaskStatusAsync(int modelId);

    Task<bool> UpdateTaskStatusAsync(TaskStatusesModel model);

    Task<IEnumerable<TaskStatusesModel>> GetAllTaskStatusesAsync();

    Task<TaskStatusesModel> GetTaskStatusByIdAsync(int id);
}
