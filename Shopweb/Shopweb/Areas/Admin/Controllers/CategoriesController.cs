using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopweb.Models;

namespace Shopweb.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Categories
        public ActionResult Index()
        {
            //xem all danh mục
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            DBContext db = new DBContext();
            //tìm danh mục mới nhất dựa vào mã có mã lớn nhấ
            var lastCate = db.Categories.OrderByDescending(c => c.category_id).FirstOrDefault();
            //tạo mã mới = +1
            int newid = lastCate == null ? 1 : (lastCate.category_id + 1);
            ViewBag.newid = newid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name)
        {
            Category category = new Category();

            try
            {
                if (ModelState.IsValid)
                {
                    DBContext db = new DBContext();
                    var lastCate = db.Categories.OrderByDescending(c => c.category_id).FirstOrDefault();
                    int newid = lastCate == null ? 1 : (lastCate.category_id + 1);
                    if (db.Categories.Where(c => c.name.ToLower() == category.name.ToLower()).FirstOrDefault() != null)
                    {
                        ViewBag.Error = "Đã tồn tại danh mục này!";
                        return View(category);
                    }
                    category.category_id= newid;
                    category.name = name;
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(category);
            }
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)//truyền vào một id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "category_id,name")] Category category)//truyền một category
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                ViewBag.Error = "Lỗi sửa dữ liệu!" + ex.Message;
                return View(category);
            }
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            try
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không xóa được bản ghi này!" + ex.Message;
                return View("Delete", category);
            }
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
