using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanMyPham.Models;
using System.Diagnostics;
using X.PagedList;

namespace ShopBanMyPham.Controllers
{
    public class HomeController : Controller
    {
        ShopBanMyPhamContext _context = null;

        public HomeController()
        {
            _context = new ShopBanMyPhamContext();
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listProduct = _context.Products.OrderBy(x => x.Price).ToList();
            PagedList<Product> res = new PagedList<Product>(listProduct, pageNumber, pageSize);
            ViewBag.Category = _context.Categories.ToList();
            return View(res);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}