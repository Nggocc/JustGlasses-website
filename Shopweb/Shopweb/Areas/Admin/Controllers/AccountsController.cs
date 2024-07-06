using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopweb.Models;

namespace Shopweb.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Customer);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.Customers, "customer_id", "name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "loginName,password,role,customer_id")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.Customers, "customer_id", "name", account.customer_id);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "loginName,password,role,customer_id")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }catch(Exception ex)
                {
                    ViewBag.Error = "Lỗi dữ liệu: " + ex.Message;
                }
               
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
