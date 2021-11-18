using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        // public Product GetProductDetails(int id)
        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                                .Where(i => i.Url.ToLower() == url.ToLower())
                                .Include(i => i.ProductCategories)
                                .ThenInclude(c => c.Category)
                                .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();//veritabanına göndermeden önce ben üzerine sorgu yazıyorum demek gibi

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Url == name.ToLower()));
                }

                return products.ToList();
            }
        }

        public List<Product> GetTop5Products()
        {
            throw new System.NotImplementedException();
        }
    }
}