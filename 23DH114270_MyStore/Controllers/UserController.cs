using _23DH114270_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23DH114270_MyStore.Controllers
{
    public class UserController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        [HttpGet]
        // GET: User
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]

        public ActionResult DangNhap(string username, string password)
        {
            // Kiểm tra xem có user nào với username và password đã nhập trong bảng User không
            var user = db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Thiết lập session cho User
                Session["Username"] = user.Username;
                Session["UserRole"] = user.UserRole; // Lưu vai trò của user
                return RedirectToAction("TrangChu", "Home"); // Chuyển hướng đến trang dành riêng cho user
            }
            else
            {
                // Nếu không tìm thấy tài khoản nào khớp, quay lại trang đăng nhập với thông báo lỗi
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                return View();
            }
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(string username, string password, string userRole, string customerName, string customerPhone, string customerEmail, string customerAddress)
        {
            try
            {
                // Kiểm tra nếu username đã tồn tại trong bảng User
                var existingUser = db.Users.SingleOrDefault(u => u.Username == username);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.";
                    return View();
                }

                // Tạo mới User
                var user = new User
                {
                    Username = username,
                    Password = password, // Lưu ý: trong thực tế, bạn nên mã hóa mật khẩu
                    UserRole = "Customer"
                };

                // Thêm User mới vào cơ sở dữ liệu
                db.Users.Add(user);
                db.SaveChanges();

                // Tạo mới Customer liên kết với User
                var customer = new Customer
                {
                    CustomerName = customerName,
                    CustomerPhone = customerPhone,
                    CustomerEmail = customerEmail,
                    CustomerAddress = customerAddress,
                    Username = username // Giả sử có quan hệ giữa Customer và User qua Username
                };

                // Thêm Customer mới vào cơ sở dữ liệu
                db.Customers.Add(customer);
                db.SaveChanges();

                // Thiết lập session sau khi đăng ký thành công
                Session["Username"] = user.Username;
                Session["UserRole"] = user.UserRole;
                Session["CustomerID"] = customer.CustomerID;
                Session["CustomerName"] = customer.CustomerName;

                return RedirectToAction("TrangChu", "Home"); // Chuyển hướng đến trang thích hợp
            }
            catch (DbEntityValidationException ex)
            {
                // Lấy thông tin chi tiết của lỗi xác thực
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // In ra lỗi để dễ dàng kiểm tra
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }

                ViewBag.ErrorMessage = "Có lỗi xảy ra khi đăng ký. Vui lòng kiểm tra lại thông tin.";
                return View();
            }
        }

    }
}