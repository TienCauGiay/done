using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ShopBanMyPham.Models;

namespace ShopBanMyPham.Controllers
{
    public class LoginController : Controller
    {
        ShopBanMyPhamContext _context = null;

        public INotyfService _notyfService { get; }

        public LoginController(INotyfService notyfService)
        {
            _context = new ShopBanMyPhamContext();
            _notyfService= notyfService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Index(string UserName, string Password)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var user = _context.Users.FirstOrDefault(x => x.Username == UserName && x.Password == Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.Username.ToString());
                    _notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorLogin = "Tên đăng nhập hoặc mật khẩu không đúng";
                    ViewBag.UserName = UserName;
                    ViewBag.Password = Password;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string UserName, string Password)
        {
            var res = _context.Users.FirstOrDefault(x => x.Username == UserName);
            if (res == null)
            {
                User user = new User();
                user.Username = UserName;
                user.Password = Password;
                user.TypeUserId = 3;
                _context.Users.Add(user);
                _context.SaveChanges();
                _notyfService.Success("Đăng kí thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ErrorRegister = "Tài khoản đã tồn tại";
            ViewBag.UserName = UserName;
            ViewBag.Password = Password;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forget(string UserName, string Password)
        {
            var res = _context.Users.FirstOrDefault(x => x.Username == UserName);
            if (res == null)
            {
                ViewBag.ErrorUserName = "Không tồn tại tài khoản nào có tên " + UserName;
                return View();
            }
            else
            {
                res.Password = Password;
                _context.SaveChanges();
                _notyfService.Success("Đổi mật khẩu thành công");
                return RedirectToAction("Index");
            }
        }
    }
}
