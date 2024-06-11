using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopbanhang.Models;

namespace Shopbanhang.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private Entities db = new Entities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.TransactStatu);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        

        // GET: Orders/Create
        public ActionResult Create()
        {
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            ViewBag.giohang = giohang;
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( string Firstname, string Lastname, string Email,string Country, int Phone, string Address)
        {
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    FirstName = Firstname,
                    LastName = Lastname,
                    Email = Email,
                    Country = Country,
                    Phone = Phone,
                    Address = Address,
                    OrderDate= DateTime.Now,
                    TransactStatusId = 1,
                    Deleted = false,
                    Paid = false,

                };
                foreach(var item in giohang)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.ProductId = item.id;
                    orderDetail.Quantity = item.soluong;
                    orderDetail.Total = item.thanhtien;
                    db.OrderDetails.Add(orderDetail);



                }
                
                db.Orders.Add(order);
                db.SaveChanges();
                HttpContext.Session.Remove("giohang");
                
                
                return RedirectToAction("OrderPlaced");
            }

            
            return View();
        }
        public ActionResult OrderPlaced()
        {
            return View();
        }


    }
}
