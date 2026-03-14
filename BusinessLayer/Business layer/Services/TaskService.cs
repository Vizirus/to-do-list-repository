using DataLayer.DataLayer.Interfaces;
using WebApi.BusinessLayer.Models;
using AutoMapper;
using BusinessLayer.BusinessLayer.Interfaces;

namespace BusinessLayer.BusinessLayer.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> AddAsync(TaskModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            model.ListId > 0 &&
            !string.IsNullOrEmpty(model.TaskName) &&
            !string.IsNullOrEmpty(model.TaskDescription) &&
            model.StatusId > 0 &&
            model.AssigndUserId > 0 &&
            model.TaskStatusIds > 0 &&
            DateTime.Compare(model.TaskStartDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0 &&
            DateTime.Compare(model.TaskFinishDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            await this.unitOfWork.tasksRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.Task>(model));
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
                result = await this.unitOfWork.tasksRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync()
    {
        var result = await this.unitOfWork.tasksRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<TaskModel>(x));
    }

    public async Task<TaskModel> GetByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.tasksRepository.GetByIdAsync(id);
            return this.mapper.Map<TaskModel>(result);
        }
        return null!;
    }

    public async Task<bool> UpdateAsync(TaskModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            model.ListId > 0 &&
            !string.IsNullOrEmpty(model.TaskName) &&
            !string.IsNullOrEmpty(model.TaskDescription) &&
            model.StatusId > 0 &&
            model.AssigndUserId > 0 &&
            model.TaskStatusIds > 0 &&
            DateTime.Compare(model.TaskStartDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0 &&
            DateTime.Compare(model.TaskFinishDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            this.unitOfWork.tasksRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.Task>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }
}
