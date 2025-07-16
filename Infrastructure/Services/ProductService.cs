using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class ProductService: IProductService
    {
        private readonly IGenericRepo<Product> productRepo;
        private readonly MediaUploadService mediaUploadService;
        public ProductService(IGenericRepo<Product> repo, MediaUploadService mediaUploadService)
        {
            productRepo = repo;
            this.mediaUploadService = mediaUploadService;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO newProduct)
        {
            if (newProduct is null)
                throw new InValidObjectException("لا توجد بيانات للمنتج لاضافته");

            if (newProduct.ImageUrls is null || newProduct.ImageUrls.Count == 0)
                throw new NotFoundException("لا يوجد صور لهذا المنتج");

            var images = new List<ProductImage>();

            foreach (var image in newProduct.ImageUrls)
            {
                var uploadedUrl = await mediaUploadService.UploadImage(image, newProduct.Name);
                images.Add(new ProductImage { Url = uploadedUrl });
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = newProduct.Name,
                Description = newProduct.Description,
                ImageUrls = images,
                ProductType = newProduct.ProductType,
                ProductVarients = newProduct.ProductVarients.Select(v => new ProductVarient
                {
                    ColorId = v.ColorId,
                    SizeId = v.SizeId,
                    Price = v.Price,
                    Stock = v.Stock,
                    SKU = v.SKU,
                    Barcode = v.Barcode
                }).ToList(),
                CategoryId = newProduct.CategoryId
            };

            await productRepo.AddAsync(product);
            await productRepo.SaveAllAsync();

            product.ProductVarients.ForEach(v => v.ProductId = product.Id);
            productRepo.Update(product);
            await productRepo.SaveAllAsync();

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrls = product.ImageUrls,
                ProductType = product.ProductType,
                ProductVarients = product.ProductVarients,
                CategoryId = product.CategoryId
            };
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد منتج يحمل رقم المعرف هذا, {productId}");

            bool result = await productRepo.DeleteByIdAsync(productId);
            return await productRepo.SaveAllAsync();
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await productRepo.GetAllAsync();
            
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrls = p.ImageUrls,
                Description = p.Description,
                ProductType = p.ProductType,
                ProductVarients = p.ProductVarients,
                CategoryId = p.CategoryId,
                NuOfPurchases = p.NuOfPurchases,
                NuOfPutInCart = p.NuOfPutInCart,
                NuOfPutInWishList = p.NuOfPutInWishList
            }).ToList();

            return productDTOs;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"رقم معرف غير صحيح للمنتج, {productId}");

            var product = await productRepo.GetByIdAsync(productId);

            if (product is null)
            {
                throw new NotFoundException($"لا يوجد منتج يحمل هذا الرقم المعرف, {productId}");
            }

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrls = product.ImageUrls,
                ProductType = product.ProductType,
                ProductVarients = product.ProductVarients,
                CategoryId = product.CategoryId,
                NuOfPurchases = product.NuOfPurchases,
                NuOfPutInCart = product.NuOfPutInCart,
                NuOfPutInWishList = product.NuOfPutInWishList
            };
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد تصنيف يحمل هذا المعرف, {categoryId}");

            var products = await productRepo.FindAsync(p => p.CategoryId == categoryId && p.IsDeleted == false);
            
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrls = p.ImageUrls,
                Description = p.Description,
                ProductType = p.ProductType,
                ProductVarients = p.ProductVarients,
                CategoryId = p.CategoryId,
                NuOfPurchases = p.NuOfPurchases,
                NuOfPutInCart = p.NuOfPutInCart,
                NuOfPutInWishList = p.NuOfPutInWishList
            }).ToList();
            return productDTOs;
        }

        public async Task<bool> SoftDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد منتج يحمل هذا الرقم المعرف, {productId}");

            var product = await productRepo.GetByIdAsync(productId);
            if (product is null)
                throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {productId}");

            product.IsDeleted = true;
            productRepo.Update(product);
            return await productRepo.SaveAllAsync();
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct)
        {
            if (oldProductId == Guid.Empty || newProduct is null)
                throw new InValidPropertyIdException($"الرقم المعرف للمنتج خاطئ, او بيانات المنتج الجديد غير واضحة");

            var oldProduct = await productRepo.GetByIdAsync(oldProductId);

            if (oldProduct is null || oldProduct.IsDeleted == true)
                throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {oldProductId}");

            oldProduct.Name = newProduct.Name;
            oldProduct.Description = newProduct.Description;
            oldProduct.CategoryId = newProduct.CategoryId;

            if (newProduct.ImageUrls is not null)
            {
                var images = new List<ProductImage>();

                foreach (var image in newProduct.ImageUrls)
                {
                    var uploadedUrl = await mediaUploadService.UploadImage(image, newProduct.Name);
                    images.Add(new ProductImage { Url = uploadedUrl });
                }

                oldProduct.ImageUrls = images;
            }

            if (newProduct.ProductVarients is not null)
            {
                var varients = new List<ProductVarient>();
                foreach (var varient in newProduct.ProductVarients)
                {
                    varients.Add(new ProductVarient
                    {
                        ColorId = varient.ColorId,
                        SizeId = varient.SizeId,
                        Price = varient.Price,
                        Stock = varient.Stock,
                        SKU = varient.SKU,
                        Barcode = varient.Barcode
                    });
                }
                oldProduct.ProductVarients = varients;
            }

            oldProduct.UpdatedAt = DateTime.UtcNow;
            productRepo.Update(oldProduct);
            await productRepo.SaveAllAsync();
            return new ProductDTO
            {
                Id = oldProduct.Id,
                Name = newProduct.Name,
                Description = oldProduct.Description,
                ImageUrls = oldProduct.ImageUrls,
                ProductType = oldProduct.ProductType,
                ProductVarients = oldProduct.ProductVarients,
                CategoryId = oldProduct.CategoryId,
                NuOfPurchases = oldProduct.NuOfPurchases,
                NuOfPutInCart = oldProduct.NuOfPutInCart,
                NuOfPutInWishList = oldProduct.NuOfPutInWishList
            };
        }
    }
}
