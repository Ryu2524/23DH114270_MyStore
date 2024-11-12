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
            var products = db.Products.Include(p => p.Category);
            // Tìm kiếm chuỗi truy vấn theo category
            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.ProductName);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.CategoryID).Where(x => x.CategoryID == category);
            }
            //tim san pham theo ten
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = db.Products.OrderByDescending(x => x.CategoryID).Where(s => s.ProductName.Contains(SearchString.Trim()));
            }
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
