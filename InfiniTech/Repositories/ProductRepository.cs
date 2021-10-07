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
            await _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetLatestProductsList()
        {
            return await _context.Products.OrderByDescending(p=>p.DateAdded).Take(4).ToListAsync();
        }

        public Product GetProduct(Guid Productid)
        {
            return _context.Products.Include(p => p.Brand).Include(p => p.Category)
                .Include(p=>p.ProductImages)
                    .FirstOrDefault(p=>p.ID == Productid);
        }

        public Task<Product> GetProductAsync(Guid Productid)
        {
            return _context.Products.Include(p => p.Brand).Include(p => p.Category)
                .Include(p=>p.ProductImages)
                    .FirstOrDefaultAsync(p=>p.ID==Productid);
        }

        public async Task<PagedList<Product>> GetProductsList(ProductParameters parameters)
        {
            if (parameters == null)
            {
                return null;
            }

            var collection = _context.Products.OrderByDescending(p=>p.DateAdded).Include(p=>p.Brand).Include(p=>p.Category) as IQueryable<Product>;
            // Filtering By The Entered Details :
            collection = parameters.ShowInvisible ? collection : collection.Where(p => p.isVisible);
            // By SearchTerm :
            collection = string.IsNullOrEmpty(parameters.SearchTerm) ? collection :
                            collection.Where(p =>
                            p.Name.ToLower().Contains(parameters.SearchTerm.ToLower())
                            || p.Description.ToLower().Contains(parameters.SearchTerm.ToLower())
                            || p.ShortDescription.ToLower().Contains(parameters.SearchTerm.ToLower())
                            || p.Brand.Name.ToLower().Contains(parameters.SearchTerm.ToLower())
                            || p.Category.Name.ToLower().Contains(parameters.SearchTerm.ToLower())
                            );
            // By Category :
            collection = parameters.Categoryid == Guid.Empty ? collection :
                            collection.Where(p=>p.CategoryId == parameters.Categoryid);

            // By Brand :
            collection = parameters.BrandId == Guid.Empty ? collection :
                            collection.Where(p => p.BrandId == parameters.BrandId);

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

        public async Task<IEnumerable<Product>> GetRandomProductsList()
        {
            return await _context.Products.Include(p=>p.Brand).Include(p=>p.Category).Take(4).ToListAsync();
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
