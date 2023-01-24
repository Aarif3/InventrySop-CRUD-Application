using InventryShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventryShop.Controllers
{
    public class ProductController : Controller
    {
        ProductContext db = new ProductContext();
        // GET: Product

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(int PageNumber = 1)
        {
            var data =await db.Products.ToListAsync();
            ViewBag.Totalpages =Math.Ceiling(data.Count() /5.0);
            data = data.Skip((PageNumber - 1)*5).Take(5).ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync(Product p)
        {
            if(ModelState.IsValid == true) 
            {
                db.Products.Add(p);
                int a =await db.SaveChangesAsync();
                if (a > 0)
                {
                    TempData["Message"] ="<script>alert('Item Created successfully')</script>";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "<script>alert('Item Created successfully')</script>";
                }
            }


            return View();
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(int id)
        {
            var row = await db.Products.Where(Model => Model.Id == id).FirstOrDefaultAsync();
            return View(row);
        }

        [HttpPut]
        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(Product p)
        {
            db.Entry(p).State = EntityState.Modified;
            int a =await db.SaveChangesAsync();
            if (a > 0)
            {
                TempData["Message"] = "<script>alert('Data Edited Successfully')</script>";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "<script>alert('Data Not Edited')</script>";
                ModelState.Clear();
            }

            return View();
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var detailsvalue =await db.Products.Where(Model =>Model.Id == id).FirstOrDefaultAsync();
            return View(detailsvalue);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync (int id)
        {
            var deletevalue =await db.Products.Where(Model => Model.Id == id).FirstOrDefaultAsync();
            return View(deletevalue);
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(Product p)
        {
            db.Entry(p).State = EntityState.Deleted;
            int a =await db.SaveChangesAsync();

            if (a > 0)
            {
                TempData["Message"] = "<script>alert('Data Deleted Successfully')</script>";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "<script>alert('Data Not deleted')</script>";
            }

            return View();

        }
    }
}