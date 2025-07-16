using Core.DTOs;
using Core.Models;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(CreateProductDTO newProduct);
        Task<ProductDTO> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct);
        Task<bool> SoftDeleteProductAsync(Guid productId);
        Task<bool> DeleteProductAsync(Guid productId);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(Guid productId);
        Task<List<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId);
    }
}
