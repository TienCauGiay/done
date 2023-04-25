using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopBanMyPham.Models;
using X.PagedList;

namespace ShopBanMyPham.Controllers
{
    public class ProductController : Controller
    {
        ShopBanMyPhamContext _context = null;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public ProductController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = new ShopBanMyPhamContext();
            hostingEnvironment = environment;
        }
        public IActionResult ProductByCategory(int? page, int id)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var products = _context.Products.Where(x => x.CategoryId == id).ToList();
            PagedList<Product> res = new PagedList<Product>(products, pageNumber, pageSize);
            ViewBag.Category = _context.Categories.ToList();
            ViewBag.CategorySearch = id;
            return View(res);
        }

        public IActionResult ProductDetail(int id) 
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId== id);
            ViewBag.ListFavAlso = _context.Products.Where(x => x.CategoryId == product.CategoryId && x.ProductId != product.ProductId).ToList();
            return View(product);
        }

        public IActionResult Search(int? page, string searchString)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var products = _context.Products.Where(x => x.ProductName.Contains(searchString)).ToList();
            PagedList<Product> res = new PagedList<Product>(products, pageNumber, pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.Category = _context.Categories.ToList();
            return View(res);
        }

        public async Task<IActionResult> AddProduct()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var imagePath = Path.Combine(hostingEnvironment.WebRootPath, "assets/product", image.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    product.Image = image.FileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }
    }
}
