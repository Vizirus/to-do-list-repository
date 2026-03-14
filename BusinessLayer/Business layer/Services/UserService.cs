using DataLayer.DataLayer.Interfaces;
using WebApi.BusinessLayer.Models;
using AutoMapper;
using BusinessLayer.BusinessLayer.Interfaces;

namespace BusinessLayer.BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> AddAsync(UserModel model)
    {
        if (model is not null && model.Id != 0 && !string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.PasswordHash) && DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            await this.unitOfWork.userRepository.AddAsync(this.mapper.Map<DataLayer.DataLayer.Entities.User>(model));
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
                result = await this.unitOfWork.userRepository.DeleteByIdAsync(modelId);
                await this.unitOfWork.SaveAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var result = await this.unitOfWork.userRepository.GetAllAsync();
        return result.Select(x => this.mapper.Map<UserModel>(x));
    }

    public async Task<UserModel> GetByIdAsync(int id)
    {
        if (id > 0)
        {
            var result = await this.unitOfWork.userRepository.GetByIdAsync(id);
            return this.mapper.Map<UserModel>(result);
        }
        return null!;
    }

    public async Task<bool> UpdateAsync(UserModel model)
    {
        if (model is not null && model.Id != 0 && !string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.PasswordHash) && DateTime.Compare(model.CreatedDate, new DateTime(1920, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) > 0)
        {
            this.unitOfWork.userRepository.Update(this.mapper.Map<DataLayer.DataLayer.Entities.User>(model));
            await this.unitOfWork.SaveAsync();
            return true;
        }
        return false;
    }
}
