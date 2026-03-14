
using WebApi.BusinessLayer.Models;


namespace BusinessLayer.BusinessLayer.Interfaces;
public interface IListService : IModelService<ListsModel>
{
    Task<bool> AddSahredListAsync(SharedListsModel model);

    Task<bool> DeleteSahredListAsync(int modelId);

    Task<bool> UpdateSahredListgAsync(SharedListsModel model);

    Task<IEnumerable<SharedListsModel>> GetAllTSahredListsAsync();

    Task<SharedListsModel> GetSahredListByIdAsync(int id);
}
