namespace BusinessLayer.BusinessLayer.Interfaces;

public interface IModelService<TModel>
    where TModel : class
{
    Task<bool> AddAsync(TModel model);

    Task<bool> DeleteAsync(int modelId);

    Task<bool> UpdateAsync(TModel model);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> GetByIdAsync(int id);
}
