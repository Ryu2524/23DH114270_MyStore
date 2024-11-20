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
    public class OrderDetailsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "PaymentStatus");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int productId)
        {
            // Get the currently logged-in customer (assume a method exists to get the current customer)
            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
            if (Session["Username"] == null)
            {
                return RedirectToAction("DangNhap", "User"); // Redirect to login if the user is not authenticated
            }

            // Check if there's an existing order for this customer that hasn't been paid yet
            var order = db.Orders.FirstOrDefault(o => o.CustomerID == customer.CustomerID && o.PaymentStatus == "Pending");

            if (order == null)
            {
                // Create a new order if not found
                order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0, // Initial total
                    PaymentStatus = "PendinDefaultg",
                    AddressDelivery = customer.CustomerAddress // Assuming the customer has an address
                };

                db.Orders.Add(order);
                db.SaveChanges(); // Save to generate OrderID
            }

            // Find the product by ID
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound("Product not found");
            }

            // Create and add a new OrderDetail object
            var orderDetail = new OrderDetail
            {
                ProductID = product.ProductID,
                Quantity = 1, // Default quantity for now
                UnitPrice = (decimal)product.ProductPrice,
                OrderID = order.OrderID
            };

            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                order.TotalAmount += (decimal)product.ProductPrice * orderDetail.Quantity; // Update total amount
                db.SaveChanges();
                return RedirectToAction("Index", "Cart"); // Redirect to a cart view page
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "PaymentStatus", orderDetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }


        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "PaymentStatus", orderDetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,OrderID,Quantity,UnitPrice")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "PaymentStatus", orderDetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
