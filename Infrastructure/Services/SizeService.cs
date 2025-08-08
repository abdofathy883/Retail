using Core.DTOs.VariantsDTO;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class SizeService : ISizeService
    {
        private readonly IGenericRepo<Size> sizeRepo;

        public SizeService(IGenericRepo<Size> _sizeRepo)
        {
            sizeRepo = _sizeRepo;
        }
        public async Task<List<Size>> GetSizesAsync()
        {
            var sizes = await sizeRepo.GetAllForAdminAsync();
            return sizes.ToList();
        }

        public async Task<Size> GetSizeByIdAsync(int id)
        {
            if (id <= 0)
                throw new InValidPropertyIdException($"معرف المقاس غير صحيح, {id}");

            var size = await sizeRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على المقاس بهذا المعرف, {id}");

            return size;
        }

        public async Task<Size> AddNewSizeAsync(AddSizeDTO size)
        {
            if (size is null)
                throw new InValidObjectException("اسم المقاس لا يمكن ان يكون فارغا");

            var existing = await sizeRepo.ExistsAsync(s => s.Name.ToLower().Trim() == size.Name.ToLower().Trim());

            if (existing)
                throw new ObjectAlreadyExistsException("المقاس موجود بالفعل");

            var newSize = new Size
            {
                Name = size.Name
            };

            await sizeRepo.AddAsync(newSize);
            await sizeRepo.SaveAllAsync();
            return newSize;
        }

        public async Task<Size> UpdateSizeAsync(int id, string newName)
        {
            if (id <= 0)
                throw new InValidPropertyIdException($"معرف المقاس غير صحيح, {id}");

            if (string.IsNullOrWhiteSpace(newName))
                throw new InValidObjectException("اسم المقاس لا يمكن ان يكون فارغا");

            var size = await sizeRepo.GetByIdAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على المقاس بهذا المعرف, {id}");

            size.Name = newName ?? size.Name;
            sizeRepo.Update(size);
            await sizeRepo.SaveAllAsync();
            return size;
        }

        public async Task<bool> DeleteSizeAsync(int id)
        {
            if (id == 0)
                throw new InValidPropertyIdException($"معرف المقاس غير صحيح, {id}");

            var size = sizeRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على المقاس بهذا المعرف, {id}");

            await sizeRepo.DeleteByIdAsync(size);
            return await sizeRepo.SaveAllAsync();
        }

        public async Task<Size> ToggleVisibilityAsync(int id)
        {
            if (id == 0)
                throw new InValidPropertyIdException($"معرف المقاس غير صحيح, {id}");

            var size = await sizeRepo.GetByIdForAdminAsync(id)
                ?? throw new InValidObjectException($"لم يتم العثور على المقاس بهذا المعرف, {id}");

            size.IsDeleted = !size.IsDeleted;
            sizeRepo.Update(size);
            await sizeRepo.SaveAllAsync();
            return size;
        }        
    }
}
