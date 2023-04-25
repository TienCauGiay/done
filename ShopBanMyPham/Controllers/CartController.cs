using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopBanMyPham.Models;
using System.Collections.Generic;

namespace ShopBanMyPham.Controllers
{
    public class CartController : Controller
    {
        ShopBanMyPhamContext _context = null;

        public INotyfService _notyfService { get; }

        private const string GIOHANG = "GIOHANG";

        public CartController(INotyfService notyfService)
        {
            _context = new ShopBanMyPhamContext();
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = cart;
                }
                var total = 0;
                foreach (var item in list)
                {
                    total += item.SellQuantity * item.Product.Price.GetValueOrDefault(0);
                }
                ViewBag.Total = total;
                var totalShip = total;
                if(list.Count> 0)
                {
                    var cartItem = list.FirstOrDefault();
                    totalShip += cartItem.ShippingCost;
                }
                ViewBag.TotalShip = totalShip;
                return View(list);
            }
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
                if (cart != null)
                {
                    var list = (List<CartItem>)cart;
                    if (list.Exists(x => x.Product.ProductId == id))
                    {
                        foreach (var item in list)
                        {
                            if (item.Product.ProductId == id)
                            {
                                item.SellQuantity += quantity;
                            }
                        }
                    }
                    else
                    {
                        // tạo mới 1 đối tượng cart item
                        var item = new CartItem();
                        var product = _context.Products.SingleOrDefault(x => x.ProductId == id);
                        item.Product = product;
                        item.SellQuantity = quantity;
                        list.Add(item);
                    }
                    var cartJson = JsonConvert.SerializeObject(list);
                    HttpContext.Session.SetString(GIOHANG, cartJson);
                }
                else
                {
                    // tạo mới 1 đối tượng cart item
                    var item = new CartItem();
                    var product = _context.Products.SingleOrDefault(x => x.ProductId == id);
                    item.Product = product;
                    item.SellQuantity = quantity;
                    var list = new List<CartItem>();
                    list.Add(item);
                    // Gán vào session
                    var cartJson = JsonConvert.SerializeObject(list);
                    HttpContext.Session.SetString(GIOHANG, cartJson);
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult MinusProduct(int id)
        {
            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
            if (cart != null)
            {
                var itemMinus = cart.SingleOrDefault(item => item.Product.ProductId == id);
                if (itemMinus != null)
                {
                    itemMinus.SellQuantity -= 1;
                    if (itemMinus.SellQuantity <= 0) itemMinus.SellQuantity = 1;

                    var cartJson = JsonConvert.SerializeObject(cart);
                    HttpContext.Session.SetString(GIOHANG, cartJson);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult PlusProduct(int id)
        {
            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
            if (cart != null)
            {
                var itemMinus = cart.SingleOrDefault(item => item.Product.ProductId == id);
                if (itemMinus != null)
                {
                    itemMinus.SellQuantity += 1;
                    if (itemMinus.SellQuantity <= 0) itemMinus.SellQuantity = 1;

                    var cartJson = JsonConvert.SerializeObject(cart);
                    HttpContext.Session.SetString(GIOHANG, cartJson);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);

            if (cart != null)
            {
                var itemToRemove = cart.SingleOrDefault(item => item.Product.ProductId == id);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    var cartJson = JsonConvert.SerializeObject(cart);
                    HttpContext.Session.SetString(GIOHANG, cartJson);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult FastShipping()
        {
            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
            foreach(var item in cart)
            {
                item.ShippingCost = 50000;
            }
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(GIOHANG, cartJson);
            return RedirectToAction("Index");
        }

        public IActionResult FreeShipping()
        {
            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
            foreach (var item in cart)
            {
                item.ShippingCost = 0;
            }
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(GIOHANG, cartJson);
            return RedirectToAction("Index");
        }

        public IActionResult Payment()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = cart;
                }
                var total = 0;
                foreach (var item in list)
                {
                    total += item.SellQuantity * item.Product.Price.GetValueOrDefault(0);
                }
                ViewBag.Total = total;
                var totalShip = total;
                if (list.Count > 0)
                {
                    var cartItem = list.FirstOrDefault();
                    totalShip += cartItem.ShippingCost;
                }
                ViewBag.TotalShip = totalShip;
                string userName = HttpContext.Session.GetString("UserName");
                var user = _context.Users.FirstOrDefault(x => x.Username == userName);
                var kh = _context.Customers.FirstOrDefault(x => x.UserId == user.UserId);
                ViewBag.KhachHang = kh;
                return View(list);
            }
        }

        public IActionResult ConfirmPayment()
        {
            string userName = HttpContext.Session.GetString("UserName");
            var user = _context.Users.FirstOrDefault(x => x.Username == userName);
            var customer = _context.Customers.FirstOrDefault(x => x.UserId == user.UserId);
            if(customer == null)
            {
                _notyfService.Success("Bạn cần cập nhật thông tin khách hàng");
                return RedirectToAction("Payment");
            }

            var giohang = HttpContext.Session.GetString(GIOHANG) ?? string.Empty;
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(giohang);
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart;
            }
            if(list.Count < 1)
            {
                _notyfService.Success("Chưa có sản phẩm nào để thanh toán");
                return RedirectToAction("Index", "Home");
            }
            var total = 0;
            foreach (var item in list)
            {
                total += item.SellQuantity * item.Product.Price.GetValueOrDefault(0);
            }
            if (list.Count > 0)
            {
                var cartItem = list.FirstOrDefault();
                total += cartItem.ShippingCost;
            }
            
            var order = new Order();
            order.CustomerId = customer.CustomerId;
            order.EmployeeId = 1;
            order.OrderStatus = false;
            order.TotalMoney = total;
            _context.Orders.Add(order);
            _context.SaveChanges();
            try
            {
                foreach (var item in list)
                {
                    var sp = _context.Products.FirstOrDefault(x => x.ProductId == item.Product.ProductId);
                    if (sp.Quantity >= item.SellQuantity)
                    {
                        OrderDetail ct = new OrderDetail();
                        ct.OrderId = order.OrderId;
                        ct.ProductId = item.Product.ProductId;
                        ct.SellNumber = item.SellQuantity;
                        ct.ShippingCost = item.ShippingCost / list.Count;
                        _context.OrderDetails.Add(ct);
                        sp.Quantity -= item.SellQuantity;
                        _context.SaveChanges();
                    }
                    else
                    {
                        _notyfService.Success("Số sản phẩm hiện tại tại cửa hàng không đủ");
                        return RedirectToAction("Index");
                    }
                }
                HttpContext.Session.Remove(GIOHANG);
            }
            catch (Exception ex)
            {
                
            }
            _notyfService.Success("Bạn đã đặt hàng thành công");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ConfirmOrder()
        {
            var listOrder = _context.Orders.Where(x => x.OrderStatus== false).ToList();
            return View(listOrder);
        }

        public IActionResult ConfirmOrders(int id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            string userName = HttpContext.Session.GetString("UserName");
            var user = _context.Users.FirstOrDefault(x => x.Username == userName);
            var employee = _context.Employees.FirstOrDefault(x => x.UserId == user.UserId);
            order.EmployeeId = employee.EmployeeId;
            order.OrderStatus = true;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("ConfirmOrder");
        }
    }
}
