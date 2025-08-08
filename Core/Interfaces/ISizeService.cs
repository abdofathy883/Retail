using Core.DTOs.VariantsDTO;
using Core.Models;

namespace Core.Interfaces
{
    public interface ISizeService
    {
        Task<List<Size>> GetSizesAsync();
        Task<Size> GetSizeByIdAsync(int id);
        Task<Size> AddNewSizeAsync(AddSizeDTO newSize);
        Task<Size> UpdateSizeAsync(int id, string newName);
        Task<Size> ToggleVisibilityAsync(int id);
        Task<bool> DeleteSizeAsync(int id);
    }
}
