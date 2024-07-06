using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopweb.Models;
using ClosedXML.Excel;
using System.IO;


namespace Shopweb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["Month"] as string))
            {
                Session["Month"] = Request.QueryString["month"];
            }


            if (Session["HoTen"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }

        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string loginName, string password)
        {
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                ViewBag.thongBao = "Vui lòng nhập đầy đủ thông tin !";
                return View();
            }

            var account = db.Accounts.SingleOrDefault(a => a.loginName == loginName);

            if (account == null)
            {
                
                ViewBag.thongBao = "Tài khoản hoặc mật khẩu không chính xác !";
                return View();
            }

            if (account.password.Trim() != password.Trim())
            {
                ViewBag.thongBao = "Tài khoản hoặc mật khẩu không chính xác !";
                return View();
            }
            Session["HoTen"] = loginName;

            return RedirectToAction("Index");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }


        public ActionResult ExportToExcel()
        {
            var month = Session["Month"] as string;

            int monthInt;
            if (!int.TryParse(month, out monthInt))
            {
                return RedirectToAction("Index");
            }

            var query = from od in db.Order_Details
                        join p in db.Products on od.product_id equals p.product_id
                        join o in db.Orders on od.order_id equals o.order_id
                        where o.date.Month == monthInt && o.date.Year == DateTime.Now.Year
                        select new
                        {

                            OrderDate = o.date,
                            Quantity = od.quantity,
                            ProductId = od.product_id,
                            ProductName = p.name,
                            TotalPrice = od.quantity * p.price
                        };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Order Details");

                // Add headers
                worksheet.Cell(1, 1).Value = "Số lượng";
                worksheet.Cell(1, 2).Value = "Mã sản phẩm";
                worksheet.Cell(1, 3).Value = "Tên sản phẩm";
                worksheet.Cell(1, 4).Value = "Ngày đặt";
                worksheet.Cell(1, 5).Value = "Tổng tiền";

                // Add data
                int row = 2;
                foreach (var item in query.ToList())
                {
                    worksheet.Cell(row, 1).Value = item.Quantity;
                    worksheet.Cell(row, 2).Value = item.ProductId;
                    worksheet.Cell(row, 3).Value = item.ProductName;
                    worksheet.Cell(row, 4).Value = item.OrderDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 5).Value = item.TotalPrice;
                    row++;
                }

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "order_details.xlsx");
            }
        }
    }
}