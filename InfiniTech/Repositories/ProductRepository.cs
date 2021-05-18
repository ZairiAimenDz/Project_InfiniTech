using Application.Dtos.Product;
using Application.Interfaces;
using Application.Wrappers;
using Domain;
using InfiniTech.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task AddProductAsync(Product product)
        {
            product.DateAdded = DateTime.Now;
            await _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public Product GetProduct(Guid Productid)
        {
            return _context.Products.Include(p => p.Brand).Include(p => p.Category)
                    .FirstOrDefault(p=>p.id == Productid);
        }

        public Task<Product> GetProductAsync(Guid Productid)
        {
            return _context.Products.Include(p => p.Brand).Include(p => p.Category)
                    .FirstOrDefaultAsync(p=>p.id==Productid);
        }

        public async Task<PagedList<Product>> GetProductsList(ProductParameters parameters)
        {
            if (parameters == null)
            {
                return null;
            }

            var collection = _context.Products.OrderByDescending(p=>p.DateAdded).Include(p=>p.Brand).Include(p=>p.Category) as IQueryable<Product>;
            // Filtering By The Entered Details :
            // By Name :
            collection = string.IsNullOrEmpty(parameters.name) ? collection :
                            collection.Where(p => p.Name.ToLower()
                                    .Contains(parameters.name.ToLower()));
            // By Category :
            collection = parameters.Categoryid == Guid.Empty ? collection :
                            collection.Where(p=>p.CategoryId == parameters.Categoryid);

            // By Min Price
            collection = parameters.minprice == 0 ? collection :
                            collection.Where(b => b.Price > parameters.minprice );

            // By Max Price
            collection = parameters.maxprice == 0 ? collection :
                            collection.Where(b => b.Price < parameters.maxprice );

            /* Delete This And Add More Filters Here After Adding Them To THe Product Parameters Class : */

            //// End Of Filtering /////
            return await PagedList<Product>.CreateAsync(collection,
                parameters.PageNumber,
                parameters.PageSize);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
