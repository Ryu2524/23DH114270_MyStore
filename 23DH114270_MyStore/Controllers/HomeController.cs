using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23DH114270_MyStore.Models;

namespace _23DH114270_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities(); // Khởi tạo đối tượng truy cập cơ sở dữ liệu

        public ActionResult TrangChu()
        {
            // Lấy 5 sản phẩm đầu tiên từ cơ sở dữ liệu
            var products = db.Products.Take(5).ToList();
            // Lấy tất cả danh mục từ cơ sở dữ liệu
            var categories = db.Categories.ToList();

            // Tạo một ViewModel để truyền cả sản phẩm và danh mục đến view
            var viewModel = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel); // Truyền ViewModel đến view
        }
    }
}