using AutoMapper;
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
        private readonly IMapper mapper;
        public CategoryService(IGenericRepo<Category> repo, 
            MediaUploadService uploadService,
            IMapper _mapper)
        {
            categoryService = repo;
            mediaUploadService = uploadService;
            mapper = _mapper;
        }
        public async Task<CategoryDTO> CreateCategoryAsync(Create_UpdateCategoryDTO newCategory)
        {
            if (newCategory is null)
                throw new InValidObjectException("بيانات التصنيف غير كاملة");

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
            return mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InValidPropertyIdException($"رقم معرف غير صحيح, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId)
                ?? throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            await categoryService.DeleteByIdAsync(category);
            return await categoryService.SaveAllAsync();
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            var categories = await categoryService.GetAllAsync()
                ?? throw new NotFoundException("لا توجد تصنيفات متاحة");

            return mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد تصنيف يحمل رقم المعرف هذا, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId)
                ?? throw new InValidObjectException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            return mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> SoftDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InValidPropertyIdException($"رقم معرف غير صحيح, {categoryId}");

            var category = await categoryService.GetByIdAsync(categoryId)
                ?? throw new InValidObjectException($"لم يتم العثور على تصنيف بهذا المعرف, {categoryId}");

            category.IsDeleted = true;
            categoryService.Update(category);
            return await categoryService.SaveAllAsync();
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid oldCategoryId, Create_UpdateCategoryDTO newCategory)
        {
            if (newCategory is null || oldCategoryId == Guid.Empty)
                throw new NotFoundException("رقم المعرف غير صحيح - بيانات التصنيف غير كاملة");

            var category = await categoryService.GetByIdAsync(oldCategoryId)
                ?? throw new NotFoundException($"لم يتم العثور على تصنيف بهذا المعرف, {oldCategoryId}");

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
            return mapper.Map<CategoryDTO>(category);
        }
    }
}
