using InventryShop.Models;
using System;
//using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Ast;

namespace InventryShop.Controllers
{
    public class CategoryController : Controller
    {
    
        ProductContext db = new ProductContext();
        // GET: Category
        public async Task<ActionResult> Index(int PageNumber =1)
        {
           var data =await db.Categories.ToArrayAsync();
            ViewBag.Totalpages = Math.Ceiling(data.Count() / 5.0);
            var data2 = data.Skip((PageNumber - 1)*5).Take(5).ToList();
            return View(data2);
        }

        public  ActionResult Create() 
        {
            return View();
        }

        [ActionName("Create")]
        [HttpPost]
        public async Task<ActionResult> CreateAsync(Category c)
        {
            if (ModelState.IsValid == true)
            {
                db.Categories.Add(c);
                int a =await db.SaveChangesAsync();
                if (a > 0)
                {
                    TempData["Message"] = "<script>alert('Item Created successfully')</script>";
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
            var idvalue =await db.Categories.Where(Model => Model.Id == id).FirstOrDefaultAsync();
            return View(idvalue);
        }


        [HttpPut]
        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync (Category c)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c).State = EntityState.Modified;
                int a =await  db.SaveChangesAsync();
                if (a > 0)
                {
                    TempData["Message"] = "<script>alert('Edited Successfully')</script>";
                    return RedirectToAction("Index");
                }

                {
                    TempData["Message"] = "<script>alert('Data Not Edited')</script>";
                    ModelState.Clear();
                }

            }

            return View();
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var detailsValue = await db.Categories.Where(Model => Model.Id ==id).FirstOrDefaultAsync();
            return View(detailsValue);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var deleteValue =await db.Categories.Where(Model => Model.Id == id).FirstOrDefaultAsync();
            return View(deleteValue);
        }

        [HttpDelete]
        [ActionName("Delete")]

        public async Task<ActionResult> DeleteAsync(Category c)
        {

            db.Entry(c).State = EntityState.Deleted;
            int a =await db.SaveChangesAsync();
            if (a > 0)
            {
                TempData["Message"] = "<script>alert('Deleted Successfully')</script>";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "<script>alert('Data Not Deleted ')</script>";
                ModelState.Clear();
            }
            return View();

        }


        [ActionName("AddProduct")]
        public async Task<ActionResult> AddProductAsync(int id)
        {
            Session["CatagoryId"] = id;
            var data = await db.Products.ToListAsync();
            return View(data);
        }

        [ActionName("AddProductdata")]
        public async Task<ActionResult> AddProductDataAsync(int Pid, int Cid)
        
        {
            CategoryList list = new CategoryList();

            list.ProductId = Pid;
            list.CategoryId = Cid;
            list.IsActive = true;
            //var sqlparameter = new[]
            //{
            //    new SqlParameter("@ProductId",Pid),
            //    new SqlParameter("@CatagoryId",Cid)

            //};

            //var data = db.CategoriesList.SqlQuery($"execute spAddProToCat @ProductId = @ProductId, @CatagoryId = @CatagoryId",sqlparameter);

            var data = db.CategoriesList.Add(list);
            int a =await db.SaveChangesAsync();
            if (a > 0)
            {
                TempData["Message"] = "<script>alert('Product Added Successfully')</script>";
                return RedirectToAction("ProductList", new { id = list.CategoryId });
            }
            else
            {
                TempData["Message"] = "<script>alert('Product Not Added')</script>";
            }

            return RedirectToAction("ProductList","Category", new {id = list.CategoryId });

        }

        [ActionName("ProductList")]
        public async Task<ActionResult> ProductListAsync(int id)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter ("@CatagoryId",id)
            };
            var data = await db.Database.SqlQuery<Product>("spShowCatProduct @CatagoryId = @CatagoryId",param).ToListAsync();



            return View(data); 
        }


        public ActionResult DeActive(int id)
        {
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter ("@CategoryId",id)
            };
            var data = db.Database.SqlQuery<CategoryList>("spDeActive @CategoryId = @CategoryId", para).ToList();
            return RedirectToAction("Index");

        }
    }
}