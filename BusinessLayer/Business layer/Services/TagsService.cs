using AutoMapper;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using BusinessLayer.BusinessLayer.Interfaces;
using WebApi.BusinessLayer.Models;

namespace BusinessLayer.BusinessLayer.Services;

public class TagsService : ITagsService
{
    private readonly IUnitOfWork unitOfWork;

    private readonly IMapper mapper;

    public TagsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> AddAsync(TagsModel model)
    {
        if (model is not null && model.Id != 0 && !string.IsNullOrEmpty(model.Name))
        {
            await this.unitOfWork.tagsRepository.AddAsync(this.mapper.Map<Tags>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> AddTaskTagAsync(TaskTagsModel model)
    {
        if (model is not null && model.Id > 0 && model.TaskId > 0 && model.TagId > 0)
        {
            await this.unitOfWork.taskTagsRepository.AddAsync(this.mapper.Map<TaskTags>(model));
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
                result = await this.unitOfWork.tagsRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }

        return result;
    }

    public async Task<bool> DeleteTaskTagAsync(int modelId)
    {
        var result = false;
        if (modelId > 0)
        {
            var model = await this.GetTaskTagByIdAsync(modelId);
            if (model is not null)
            {
                result = await this.unitOfWork.taskTagsRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }

        return result;
    }

    public async Task<IEnumerable<TagsModel>> GetAllAsync()
    {
        var result = await this.unitOfWork.tagsRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<TagsModel>(x));
    }

    public async Task<IEnumerable<TaskTagsModel>> GetAllTaskTagsAsync()
    {
        var result = await this.unitOfWork.taskTagsRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<TaskTagsModel>(x));
    }

    public async Task<TagsModel> GetByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.tagsRepository.GetByIdAsync(id);
            return this.mapper.Map<TagsModel>(result);
        }

        return null!;
    }

    public async Task<TaskTagsModel> GetTaskTagByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.taskTagsRepository.GetByIdAsync(id);
            return this.mapper.Map<TaskTagsModel>(result);
        }

        return null!;
    }

    public async Task<bool> UpdateAsync(TagsModel model)
    {
        if (model is not null && model.Id != 0 && !string.IsNullOrEmpty(model.Name))
        {
            this.unitOfWork.tagsRepository.Update(this.mapper.Map<Tags>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateTaskTagAsync(TaskTagsModel model)
    {
        if (model is not null && model.Id > 0 && model.TaskId > 0 && model.TagId > 0)
        {
            this.unitOfWork.taskTagsRepository.Update(this.mapper.Map<TaskTags>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }

        return false;
    }
}
