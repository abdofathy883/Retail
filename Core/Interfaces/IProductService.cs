using Core.DTOs.ProductDTOs;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(Guid productId);
        Task<List<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId);
    }
}
