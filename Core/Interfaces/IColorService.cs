using Core.DTOs.VariantsDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IColorService
    {
        Task<List<Color>> GetColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task<Color> AddNewColorAsync(AddColorDTO color);
        Task<Color> UpdateColorAsync(UpdateColorDTO updateColor);
        Task<Color> VisibilityToggleAsync(int id);
        Task<bool> DeleteColorAsync(int id);
    }
}
