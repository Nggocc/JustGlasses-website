using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Shopweb.Models;

namespace Shopweb.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Products
        public ActionResult Index(string id, string sortOder, string searchString, string currentFilter, int? page)
        {
            List<Product> sanPhams = new List<Product>();

            ViewBag.CurrentSort = sortOder;//lấy yêu cầu sắp

            if (id == null)
            {
                sanPhams = db.Products.Select(s => s).ToList();
            }
            else
            {
                sanPhams = db.Products.Where(s => s.Category.name.Contains(id)).Select(s => s).ToList();
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;



            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = db.Products.Where(s => s.name.Contains(searchString)).Select(s => s).ToList();
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(sanPhams.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {

            ViewBag.category_id = new SelectList(db.Categories, "category_id", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,name,SKU,description,price,stock,material,gender,shape,color,image,eye_width,eye_lenth,InventoryDate,entryPrice,category_id")] Product product)
        {
            try
            {
                if (product.product_id == 0)
                {
                    ViewBag.Error = "id null";
                    ViewBag.category_id = new SelectList(db.Categories, "category_id", "name", product.category_id);
                    return View(product);

                }
                if (ModelState.IsValid)
                {
                    // Lưu ảnh chính
                    product.image = "";
                    var mainImageFile = Request.Files["MainImageFile"];
                    if (mainImageFile != null && mainImageFile.ContentLength > 0)
                    {
                        string mainFileName = Path.GetFileName(mainImageFile.FileName);
                        string mainUploadFile = Server.MapPath("~/Content/Images/") + mainFileName;
                        mainImageFile.SaveAs(mainUploadFile);
                        product.image = mainFileName;
                    }

                    // Lưu ảnh gallery
                    var galleryFiles = Request.Files.GetMultiple("GalleryFiles");
                    List<string> galleryFileNames = new List<string>();
                    foreach (var file in galleryFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string galleryFileName = Path.GetFileName(file.FileName);
                            string galleryUploadFile = Server.MapPath("~/Content/Images/") + galleryFileName;
                            file.SaveAs(galleryUploadFile);
                            galleryFileNames.Add(galleryFileName);
                        }
                    }
                    product.gallery = string.Join(",", galleryFileNames);

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message + " " + ex.InnerException?.Message;
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "name", product.category_id);
                return View(product);
            }
         
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "name", product.category_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,name,SKU,description,price,stock,material,gender,shape,color,image,eye_width,eye_lenth,InventoryDate,entryPrice,category_id")] Product product)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    product.image = "";
                    var mainImageFile = Request.Files["MainImageFile"];
                    if (mainImageFile != null && mainImageFile.ContentLength > 0)
                    {
                        string mainFileName = Path.GetFileName(mainImageFile.FileName);
                        string mainUploadFile = Server.MapPath("~/Content/Images/") + mainFileName;
                        mainImageFile.SaveAs(mainUploadFile);
                        product.image = mainFileName;
                    }

                    // Lưu ảnh gallery
                    var galleryFiles = Request.Files.GetMultiple("GalleryFiles");
                    List<string> galleryFileNames = new List<string>();
                    foreach (var file in galleryFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string galleryFileName = Path.GetFileName(file.FileName);
                            string galleryUploadFile = Server.MapPath("~/Content/Images/") + galleryFileName;
                            file.SaveAs(galleryUploadFile);
                            galleryFileNames.Add(galleryFileName);
                        }
                    }
                    product.gallery = string.Join(",", galleryFileNames);
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.Error = "Lỗi sửa dữ liệu!" + ex.Message;
                ViewBag.category_id = new SelectList(db.Categories, "category_id", "name", product.category_id);
                return View(product);
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        //top 3 bán chạy
        public ActionResult TopBanChay()
        {
            var topSelling = db.Order_Details
                           .GroupBy(od => od.product_id) // Nhóm theo product_id
                           .Select(group => new
                           {
                               ProductId = group.Key, // Lấy product_id
                               TotalQuantity = group.Sum(od => od.quantity)
                           })
                           .OrderByDescending(g => g.TotalQuantity)
                           .Take(3)
                           .Join(db.Products,
                                 g => g.ProductId, // join  OrderDetails
                                 p => p.product_id, //join  Products
                                 (g, p) => new TopBanChay
                                 {
                                     ProductId = p.product_id,
                                     ProductName = p.name,
                                     Price = p.price,
                                     TotalQuantity = g.TotalQuantity // Tổng số lượng bán
                                 })
                           .ToList();

            return View(topSelling);
        }

        //5 Sản phẩm không bán được
        public ActionResult SanPhamKhongBanDuoc()
        {

            var unsoldProducts = db.Products
                                   .Where(p => !db.Order_Details.Any(od => od.product_id == p.product_id))
                                   .Select(p => new TopBanChay
                                   {
                                       ProductId = p.product_id,
                                       ProductName = p.name,
                                       Price = p.price,
                                       TotalQuantity = 0
                                   })
                                   .Take(5)
                                   .ToList();

            return View(unsoldProducts);
        }
    }
}
