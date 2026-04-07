using HDKTech.Models;

namespace HDKTech.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<SanPham>> GetAllWithImagesAsync();
        Task<SanPham?> GetProductWithDetailsAsync(int id);
        Task<List<SanPham>> GetRelatedProductsAsync(int categoryId, int currentProductId, int limit);
        Task<List<SanPham>> FilterProductsAsync(ProductFilterModel filter);
        Task<List<string>> GetUniqueBrandsByCategory(int categoryId);
        Task<List<string>> GetUniqueCpuLines();
    }
}
