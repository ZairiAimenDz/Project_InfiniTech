using Application.Dtos.Brand;
using Application.Wrappers;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetBrandsList();
        Task<Brand> GetBrandAsync(Guid? brandid);
        Brand GetBrand(Guid? brandid);
        void AddBrand(Brand brand);
        Task AddBrandAsync(Brand brand);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);
        bool Save();
        Task<bool> SaveAsync();
    }
}
