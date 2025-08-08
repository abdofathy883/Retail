using Core.DTOs.ProductDTOs;
using Core.DTOs.ProductDTOs.AdminLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.BackStore_Interfaces
{
    public interface IProductAdminService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(Guid productId);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO newProduct);
        Task<ProductDTO> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct);
        Task<bool> SoftDeleteProductAsync(Guid productId);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
