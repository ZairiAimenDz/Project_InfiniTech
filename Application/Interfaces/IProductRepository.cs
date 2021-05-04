using Application.Dtos.Product;
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
        Task<PagedList<Product>> GetProductsList(ProductParameters parameters);
        Task<Product> GetProductAsync(Guid Productid);
        Product GetProduct(Guid Productid);
        void AddProduct(Product product);
        Task AddProductAsync(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool Save();
        Task<bool> SaveAsync();
    }
}
