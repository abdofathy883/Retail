using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(Create_UpdateCategoryDTO newCategory);
        Task<CategoryDTO> UpdateCategoryAsync(Guid oldCategoryId, Create_UpdateCategoryDTO newCategory);
        Task<bool> SoftDeleteCategoryAsync(Guid categoryId);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
        Task<List<CategoryDTO>> GetAllCategorysAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId);
    }
}
