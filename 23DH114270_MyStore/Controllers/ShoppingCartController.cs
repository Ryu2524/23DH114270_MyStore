using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _23DH114270_MyStore.Models;


namespace _23DH114270_MyStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: ShoppingCart, chuẩn bị dữ liệu cho View 
        public ActionResult ShowCart()
        {
            try
            {
                // Kiểm tra xem giỏ hàng có tồn tại trong session không
                Cart _cart = Session["Cart"] as Cart;
                if (_cart == null)
                {
                    // Nếu không có giỏ hàng, khởi tạo một giỏ hàng mới
                    _cart = new Cart();
                    Session["Cart"] = _cart; // Lưu giỏ hàng vào session
                }
                return View(_cart); // Trả về view giỏ hàng
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View("Error"); // Chuyển hướng đến trang lỗi nếu có
            }
        }
        // Tạo mới giỏ hàng, nguồn được lấy từ Session 
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // Thêm sản phẩm vào giỏ hàng 
        public ActionResult AddToCart(int id)
        {
            // lấy sản phẩm theo id 
            var _pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Cập nhật số lượng và tính lại tổng tiền 
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(Request.Form["idPro"]);
            int _quantity = int.Parse(Request.Form["carQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Xóa dòng sản phẩm trong giỏ hàng 
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
    }
}