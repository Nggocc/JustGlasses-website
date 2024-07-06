using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopweb.Models;

namespace Shopweb.Controllers
{
    public class OrdersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Orders
        public ActionResult MyPurchase(string currentFilter)
        {
            var acc = (Account)Session["account"];
            if (acc == null)
            {
                return RedirectToAction("/Accounts/DangNhap");
            }
            ViewBag.st1 = "1";
            ViewBag.st2 = "2";
            ViewBag.st3 = "3";
            ViewBag.st4 = "4";
            if (string.IsNullOrEmpty(currentFilter))
            {
                currentFilter = ViewBag.st3; 
            }
            var orders = db.Orders.Where(o => o.Shipment.customer_id == acc.customer_id).ToList();

            if (currentFilter == ViewBag.st1)
            {
                currentFilter = "1";
            }
            else if (currentFilter == ViewBag.st2)
            {
                currentFilter = "2";
            }
            else if (currentFilter == ViewBag.st3)
            {
                currentFilter = "3";
            }
            else
            {
                currentFilter = "4";
            }
            switch (currentFilter)
            {
                case "1":
                    orders = orders.Where(o => o.status == "Đơn hàng đang được chuẩn bị").ToList();
                    break;
                case "2":
                    orders = orders.Where(o => o.status == "Đang giao hàng").ToList();
                    break;
                case "3":
                    orders = orders.Where(o => o.status == "Đã giao").ToList();
                    break;
                case "4":
                    orders = orders.Where(o => o.status == "Đã hủy").ToList();
                    break;
                default:
                    break;
            }
            ViewBag.currentFilter = currentFilter;
            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            var acc = (Account)Session["account"];
            var od = db.Orders.FirstOrDefault(o => o.order_id == id && o.Shipment.customer_id == acc.customer_id);
            return View(od);
        }

        // GET: Orders/Create
        public ActionResult Cancel(int? id)
        {
            var acc = (Account)Session["account"];
            var od = db.Orders.FirstOrDefault(o => o.order_id == id && o.Shipment.customer_id == acc.customer_id);
            od.status = "Đã hủy";
            db.SaveChanges();
            return RedirectToAction("MyPurchase");
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
