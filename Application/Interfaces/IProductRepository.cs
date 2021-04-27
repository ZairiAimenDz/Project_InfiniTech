using Application.Wrappers;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProductsList();
        Task<Product> GetProductAsync(Guid Productid);
        Product GetProduct(Guid Productid);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool Save();
        Task<bool> SaveAsync();
    }
}
