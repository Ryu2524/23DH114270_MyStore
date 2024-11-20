using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23DH114270_MyStore.Models;

namespace _23DH114270_MyStore.Controllers
{
    public class CustomerController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Customers
        public ActionResult NguoiDung()
        {
            // Lấy tên người dùng từ session
            var username = Session["Username"] as string;

            if (username != null)
            {
                // Tìm người dùng dựa trên tên người dùng
                var customer = db.Customers.Include(c => c.User).SingleOrDefault(c => c.Username == username);
                return View(customer);
            }

            // Nếu không có người dùng đăng nhập, chuyển hướng đến trang đăng nhập
            return RedirectToAction("DangNhap", "User ");
        }
    }
}