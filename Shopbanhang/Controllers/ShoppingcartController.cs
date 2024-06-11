using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopbanhang.Models;

namespace Shopbanhang.Controllers
{
    public class ShoppingcartController : Controller
    {
        Entities db = new Entities();


        public ActionResult addtocart(int ProductId, int soluong)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<cartitem>();
            }
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            if (giohang.FirstOrDefault(m => m.id == ProductId) == null)
            {
                Product sanpham = db.Products.Find(ProductId);
                cartitem item = new cartitem
                {
                    id = ProductId,
                    name = sanpham.ProductName,
                    price = sanpham.Price.Value,
                    soluong = soluong


                };
                giohang.Add(item);
            }
            else
            {
                cartitem item = giohang.FirstOrDefault(p => p.id == ProductId);
                item.soluong += soluong;
            }
            HttpContext.Session.Add("giohang", giohang);
            /*if (type == "ajax")
            {
                return Json(new
                {
                    soluong = giohang.Sum(c => c.soluong)
                });
            }*/

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            return View(giohang);

        }
        public ActionResult suasoluong(int id, int soluongmoi)
        {
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            cartitem item = giohang.FirstOrDefault(m => m.id == id);
            if (item != null)
            {
                item.soluong = soluongmoi;
            }
            return RedirectToAction("index");



        }
        public ActionResult XoaKhoiGio(int id)
        {
            List<cartitem> giohang = Session["giohang"] as List<cartitem>;
            cartitem itemXoa = giohang.FirstOrDefault(m => m.id == id);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }
    }
}