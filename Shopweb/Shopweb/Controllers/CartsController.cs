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
    public class CartsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Carts
        public List<Cart_Details> getGioHang()
        {
            Account acc = (Account)Session["account"];
            Cart cart = db.Carts.FirstOrDefault(g => g.cart_id == acc.customer_id);
            if (cart == null)
            {
                cart = new Cart()
                {
                    cart_id = acc.customer_id,
                    customer_id = acc.customer_id,

                };
                return new List<Cart_Details>();
            }

            var li = db.Carts.FirstOrDefault(c => c.cart_id == acc.customer_id).Cart_Details.ToList();
            return li;
        }
        public ActionResult XemGioHang()
        {
            Account acc = (Account)Session["account"];
            if (acc == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            else
            {
                return View(getGioHang());
            }

        }
        public ActionResult AddToCart(int id, int quantity)
        {

            Account acc = (Account)Session["account"];

            if (acc == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            var cart = db.Carts.Where(c => c.customer_id == acc.customer_id).FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart()
                {
                    cart_id = acc.customer_id,
                    customer_id = acc.customer_id,
                };
                db.Carts.Add(cart);
                db.SaveChanges();
            }

            List<Cart_Details> li = db.Cart_Details.Where(m => m.cart_id == cart.cart_id).ToList();
            if (li == null)
            {
                li = new List<Cart_Details>();
                Cart_Details cd = new Cart_Details();
                cd.product_id = id;
                cd.cart_id = cart.cart_id;
                cd.quanlity = quantity;
                db.Cart_Details.Add(cd);
                db.SaveChanges();
                li.Add(cd);
            }
            else
            {
                var detail = li.FirstOrDefault(c => c.product_id == id);
                if (detail != null)
                {
                    detail.quanlity += quantity;
                    db.SaveChanges();
                }
                else
                {
                    // Nếu sách chưa tồn tại, thêm mới sách vào giỏ hàng
                    Cart_Details ct = new Cart_Details()
                    {
                        product_id = id,
                        cart_id = cart.cart_id,
                        quanlity = quantity
                    };
                    db.Cart_Details.Add(ct);
                    db.SaveChanges();
                    li.Add(ct);
                }
            }
            db.SaveChanges();
            return View("XemGioHang", getGioHang());
        }

        public ActionResult XoaSanPham(int id)
        {
            List<Cart_Details> li = getGioHang();
            if (li != null)
            {
                var chiTiet = li.FirstOrDefault(c => c.product_id == id);
                if (chiTiet != null)
                {
                    db.Cart_Details.Remove(chiTiet);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("XemGioHang", getGioHang());
        }
        public ActionResult CapNhatGioHang(FormCollection form)
        {
            Account acc = (Account)Session["account"];

            if (acc == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            var cart = db.Carts.Where(c => c.customer_id == acc.customer_id).FirstOrDefault();

            try
            {
                foreach (var key in form.AllKeys)
                {
                    if (key.StartsWith("soluong_"))
                    {
                        int product_id = Convert.ToInt16(key.Substring(8));
                        int soluong = Convert.ToInt16(form[key]);
                        if (soluong <= 0)
                        {
                            ViewBag.Error = "Số lượng không hợp lệ.";
                            return View("XemGioHang", getGioHang());
                        }

                        var p = db.Products.FirstOrDefault(pr => pr.product_id == product_id);
                        if (soluong > p.stock)
                        {
                            ViewBag.Error = "Số lượng không đủ.";
                            return View("XemGioHang", getGioHang());
                        }

                        var ct = db.Cart_Details.FirstOrDefault(c => c.product_id == product_id && c.cart_id == cart.cart_id);
                        if (ct != null)
                        {
                            ct.quanlity = soluong;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("XemGioHang", getGioHang());
            }

            return RedirectToAction("XemGioHang", getGioHang());
        }
        [HttpGet]
        public ActionResult Purchase(int id, int quantity)
        {
            Account tk = (Account)Session["account"];
            if (tk == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            var shipment = db.Shipments.Where(s => s.customer_id == tk.customer_id).OrderByDescending(sh => sh.shipment_id).FirstOrDefault();
            if (shipment == null)
            {
                var lastShipment = db.Shipments.OrderByDescending(s => s.shipment_id).FirstOrDefault();
                int newShipmentId = lastShipment == null ? 1 : lastShipment.shipment_id + 1;

                shipment = new Shipment
                {
                    shipment_id = newShipmentId,
                    customer_id = tk.customer_id,
                    receiverName = "",
                    phone = "",
                    address = "",
                    city = ""
                };

                db.Shipments.Add(shipment);
                db.SaveChanges();
            }

            ViewBag.shipment = shipment;

            Cart_Details cd = new Cart_Details()
            {
                product_id = id,
                quanlity = quantity,
                cart_id = tk.customer_id

            };
            ViewBag.cd = cd;
            return View(cd);

        }
        [HttpPost]
        public ActionResult Purchase(FormCollection form, int id, int product_id, int quantity)
        {
            Order order = new Order();
            Account tk = (Account)Session["account"];
            if (tk == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            if (string.IsNullOrEmpty(form["address"]) || string.IsNullOrEmpty(form["city"]) || string.IsNullOrEmpty(form["phone"]) || string.IsNullOrEmpty(form["receiverName"]))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View(getGioHang());
            }
            try
            {

                var shipment = db.Shipments.FirstOrDefault(s => s.shipment_id == id && s.customer_id == tk.customer_id);
                if (shipment == null)
                {
                    shipment = new Shipment()
                    {
                        shipment_id = id,
                        address = form["address"],
                        city = form["city"],
                        receiverName = form["receiverName"],
                        phone = form["phone"],
                        customer_id = tk.customer_id,
                    };
                    db.Shipments.Add(shipment);
                    db.SaveChanges();
                }
                else
                {
                    shipment.address = form["address"];
                    shipment.city = form["city"];
                    shipment.receiverName = form["receiverName"];
                    shipment.phone = form["phone"];
                    db.SaveChanges();
                }

                var lastPayment = db.Payments.OrderByDescending(p => p.payment_id).FirstOrDefault();
                int new_pID = lastPayment == null ? 1 : lastPayment.payment_id + 1;

                Payment payment = new Payment()
                {
                    payment_id = new_pID,
                    date = DateTime.Now,
                    payMethod = "Thanh toán khi nhận hàng",
                    customer_id = tk.customer_id
                };
                db.Payments.Add(payment);

                // new order
                var lastOrder = db.Orders.OrderByDescending(p => p.order_id).FirstOrDefault();
                int newOrderId = lastOrder == null ? 1 : lastOrder.order_id + 1;

                order.order_id = newOrderId;
                order.date = DateTime.Now;
                order.status = "Đơn hàng đang được chuẩn bị";
                order.payment_id = payment.payment_id;
                order.shipment_id = shipment.shipment_id;
                db.Orders.Add(order);

                //List<Cart_Details> li = (List<Cart_Details>)ViewBag.li;

                Order_Details ctpm = new Order_Details()
                {
                    product_id = product_id,
                    order_id = order.order_id,
                    quantity = quantity
                };
                db.Order_Details.Add(ctpm);


                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View(getGioHang());
            }

            return View("OrderSuccess");
        }
        [HttpGet]
        public ActionResult Order()
        {
            Account tk = (Account)Session["account"];
            var shipment = db.Shipments.Where(s => s.customer_id == tk.customer_id).OrderByDescending(sh => sh.shipment_id).FirstOrDefault();
            if (shipment == null)
            {
                var lastShipment = db.Shipments.OrderByDescending(s => s.shipment_id).FirstOrDefault();
                int newShipmentId = lastShipment == null ? 1 : lastShipment.shipment_id + 1;

                shipment = new Shipment
                {
                    shipment_id = newShipmentId,
                    customer_id = tk.customer_id,
                    receiverName = "",
                    phone = "",
                    address = "",
                    city = ""
                };

                db.Shipments.Add(shipment);
                db.SaveChanges();
            }

            ViewBag.shipment = shipment;

            var li = getGioHang();
            return View(li);

        }
        [HttpPost]
        public ActionResult Order(FormCollection form, int id)
        {
            Order order = new Order();
            Account tk = (Account)Session["account"];
            if (tk == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            if (string.IsNullOrEmpty(form["address"]) || string.IsNullOrEmpty(form["city"]) || string.IsNullOrEmpty(form["phone"]) || string.IsNullOrEmpty(form["receiverName"]))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View(getGioHang());
            }
            try
            {

                var shipment = db.Shipments.FirstOrDefault(s => s.shipment_id == id && s.customer_id == tk.customer_id);
                if (shipment == null)
                {
                    shipment = new Shipment()
                    {
                        shipment_id = id,
                        address = form["address"],
                        city = form["city"],
                        receiverName = form["receiverName"],
                        phone = form["phone"],
                        customer_id = tk.customer_id,
                    };
                    db.Shipments.Add(shipment);
                    db.SaveChanges();
                }
                else
                {
                    shipment.address = form["address"];
                    shipment.city = form["city"];
                    shipment.receiverName = form["receiverName"];
                    shipment.phone = form["phone"];
                    db.SaveChanges();
                }

                var lastPayment = db.Payments.OrderByDescending(p => p.payment_id).FirstOrDefault();
                int new_pID = lastPayment == null ? 1 : lastPayment.payment_id + 1;

                Payment payment = new Payment()
                {
                    payment_id = new_pID,
                    date = DateTime.Now,
                    payMethod = "Thanh toán khi nhận hàng",
                    customer_id = tk.customer_id
                };
                db.Payments.Add(payment);

                // new order
                var lastOrder = db.Orders.OrderByDescending(p => p.order_id).FirstOrDefault();
                int newOrderId = lastOrder == null ? 1 : lastOrder.order_id + 1;

                order.order_id = newOrderId;
                order.date = DateTime.Now;
                order.status = "Đơn hàng đang được chuẩn bị";
                order.payment_id = payment.payment_id;
                order.shipment_id = shipment.shipment_id;
                db.Orders.Add(order);

                List<Cart_Details> li = getGioHang();
                foreach (var ct in li)
                {
                    Order_Details ctpm = new Order_Details()
                    {
                        product_id = ct.product_id,
                        order_id = order.order_id,
                        quantity = ct.quanlity
                    };
                    db.Order_Details.Add(ctpm);
                    db.Cart_Details.Remove(ct);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View(getGioHang());
            }

            return View("OrderSuccess");
        }
        public ActionResult OrderSuccess()
        {
            return View();
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
