using GeorgianEgg.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeorgianEgg.Controllers
{
    public class BrandsController : Controller
    {
        public IActionResult Index()
        {
            var brands = new List<Brand>();

            brands.Add(new Brand() { Name = "Intel" });
            brands.Add(new Brand() { Name = "AMD" });

            for (var i = 1; i <= 10; i++)
            {
                var model = new Brand();
                model.Name = "Brand " + i.ToString();

                brands.Add(model);
            }

            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult ShopByBrand(String brand)
        {
            ViewData["Brand"] = brand;

            return View();
        }
    }
}
