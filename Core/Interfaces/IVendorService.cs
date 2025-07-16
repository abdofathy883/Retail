using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IVendorService
    {
        Task<List<VendorDTO>> GetAllVendorsAsync();
        Task<VendorDTO> GetVendorByIdAsync(int vendorId);
        Task<VendorDTO> CreateVendorAsync(Create_UpdateVendorDTO vendor);
        Task<VendorDTO> UpdateVendorAsync(int oldVendorId, Create_UpdateVendorDTO newVendor);
        Task<bool> SoftDeleteVendorAsync(int vendorId);
        Task<bool> PerminentDeleteVendorAsync(int vendorId);
    }
}
