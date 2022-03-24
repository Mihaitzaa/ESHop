using EShop.Data;
using EShop.Models;
using EShop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public ActionResult Index()
        {
            
            return View(_db.Products);
        }

        [HttpPost]
        public ActionResult Index(string searchterm)
        {
            List<Products> products;
            if (String.IsNullOrEmpty(searchterm))
            {
                products = _db.Products.ToList();
            }
            else
            {
                products = _db.Products.Where(x => x.Name.StartsWith(searchterm)).ToList();
            }

            return View(products);
        }

        //public async Task<ActionResult> Index (string productName)
        //{
        //    if(productName != null)
        //    {
        //        var postdata = _db.Products.Where(p => p.Name.Contains(productName)).ToList();
        //        return View(postdata);
        //    }

        //    return View(await _db.Products.ToListAsync());
        //}
        [HttpPost]
        public JsonResult GetProducts(string term)
        {
            List<string> li;
            li = _db.Products.Where(x=> x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            //var products = (from c in _db.Products
            //                 where c.Name.StartsWith(term)
            //                 select(c.Name));
            return Json(li);
        }

        public IActionResult Privacy()
        {
            var products = from b in _db.Products
                           select b;
            HttpContext.Session.Set("products", new List<Products>());
            return View(products);
        }

        //GET product detail action method
        public ActionResult Detail(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //POST product detail action method
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }
        //GET Remove action methdo
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]

        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        //GET product Cart action method

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
    }
}
