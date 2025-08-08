using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.BackStore_Interfaces
{
    public interface ICategoryAdminService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId);
        Task<CategoryDTO> CreateCategoryAsync(Create_UpdateCategoryDTO newCategory);
        Task<CategoryDTO> UpdateCategoryAsync(Guid oldCategoryId, Create_UpdateCategoryDTO newCategory);
        Task<bool> SoftDeleteCategoryAsync(Guid categoryId);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
