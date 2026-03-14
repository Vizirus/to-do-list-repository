using DataLayer.DataLayer.Interfaces;
using WebApi.BusinessLayer.Models;
using AutoMapper;
using BusinessLayer.BusinessLayer.Interfaces;

namespace BusinessLayer.BusinessLayer.Services;

public class ListService : IListService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ListService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> AddAsync(ListsModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            !string.IsNullOrEmpty(model.ListName) &&
            model.CreatedByUser > 0 &&
            DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            await this.unitOfWork.listsRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.Lists>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> AddSahredListAsync(SharedListsModel model)
    {
        if (model is not null && model.Id > 0 && model.ToDoListId > 0 && model.UserWhoAssignsIs > 0 && model.AssignedUserId > 0)
        {
            await this.unitOfWork.sharedListsRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.SharedLists>(model));
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
                result = await this.unitOfWork.listsRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<bool> DeleteSahredListAsync(int modelId)
    {
        var result = false;
        if (modelId > 0)
        {
            var model = await this.GetSahredListByIdAsync(modelId);
            if (model is not null)
            {
                result = await this.unitOfWork.sharedListsRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<ListsModel>> GetAllAsync()
    {
        var result = await this.unitOfWork.listsRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<ListsModel>(x));
    }

    public async Task<IEnumerable<SharedListsModel>> GetAllTSahredListsAsync()
    {
        var result = await this.unitOfWork.sharedListsRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<SharedListsModel>(x));
    }

    public async Task<ListsModel> GetByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.listsRepository.GetByIdAsync(id);
            return this.mapper.Map<ListsModel>(result);
        }
        return null!;
    }

    public async Task<SharedListsModel> GetSahredListByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.sharedListsRepository.GetByIdAsync(id);
            return this.mapper.Map<SharedListsModel>(result);
        }
        return null!;
    }

    public async Task<bool> UpdateAsync(ListsModel model)
    {
        if (model is not null &&
            model.Id > 0 &&
            !string.IsNullOrEmpty(model.ListName) &&
            model.CreatedByUser > 0 &&
            DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            this.unitOfWork.listsRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.Lists>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateSahredListgAsync(SharedListsModel model)
    {
        if (model is not null && model.Id > 0 && model.ToDoListId > 0 && model.UserWhoAssignsIs > 0 && model.AssignedUserId > 0)
        {
            this.unitOfWork.sharedListsRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.SharedLists>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }
}
