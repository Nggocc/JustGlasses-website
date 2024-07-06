using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using Shopweb.Models;
namespace Shopweb.Controllers
{
    public class ProductsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
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
            var words = product.name.Split(' ').ToList();
            var relatedProducts = db.Products.Where(p => p.category_id == product.category_id && p.product_id != id).ToList();
            relatedProducts= relatedProducts.Where(p => p.gender==product.gender||words.Any(w=>p.name.Contains(w))).Take(4).ToList();
            ViewBag.relatedProducts = relatedProducts;
            return View(product);
        }
        public ActionResult WishList(int id)
        {
            Account acc = (Account)Session["account"];

            if (acc == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            var mywish= db.WishLists.FirstOrDefault(m=>m.customer_id==acc.customer_id);
            if (mywish == null)
            {
                var wish = new WishList()
                {
                    wishList_id = acc.customer_id,
                    customer_id = acc.customer_id,
                    product_id = id
                };
                db.WishLists.Add(wish);
            }
            else
            {
                var wish= db.WishLists.FirstOrDefault(w=>w.product_id==id&& w.customer_id == acc.customer_id);
                if (wish == null)
                {
                    var wishpro = new WishList()
                    {
                        customer_id = acc.customer_id,
                        wishList_id = mywish.wishList_id,
                        product_id = id
                    };
                    db.WishLists.Add(wishpro);

                }
                else
                {
                    db.WishLists.Remove(wish);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }
    
        // GET: Products/Create
        public ActionResult getWistList(string sortOrder)
        {
            Account acc = (Account)Session["account"];

            if (acc == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            var wishes = db.WishLists.Where(w=>w.customer_id==acc.customer_id).ToList();
            switch (sortOrder)
            {
                case "date":
                    wishes = wishes.OrderBy(p => p.Product.InventoryDate).ToList(); break;
                case "name":
                    wishes = wishes.OrderBy(p => p.Product.name).ToList(); break;
                case "price":
                    wishes = wishes.OrderBy(p => p.Product.price).ToList(); break;
                default:
                    wishes = wishes.OrderBy(p => p.Product.name).ToList();
                    break;
            }
            ViewBag.sortOrder = sortOrder;
            return View(wishes);
        }
        public PartialViewResult Category()
        {
            return PartialView(db.Categories.ToList());
        }
        [Route("ProductByCategpory/{category_id}")]
        public ActionResult ProductByCategory(int category_id, string sortOrder, string filter, decimal? priceRange, int?page)
        {
            ViewBag.sortOrder = sortOrder;

            var products = db.Products.Where(p => p.category_id == category_id);

            if (!String.IsNullOrEmpty(filter))
            {
                string[] filters = filter.Split('&');
                foreach (var item in filters)
                {
                    products = products.Where(p => item.Contains(p.gender.ToLower())
                                                || item.Contains(p.shape.ToLower())
                                                || item.Contains(p.material.ToLower()));
                }
            }

            // Apply price range filtering
            if (priceRange.HasValue)
            {
                products = products.Where(p => p.price <= priceRange.Value);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "inventory":
                    products = products.OrderBy(p => p.InventoryDate);
                    break;
                case "name":
                    products = products.OrderBy(p => p.name);
                    break;
                case "price":
                    products = products.OrderBy(p => p.price);
                    break;
                default:
                    products = products.OrderBy(p => p.name);
                    break;
            }

            // Pagination logic
            int pageSize = 9; // Number of products per page
            int pageNumber = (page ?? 1); // Default page number (1 if page is null)

            // Create a paged list of products
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            ViewBag.sortOrder = sortOrder;
            ViewBag.category_id = category_id;
            return View(pagedProducts);
        }

        public ActionResult Search(string key, int? page)
        {
            var products = db.Products.ToList();
            if (!string.IsNullOrEmpty(key))
            {
                products = products.Where(p => p.name.ToLower().Contains(key.ToLower()) || p.Category.name.ToLower().Contains(key.ToLower())).ToList();
            }
            // Pagination logic
            int pageSize = 12; // Number of products per page
            int pageNumber = (page ?? 1); // Default page number (1 if page is null)

            // Create a paged list of products
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            ViewBag.key = key;
            return View(pagedProducts);
        }
        public ActionResult AllProducts(string sortOrder, int? page)
        {
            var products = db.Products.Include(p => p.Category).ToList();
            switch (sortOrder)
            {
                case "date":
                    products = products.OrderBy(p => p.InventoryDate).ToList(); break;
                case "name":
                    products = products.OrderBy(p => p.name).ToList(); break;
                case "price":
                    products = products.OrderBy(p => p.price).ToList(); break;
                default:
                    products = products.OrderBy(p => p.name).ToList();
                    break;
            }
            int pageSize = 12; // Number of products per page
            int pageNumber = (page ?? 1); // Default page number (1 if page is null)

            // Create a paged list of products
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            ViewBag.sortOrder = sortOrder;
            return View(pagedProducts);
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
