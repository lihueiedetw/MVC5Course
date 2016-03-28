using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class TestController : BaseController
    {
        FabricsEntities db = new FabricsEntities();        

        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EDE()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EDE(EDEViewModel data)
        {
            return View(data);
        }

        public ActionResult CreateProduct()
        {
           
            var product = new Product()
            {
                ProductName = "NewProduct9900",
                Active = true,
                Price = 9900,
                Stock = 19
            };

            db.Products.Add(product);
            db.SaveChanges();
            
            return View(product);
        }

        public ActionResult ReadProduct(bool? Active)
        {
            //var data = db.Products.AsQueryable();
            //data = data.Where(p => p.ProductId > 1550).OrderByDescending(p=>p.Price);

            var data = db.Products.OrderByDescending(p => p.Price).AsQueryable();
            data = data.Where(p => p.ProductId > 1550);

            if (Active.HasValue)
            {
                data = data.Where(p => p.Active == Active);
            }
            
            return View(data);
        }

        public ActionResult OneProduct(int id)
        {
            //var data = db.Products.Find(id);  //一定要是PK
            //var data = db.Products.FirstOrDefault(p => p.ProductId == id);  //只會回傳一筆

            var data = db.Products.Where(p => p.ProductId == id).FirstOrDefault();  //WHERE後可能有多筆，但只要第一筆資料
            

            return View(data);
        }

        public ActionResult UpdateProduct(int id)
        {
            var oneProduct = db.Products.FirstOrDefault(p => p.ProductId == id);
            
            if (oneProduct == null)
            {
                return HttpNotFound();
            }            

            oneProduct.Price = oneProduct.Price * 2;

            try
            {
                db.SaveChanges();
                
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityError in ex.EntityValidationErrors)
                {
                    //return Content(entityError.ValidationErrors.Count.ToString());

                    foreach (var err in entityError.ValidationErrors)
                    {
                        return Content(err.PropertyName + ":" + err.ErrorMessage);
                    }
                }
            }
            
            return RedirectToAction("ReadProduct");
        }

        public ActionResult DeleteProduct(int id)
        {
            var delProduct = db.Products.Find(id);
            db.Products.Remove(delProduct);
            

            //作法一.用迴圈
            //有FK的資料表，所以也要把對應FK的資料刪除掉
            //foreach (var item in delProduct.OrderLines.ToList())
            //{
            //    //有FK的資料表，所以也要把對應FK的資料刪除掉
            //    db.OrderLines.Remove(item);
            //}

            //作法二.用Range
            //有FK的資料表，所以也要把對應FK的資料刪除掉
            //db.OrderLines.RemoveRange(delProduct.OrderLines); //這個做法會去JOIN資料表，再去撈一次資料

            //作法三.直接下SQL命令刪除
            //db.Database.ExecuteSqlCommand(@"DELETE FROM dbo.OrderLine WHERE ProductId=@p0", id);

            db.SaveChanges();
            
            
            return RedirectToAction("ReadProduct");
        }

        public ActionResult ProductView()
        {            
            //如果撈出來的資料沒有對應到ProductViewModel中的，就不會顯示
            var data = db.Database.SqlQuery<ProductViewModel>(
                @"SELECT * FROM dbo.Product WHERE Active=@p0 AND ProductName LIKE @p1", true, "%yellow%");

            return View(data);
        }

        public ActionResult ProductViewSP()
        {
            var data = db.GetProduct(true, "%yellow%");
            return View(data);
        }

        public ActionResult Edit(int id)
        {
            var data = db.Products.Where(p => p.ProductId == id).FirstOrDefault();  //WHERE後可能有多筆，但只要第一筆資料
           
            return View(data);

        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)  //ModelState是Controller的物件
            {
                db.SaveChanges();
            }
            //var data = db.Products.Where(p => p.ProductId == model.ProductId).FirstOrDefault();  //WHERE後可能有多筆，但只要第一筆資料

            return View();

        }
    }
}