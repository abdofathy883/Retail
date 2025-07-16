using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class VendorService: IVendorService
    {
        private readonly IGenericRepo<Vendor> vendorRepo;
        public VendorService(IGenericRepo<Vendor> vendorRepo)
        {
            this.vendorRepo = vendorRepo;
        }

        public async Task<VendorDTO> CreateVendorAsync(Create_UpdateVendorDTO vendor)
        {
            if (vendor is null)
                throw new InValidObjectException("");

            var newVendor = new Vendor
            {
                Name = vendor.Name,
                ContactEmail = vendor.ContactEmail,
                ContactPhone = vendor.ContactPhone,
                Address = vendor.Address
            };

            await vendorRepo.AddAsync(newVendor);
            await vendorRepo.SaveAllAsync();

            return new VendorDTO
            {
                Id = newVendor.Id,
                Name = newVendor.Name,
                ContactEmail = newVendor.ContactEmail,
                ContactPhone = newVendor.ContactPhone,
                Address = newVendor.Address
            };
        }

        public async Task<List<VendorDTO>> GetAllVendorsAsync()
        {
            var vendors = await vendorRepo.GetAllAsync();

            if (vendors == null)
                throw new NotFoundException("No vendors found.");

            var vendorDTOs = vendors.Select(v => new VendorDTO
            {
                Id = v.Id,
                Name = v.Name,
                ContactEmail = v.ContactEmail,
                ContactPhone = v.ContactPhone,
                Address = v.Address
            }).ToList();

            return vendorDTOs;
        }

        public async Task<VendorDTO> GetVendorByIdAsync(int vendorId)
        {
            if (vendorId <= 0)
                throw new InValidObjectException("Invalid vendor ID.");

            var vendor = await vendorRepo.GetByIdAsync(vendorId);

            if (vendor == null)
                throw new NotFoundException($"Vendor with ID {vendorId} not found.");

            return new VendorDTO
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactEmail = vendor.ContactEmail,
                ContactPhone = vendor.ContactPhone,
                Address = vendor.Address
            };
        }

        public async Task<bool> PerminentDeleteVendorAsync(int vendorId)
        {
            if (vendorId <= 0)
                throw new InValidObjectException("Invalid vendor ID.");

            await vendorRepo.DeleteByIdAsync(vendorId);
            return await vendorRepo.SaveAllAsync();
        }

        public async Task<bool> SoftDeleteVendorAsync(int vendorId)
        {
            if (vendorId <= 0)
                throw new InValidObjectException("Invalid vendor ID.");

            var vendor = await vendorRepo.GetByIdAsync(vendorId);
            if (vendor == null)
                throw new NotFoundException($"Vendor with ID {vendorId} not found.");
            vendor.IsDeleted = true;
            vendorRepo.Update(vendor);
            return await vendorRepo.SaveAllAsync();
        }

        public async Task<VendorDTO> UpdateVendorAsync(int oldVendorId, Create_UpdateVendorDTO newVendor)
        {
            if (oldVendorId <= 0)
                throw new InValidObjectException("Invalid vendor ID.");

            var vendor = await vendorRepo.GetByIdAsync(oldVendorId);

            if (vendor == null)
                throw new NotFoundException($"Vendor with ID {oldVendorId} not found.");

            vendor.Name = newVendor.Name ?? vendor.Name;
            vendor.ContactEmail = newVendor.ContactEmail ?? vendor.ContactEmail;
            vendor.ContactPhone = newVendor.ContactPhone ?? vendor.ContactPhone;
            vendor.Address = newVendor.Address ?? vendor.Address;

            vendor.UpdatedAt = DateTime.UtcNow;

            vendorRepo.Update(vendor);
            await vendorRepo.SaveAllAsync();

            return new VendorDTO
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactEmail = vendor.ContactEmail,
                ContactPhone = vendor.ContactPhone,
                Address = vendor.Address
            };


        }
    }
}
