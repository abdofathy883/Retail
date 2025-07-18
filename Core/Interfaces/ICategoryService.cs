using Core.DTOs;

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
