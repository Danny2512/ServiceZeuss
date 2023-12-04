using ServiceZeuss.Models;

namespace ServiceZeuss.Services.Zeuss
{
    public interface IZeussService
    {
        Task<List<CategoryApiResponseModel>> GetCategories(string? filter);
        Task<bool> CreateCategory(SynchronizeExternalServiceZeussCategoryModel model);
    }
}
