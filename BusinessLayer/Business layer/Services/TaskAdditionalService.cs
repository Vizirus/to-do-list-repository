using DataLayer.DataLayer.Interfaces;
using WebApi.BusinessLayer.Models;
using AutoMapper;
using BusinessLayer.BusinessLayer.Interfaces;

namespace BusinessLayer.BusinessLayer.Services;

public class TaskAdditionalService : ITaskAdditionalService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public TaskAdditionalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> AddAsync(TaskCommentsModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            model.TaskId > 0 &&
            model.UserId > 0 &&
            !string.IsNullOrEmpty(model.CommentText) &&
            DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            await this.unitOfWork.tasksCommentsRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.TaskComments>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> AddTaskStatusAsync(TaskStatusesModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            !string.IsNullOrEmpty(model.Name))
        {
            await this.unitOfWork.taskStatusesRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.TaskStatuses>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(int modelId)
    {
        var result = false;
        if (modelId > 0)
        {
            var model = await this.GetByIdAsync(modelId);
            if (model is not null)
            {
                result = await this.unitOfWork.tasksCommentsRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<bool> DeleteTaskStatusAsync(int modelId)
    {
        var result = false;
        if (modelId > 0)
        {
            var model = await this.GetTaskStatusByIdAsync(modelId);
            if (model is not null)
            {
                result = await this.unitOfWork.taskStatusesRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TaskCommentsModel>> GetAllAsync()
    {
        var result = await this.unitOfWork.tasksCommentsRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<TaskCommentsModel>(x));
    }

    public async Task<IEnumerable<TaskStatusesModel>> GetAllTaskStatusesAsync()
    {
        var result = await this.unitOfWork.taskStatusesRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<TaskStatusesModel>(x));
    }

    public async Task<TaskCommentsModel> GetByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.tasksCommentsRepository.GetByIdAsync(id);
            return this.mapper.Map<TaskCommentsModel>(result);
        }
        return null!;
    }

    public async Task<TaskStatusesModel> GetTaskStatusByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.taskStatusesRepository.GetByIdAsync(id);
            return this.mapper.Map<TaskStatusesModel>(result);
        }
        return null!;
    }

    public async Task<bool> UpdateAsync(TaskCommentsModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            model.TaskId > 0 &&
            model.UserId > 0 &&
            !string.IsNullOrEmpty(model.CommentText) &&
            DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            this.unitOfWork.tasksCommentsRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.TaskComments>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateTaskStatusAsync(TaskStatusesModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            !string.IsNullOrEmpty(model.Name))
        {
            this.unitOfWork.taskStatusesRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.TaskStatuses>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }
}
