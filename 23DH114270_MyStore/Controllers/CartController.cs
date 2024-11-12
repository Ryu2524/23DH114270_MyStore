using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23DH114270_MyStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult index()
        {
            return View();
        }
    }
}