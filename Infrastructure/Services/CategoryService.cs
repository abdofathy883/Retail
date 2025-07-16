using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepo<Category> categoryService;
        private readonly MediaUploadService mediaUploadService;
        public CategoryService(IGenericRepo<Category> repo, MediaUploadService uploadService)
        {
            categoryService = repo;
            mediaUploadService = uploadService;
        }
        public async Task<CategoryDTO> CreateCategoryAsync(Create_UpdateCategoryDTO newCategory)
        {
            if (newCategory is null)
                throw new NotFoundException("بيانات التصنيف غير كاملة");

            var image = await mediaUploadService.UploadImage(newCategory.Image, newCategory.Name);
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = newCategory.Name,
                Description = newCategory.Description,
                ImageUrl = image
            };

            await categoryService.AddAsync(category);
            await categoryService.SaveAllAsync();
            return new CategoryDTO 
            { 
                Id = category.Id, 
                Name = category.Name, 
                Description = category.Description, 
                ImageUrl = category.ImageUrl
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new NotFoundException($"رقم معرف غير صحيح, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId);

            if (category is null)
                throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            await categoryService.DeleteByIdAsync(category);
            return await categoryService.SaveAllAsync();
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            var categories = await categoryService.GetAllAsync();

            if (categories is null)
                throw new NotFoundException("لا توجد تصنيفات متاحة");

            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Products = c.Products
            }).ToList();

            return categoryDTOs;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new NotFoundException($"لا يوجد تصنيف يحمل رقم المعرف هذا, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId);

            if (category is null)
                throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                Products = category.Products
            };
        }

        public async Task<bool> SoftDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new NotFoundException($"رقم معرف غير صحيح, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId);

            if (category is null)
                throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            return category.IsDeleted = true;
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid oldCategoryId, Create_UpdateCategoryDTO newCategory)
        {
            if (newCategory is null || oldCategoryId == Guid.Empty)
                throw new NotFoundException("رقم المعرف غير صحيح - بيانات التصنيف غير كاملة");

            var category = await categoryService.GetByIdAsync(oldCategoryId);

            if (category is null)
                throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {oldCategoryId}");

            category.Name = newCategory.Name;
            category.Description = newCategory.Description;
            if (newCategory.Image is not null)
            {
                var newImage = await mediaUploadService.UploadImage(newCategory.Image, newCategory.Name);
                category.ImageUrl = newImage;
            }
            category.UpdatedAt = DateTime.UtcNow;
            categoryService.Update(category);
            await categoryService.SaveAllAsync();
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                Products = category.Products
            };
        }
    }
}
