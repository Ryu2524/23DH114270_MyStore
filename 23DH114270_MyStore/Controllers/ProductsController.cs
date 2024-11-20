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
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Products
        public ActionResult SanPham(int ? category , string SearchString)
        {
            var products = db.Products.Include(p => p.Category).AsQueryable();

            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryID == category.Value);
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(SearchString.Trim()));
                ViewBag.CurrentSearchString = SearchString; // Lưu giá trị tìm kiếm hiện tại
            }
            else
            {
                ViewBag.CurrentSearchString = string.Empty; // Đặt giá trị mặc định
            }

            products = products.OrderByDescending(x => x.ProductName);

            return View(products.ToList());
        }
        public ActionResult ChiTietSanPham(int? id)
        {
              if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }  
    }
}
