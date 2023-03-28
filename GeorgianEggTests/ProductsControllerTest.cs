using GeorgianEgg.Controllers;
using GeorgianEgg.Data;
using GeorgianEgg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgianEggTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ApplicationDbContext _context;
        private ProductsController _controller;

        private List<Product> _products = new List<Product>();
        private Category _category;
        private Brand _brand;

        [TestInitialize]
        public void TestInitialize()
        { 
            // Mock db
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(dbOptions);

            // Mock category
            _category = new Category
            {
                Id = 1000,
                Name = "Mock Category",
            };
            _context.Categories.Add(_category);

            // Mock brand
            _brand = new Brand
            {
                Id = 2000,
                Name = "Mock Brand",
            };
            _context.Brands.Add(_brand);

            // Mock products
            _products.Add(new Product
            {
                Id = 1,
                Name = "Mock Product 1",
                Price = 100,
                Rating = 1,
                
                CategoryId = _category.Id,
                Category = _category,

                BrandId = _brand.Id,
                Brand = _brand,
            });
            _products.Add(new Product
            {
                Id = 2,
                Name = "Mock Product 2",
                Price = 120,
                Rating = 3,
                
                CategoryId = _category.Id,
                Category = _category,

                BrandId = _brand.Id,
                Brand = _brand,
            });

            foreach (var product in _products)
            {
                _context.Products.Add(product);
	        }

            _context.SaveChanges();

            _controller = new ProductsController(_context);
	    }

        [TestMethod]
        public async Task IndexLoadsViewWithProducts()
        {
            var result = (ViewResult) await _controller.Index();

            CollectionAssert.AreEqual(
                _products.OrderBy(p => p.Name).ToList(),
		        (List<Product>) result.Model
		    );
        }
    }
}
