using Core.DTOs.VariantsDTO;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class ColorService : IColorService
    {
        private readonly IGenericRepo<Color> colorRepo;

        public ColorService(IGenericRepo<Color> _colorRepo)
        {
            colorRepo = _colorRepo;
        }

        public async Task<List<Color>> GetColorsAsync()
        {
            var colors = await colorRepo.GetAllForAdminAsync();
            return colors.ToList();
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            if (id <= 0)
                throw new InValidPropertyIdException($"لا يوجد لون يحمل هذا المعرف, {id}");

            var color = await colorRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على اللون بهذا المعرف, {id}");

            return color;
        }

        public async Task<Color> AddNewColorAsync(AddColorDTO color)
        {
            if (color is null)
                throw new InValidObjectException("بيانات اللون فارغة او غير صحيحةو برجاء التأكد من البيانات واعادة المحاولة");

            var existing = await colorRepo.ExistsAsync(c => c.Name.ToLower().Trim() == color.Name && c.ColorCode.ToLower().Trim() == color.ColorCode);

            if (existing)
                throw new ObjectAlreadyExistsException("اللون موجود بالفعل");

            var newColor = new Color
            {
                Name = color.Name,
                ColorCode = color.ColorCode,
            };
            await colorRepo.AddAsync(newColor);
            await colorRepo.SaveAllAsync();
            return newColor;
        }

        public async Task<Color> UpdateColorAsync(UpdateColorDTO updateColor)
        {
            if (updateColor.Id <= 0)
                throw new InValidPropertyIdException($"معرف اللون غير صحيح, {updateColor.Id}");

            if (string.IsNullOrWhiteSpace(updateColor.NewName) || string.IsNullOrWhiteSpace(updateColor.NewColorCode))
                throw new InValidObjectException("لا يمكن استبدال اللون ببيانات فارغة");

            var color = await colorRepo.GetByIdAsync(updateColor.Id)
                ?? throw new InValidObjectException($"لم يتم العثور على اللون بهذا المعرف, {updateColor.Id}");

            color.Name = updateColor.NewName ?? color.Name;
            color.ColorCode = updateColor.NewColorCode ?? color.ColorCode;
            colorRepo.Update(color);
            await colorRepo.SaveAllAsync();
            return color;
        }

        public async Task<bool> DeleteColorAsync(int id)
        {
            if (id == 0)
                throw new InValidPropertyIdException($"معرف اللون غير صحيح, {id}");

            var color = await colorRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على اللون بهذا المعرف, {id}");

            await colorRepo.DeleteByIdAsync(id);
            return await colorRepo.SaveAllAsync();
        }

        public async Task<Color> VisibilityToggleAsync(int id)
        {
            if (id <= 0)
                throw new InValidPropertyIdException($"معرف اللون غير صحيح, {id}");

            var color = await colorRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على اللون بهذا المعرف, {id}");

            color.IsDeleted = !color.IsDeleted;
            colorRepo.Update(color);
            await colorRepo.SaveAllAsync();
            return color;
        }
    }
}
