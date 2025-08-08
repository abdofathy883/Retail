using Core.DTOs;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategorysAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId);
    }
}
