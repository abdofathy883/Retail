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

        
    }
}
