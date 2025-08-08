using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class ProductService: IProductService
    {
        private readonly IGenericRepo<Product> productRepo;
        private readonly IMapper mapper;
        public ProductService(IGenericRepo<Product> repo,
            IMapper _mapper)
        {
            productRepo = repo;
            mapper = _mapper;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await productRepo.FindAsync(p => !p.IsDeleted)
                ?? throw new InValidObjectException("لا يوجد منتجات متاحة");

            return mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"رقم معرف غير صحيح للمنتج, {productId}");

            var product = await productRepo.GetByIdAsync(productId)
                ?? throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {productId}");

            return mapper.Map<ProductDTO>(product);
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد تصنيف يحمل هذا المعرف, {categoryId}");

            var products = await productRepo.FindAsync(p => p.CategoryId == categoryId && p.IsDeleted == false);
            
            return mapper.Map<List<ProductDTO>>(products);
        }
    }
}
