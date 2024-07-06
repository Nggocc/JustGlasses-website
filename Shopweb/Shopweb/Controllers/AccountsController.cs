using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Shopweb.Models;

namespace Shopweb.Controllers
{
    public class AccountsController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult DangXuat()
        {
            Session.Clear();
            Session["account"] = null;

            return RedirectToAction("DangNhap");
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string loginName, string password)
        {
            ViewBag.loginName = loginName;
            ViewBag.pas = password;

            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                ViewBag.thongBao = "Vui lòng nhập đầy đủ thông tin !";
                return View();
            }

            var account = db.Accounts.SingleOrDefault(a => a.loginName == loginName);

            if (account == null)
            {
                ViewBag.kq2 = null;
                ViewBag.thongBao = "Tài khoản hoặc mật khẩu không chính xác !";
                return View();
            }

            if (account.password.Trim() != password.Trim())
            {
                ViewBag.kq2 = "cay nha";
                ViewBag.thongBao = "Tài khoản hoặc mật khẩu không chính xác !";
                return View();
            }
            Session["account"] = account;
            return Redirect("/Products/Index");

        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(string loginName, string password, string passwordconfirm, string email)
        {
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordconfirm) || string.IsNullOrEmpty(email))
            {
                ViewBag.thongBao = "Vui lòng nhập đầy đủ thông tin !";
                return View();
            }
            if (db.Accounts.Any(c => c.loginName == loginName))
            {
                ViewBag.thongBao = "Tên đăng nhập đã tồn tại !";
                return View();
            }
            

            if (db.Customers.Any(c => c.email == email))
            {
                ViewBag.thongBao = "Email đang được sử dụng !";
                return View();
            }
            if (password.Length < 8)
            {
                ViewBag.thongBao = "Mật khẩu phải có ít nhất 8 ký tự !";
                return View();
            }
            if (password != passwordconfirm)
            {
                ViewBag.thongBao = "Vui lòng xác nhận lại mật khẩu !";
                return View();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int id = db.Customers.OrderByDescending(c => c.customer_id).FirstOrDefault().customer_id + 1;
                    var customer = new Customer { customer_id = id, email = email };

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    var account = new Account
                    {
                        loginName = loginName,
                        password = password,
                        role = "user",
                        customer_id = customer.customer_id
                    };

                    db.Accounts.Add(account);
                    db.SaveChanges();

                    transaction.Commit();
                    ViewBag.thongBao = "Đăng ký thành công!";
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    ViewBag.thongBao = "Có lỗi: " + ex.Message;
                }
            }


            return RedirectToAction("DangNhap");
        }
        public ActionResult TaiKhoanCuaToi()
        {
            Account account = (Account)Session["account"];
            Customer customer = db.Customers.FirstOrDefault(c => c.customer_id == account.customer_id);
            return View(customer);
        }
        [HttpPost]
        public ActionResult TaiKhoanCuaToi(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        //lấy tên file
                        string fileName = Path.GetFileName(f.FileName);
                        //lấy đường dẫn để lưu file
                        string uploadPath = Server.MapPath("~/Content/Images/Customer" + fileName);
                        //lưu file
                        f.SaveAs(uploadPath);
                        //gán giá trị trong db
                        customer.image = fileName;
                    }
                    else
                    {
                        customer.image = Request.Form["image"];
                    }

                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    View(customer);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi: " + ex.Message;

            }
            return View(customer);
        }
        public ActionResult allCusstomer()
        {
            return View(db.Customers.ToList());
        }
        [HttpGet]
        public ActionResult SetPass()
        {
            Account tk = (Account)Session["account"];
            return View(tk);
        }
        public ActionResult SetPass(string oldpass, string confirmPw, Account acc)
        {

            if (String.IsNullOrEmpty(acc.password)||string.IsNullOrEmpty(oldpass))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }
            if (oldpass.Trim() != db.Accounts.FirstOrDefault(a => a.loginName == acc.loginName).password.Trim())
            {
                ViewBag.Error = "Mật khẩu cũ không đúng!";
                return View();
            }
            if (acc.password.Contains(' '))
            {

                ViewBag.Error = "Mật khẩu không được chứa dấu cách!";
                return View();

            }

            if (acc.password.Length < 8)
            {

                ViewBag.Error = "Mật khẩu phải có độ dài tối thiểu 8 ký tự!";
                return View();

            }
            if (acc.password.Trim() != confirmPw.Trim())
            {
                ViewBag.Error = "Bạn cần xác nhận lại mật khẩu!";
                return View();
            }
            db.Accounts.Where(a => a.loginName == acc.loginName).FirstOrDefault().password = acc.password.Trim();
            db.SaveChanges();
            return RedirectToAction("DangNhap");


            //ViewBag.Error = "Dữ liệu không hợp lệ!";
            //return View();
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

