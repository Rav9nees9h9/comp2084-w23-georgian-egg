using GeorgianEgg.Data;
using GeorgianEgg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeorgianEgg.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList();

            return View(categories);
        }

        public IActionResult Category(int Id)
        {
            var category = _context.Categories.Find(Id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["CategoryName"] = category.Name;

            var products = _context.Products
                .Where(p => p.CategoryId == Id)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();

            return View(products);
        }

        // POST Shop/AddToCart
        [HttpPost]
        public IActionResult AddToCart([FromForm] int ProductId, [FromForm] int Quantity)
        {
            if (Quantity <= 0)
            {
                return BadRequest();
            }

            var product = _context.Products.Find(ProductId);
            if (product == null)
            {
                return BadRequest(); // HTTP Status code 400
            }

            var price = product.Price * Quantity;

            var customerId = GetCustomerId();

            var cartLine = new CartLine() {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                CustomerId = customerId,
            };

            _context.CartLines.Add(cartLine);
            _context.SaveChanges();

            return Redirect("Cart");
        }

        private static String GetCustomerId()
        {
            // TODO
            return "1";
        }
    }
}
