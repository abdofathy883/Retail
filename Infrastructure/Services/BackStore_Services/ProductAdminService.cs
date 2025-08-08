using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.DTOs.ProductDTOs.AdminLevel;
using Core.Interfaces;
using Core.Interfaces.BackStore_Interfaces;
using Core.Models;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BackStore_Services
{
    public class ProductAdminService: IProductAdminService
    {
        private readonly IGenericRepo<Product> productRepo;
        private readonly IGenericRepo<Category> categoryRepo;
        private readonly MediaUploadService mediaUploadService;
        private readonly IMapper mapper;
        public ProductAdminService(
            IGenericRepo<Product> repo,
            IGenericRepo<Category> catRepo,
            MediaUploadService mediaUploadService,
            IMapper _mapper)
        {
            productRepo = repo;
            this.mediaUploadService = mediaUploadService;
            mapper = _mapper;
            categoryRepo = catRepo;
        }
    
        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO newProduct)
        {
            if (newProduct is null)
                throw new InValidObjectException("لا توجد بيانات للمنتج لاضافته");

            Console.WriteLine("Category id is ====>>>>",newProduct.CategoryId);
            Console.WriteLine(newProduct.CategoryId.GetType());
            var category = await categoryRepo.GetByIdAsync(newProduct.CategoryId)
                ?? throw new InValidObjectException($"لا يوجد فئة بهذا الرقم المعرف, {newProduct.CategoryId}");

            var duplicateCombos = newProduct.ProductVariants
                .GroupBy(v => new { v.ColorId, v.SizeId })
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateCombos.Any())
                throw new InValidObjectException("توجد متغيرات مكررة لنفس اللون والمقاس");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = newProduct.Name,
                Description = newProduct.Description,
                CategoryId = newProduct.CategoryId,
                ProductVariants = new List<ProductVariant>()
            };

            foreach (var variantDto in newProduct.ProductVariants)
            {
                var variant = new ProductVariant
                {
                    ColorId = variantDto.ColorId,
                    SizeId = variantDto.SizeId,
                    OriginalPrice = variantDto.OriginalPrice,
                    SalePrice = variantDto.SalePrice,
                    WholesalePrice = variantDto.WholesalePrice,
                    Stock = variantDto.Stock,
                    SKU = variantDto.SKU,
                    Barcode = variantDto.Barcode,
                    IsDeleted = variantDto.IsDeleted,
                    IsFeatured = variantDto.IsFeatured,
                };

                var imageUrl = await mediaUploadService.UploadImage(variantDto.VariantImage.Image, newProduct.Name);

                variant.VariantImage = new ProductImage
                {
                    Url = imageUrl,
                    AltText = variantDto.VariantImage.AltText ?? "test alt text",
                    IsFeatured = variantDto.VariantImage.IsFeatured
                };
                product.ProductVariants.Add(variant);
            }

            await productRepo.AddAsync(product);
            await productRepo.SaveAllAsync();

            return mapper.Map<ProductDTO>(product);
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
            var products = await productRepo.GetAllForAdminAsync()
                ?? throw new InValidObjectException("لا توجد منتجات متاحة");

            //var productsWithVariants = products.inc

            return mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"الرقم المعرف للمنتج غير صحيح, {productId}");

            var product = await productRepo.GetByIdAsync(productId)
                ?? throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {productId}");

            return mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> SoftDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new InValidPropertyIdException($"لا يوجد منتج يحمل هذا الرقم المعرف, {productId}");

            var product = await productRepo.GetByIdAsync(productId)
                ?? throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {productId}");

            product.IsDeleted = true;
            productRepo.Update(product);
            return await productRepo.SaveAllAsync();
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct)
        {
            if (oldProductId == Guid.Empty)
                throw new InValidPropertyIdException("الرقم المعرف للمنتج خاطئ");

            if (newProduct is null)
                throw new InValidObjectException("لا توجد بيانات للمنتج لتحديثه");

            var oldProduct = await productRepo.GetProductWithVariantsAsync(oldProductId) // 🔁 must include variants + images
                ?? throw new InValidObjectException($"لم يتم العثور على المنتج الذي يحمل رقم المعرف, {oldProductId}");

            oldProduct.Name = newProduct.Name;
            oldProduct.Description = newProduct.Description;
            oldProduct.CategoryId = newProduct.CategoryId;

            // ============================
            // === Variant Tracking Logic
            // ============================

            var inputVariantIds = newProduct.ProductVariants
                .Where(v => v.Id > 0)
                .Select(v => v.Id)
                .ToHashSet();

            var existingVariants = oldProduct.ProductVariants.ToList();

            // 🧹 Remove missing variants
            var toRemove = existingVariants
                .Where(ev => !inputVariantIds.Contains(ev.Id))
                .ToList();

            foreach (var variant in toRemove)
                oldProduct.ProductVariants.Remove(variant);

            // 🔁 Update or Add
            foreach (var vDto in newProduct.ProductVariants)
            {
                var existing = existingVariants.FirstOrDefault(v => v.Id == vDto.Id);

                if (existing is not null)
                {
                    // === Update existing ===
                    existing.ColorId = vDto.ColorId;
                    existing.SizeId = vDto.SizeId;
                    existing.OriginalPrice = vDto.OriginalPrice;
                    existing.SalePrice = vDto.SalePrice;
                    existing.WholesalePrice = vDto.WholesalePrice;
                    existing.Stock = vDto.Stock;
                    existing.SKU = vDto.SKU;
                    existing.Barcode = vDto.Barcode;
                    existing.IsDeleted = vDto.IsDeleted;
                    existing.IsFeatured = vDto.IsFeatured;

                    // 💾 Update images (you may want to diff here too)
                    //existing.ProductVariantImages.Clear();
                    //foreach (var imgDto in vDto.ProductVariantImages)
                    //{
                    //    var url = await mediaUploadService.UploadImage(imgDto.Image, newProduct.Name);
                    //    existing.ProductVariantImages.Add(new ProductImage
                    //    {
                    //        Url = url,
                    //        AltText = imgDto.AltText,
                    //        IsFeatured = imgDto.IsFeatured
                    //    });
                    //}
                }
                else
                {
                    // === Add new variant ===
                    var newVariant = new ProductVariant
                    {
                        ColorId = vDto.ColorId,
                        SizeId = vDto.SizeId,
                        OriginalPrice = vDto.OriginalPrice,
                        SalePrice = vDto.SalePrice,
                        WholesalePrice = vDto.WholesalePrice,
                        Stock = vDto.Stock,
                        SKU = vDto.SKU,
                        Barcode = vDto.Barcode,
                        IsDeleted = vDto.IsDeleted,
                        IsFeatured = vDto.IsFeatured,
                        //ProductVariantImages = new List<ProductImage>()
                    };

                    //var url = await mediaUploadService.UploadImage(newVariant.VariantImage, newProduct.Name);
                    //foreach (var imgDto in vDto.ProductVariantImages)
                    //{
                    //    newVariant.ProductVariantImages.Add(new ProductImage
                    //    {
                    //        Url = url,
                    //        AltText = imgDto.AltText,
                    //        IsFeatured = imgDto.IsFeatured
                    //    });
                    //}

                    oldProduct.ProductVariants.Add(newVariant);
                }
            }

            oldProduct.UpdatedAt = DateTime.UtcNow;
            productRepo.Update(oldProduct);
            await productRepo.SaveAllAsync();

            return mapper.Map<ProductDTO>(oldProduct);
        }

        
    }
}
