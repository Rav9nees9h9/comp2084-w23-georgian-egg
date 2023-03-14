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

        public IActionResult Cart()
        {
            var customerId = GetCustomerId();

            var cartLines = _context.CartLines
                .Where(cl => cl.CustomerId == customerId)
                .Include(cl => cl.Product)
                .OrderByDescending(cl => cl.Id)
                .ToList();

            ViewData["TotalPrice"] = cartLines.Sum(cl => cl.Price).ToString("C");

            /*
            decimal totalPrice = 0;

            for (int i = 0; i < cartLines.Count(); i++)
            {
                CartLine cartLine = cartLines[i];

                totalPrice += cartLine.Price;
            }

            ViewData["TotalPrice"] = totalPrice.ToString("C");
            */

            return View(cartLines);
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

            var cartLine = _context.CartLines
                .Where(cl => cl.ProductId == ProductId && cl.CustomerId == customerId)
                .FirstOrDefault();

            if (cartLine != null)
            {
                cartLine.Quantity += Quantity;
                cartLine.Price += price;

                _context.CartLines.Update(cartLine);
                _context.SaveChanges();
            }
            else
            {
                cartLine = new CartLine()
                {
                    ProductId = ProductId,
                    Quantity = Quantity,
                    Price = price,
                    CustomerId = customerId,
                };

                _context.CartLines.Add(cartLine);
                _context.SaveChanges();
            }

            return Redirect("Cart");
        }

        // POST Shop/UpdateCart
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int CartLineId, [FromForm] int Quantity)
        {
            if (Quantity <= 0)
            {
                return BadRequest();
            }

            var cartLine = _context.CartLines.Find(CartLineId);

            if (cartLine == null)
            {
                return BadRequest();
            }

            var product = _context.Products.Find(cartLine.ProductId);
            var discount = cartLine.Price - (product.Price * cartLine.Quantity);

            cartLine.Quantity = Quantity;
            cartLine.Price = Math.Max((product.Price * Quantity) - discount, 0);

            _context.CartLines.Update(cartLine);
            _context.SaveChanges();

            return Redirect("Cart");
        }

        // POST Shop/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart([FromForm] int CartLineId)
        {
            var cartLine = _context.CartLines.Find(CartLineId);

            if (cartLine == null)
            {
                return BadRequest();
            }

            _context.CartLines.Remove(cartLine);
            _context.SaveChanges();

            return Redirect("Cart");
        }

        private String GetCustomerId()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");

            if (String.IsNullOrWhiteSpace(customerId))
            {
                // '??' is called 'null-coalescing' operator
                var isLoggedIn = User?.Identity?.IsAuthenticated ?? false;

                if (isLoggedIn)
                {
                    // 1.a If user is logged in, use their email
                    customerId = User.Identity.Name;
                }
                else
                {
                    // 1.b Else generate a unique ID

                    // UUID or GUID
                    // UUID = Universally Unique ID
                    // GUID = Globally Unique Id
                    // C# uses "GUID"

                    customerId = Guid.NewGuid().ToString();
                }

                HttpContext.Session.SetString("CustomerId", customerId);
            }

            return customerId;
        }
    }
}
