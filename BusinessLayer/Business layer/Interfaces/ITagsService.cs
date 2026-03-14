using WebApi.BusinessLayer.Models;

namespace BusinessLayer.BusinessLayer.Interfaces;

internal interface ITagsService : IModelService<TagsModel>
{
    // TaskTags CRUD
    Task<bool> AddTaskTagAsync(TaskTagsModel model);

    Task<bool> DeleteTaskTagAsync(int modelId);

    Task<bool> UpdateTaskTagAsync(TaskTagsModel model);

    Task<IEnumerable<TaskTagsModel>> GetAllTaskTagsAsync();

    Task<TaskTagsModel> GetTaskTagByIdAsync(int id);
}
