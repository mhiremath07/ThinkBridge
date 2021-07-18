using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MachineTask2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                MachineTask2Entities entities = new MachineTask2Entities();
                List<ProductMaster> ProductMasters = entities.ProductMasters.ToList();

                //Add a Dummy Row.
                ProductMasters.Insert(0, new ProductMaster());
                return View(ProductMasters.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        public ActionResult InsertProductMaster(ProductMaster ProductMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MachineTask2Entities entities = new MachineTask2Entities())
                    {
                        entities.ProductMasters.Add(ProductMaster);
                        entities.SaveChanges();
                    }
                    return Json(ProductMaster);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .ToList();
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                }
            }
            catch (Exception ex)
            {
                string Message = ex.GetBaseException().ToString();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Message);
            }

        }

        [HttpPost]
        public ActionResult UpdateProductMaster(ProductMaster productMasterFromUi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MachineTask2Entities entities = new MachineTask2Entities())
                    {
                        ProductMaster ProductMaster = (from c in entities.ProductMasters
                                                       where c.Id == productMasterFromUi.Id
                                                       select c).FirstOrDefault();
                        ProductMaster.Name = productMasterFromUi.Name;
                        ProductMaster.Price = productMasterFromUi.Price;
                        ProductMaster.ProdCode = productMasterFromUi.ProdCode;
                        ProductMaster.Description = productMasterFromUi.Description;
                        entities.SaveChanges();
                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                                          .Where(y => y.Count > 0)
                                          .ToList();
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                }

                return new EmptyResult();
            }
            catch(Exception ex)
            {
                string Message = ex.GetBaseException().ToString();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Message);
            }
            
        }

        [HttpPost]
        public ActionResult DeleteProductMaster(int productMasterId)
        {
            try
            {
                using (MachineTask2Entities entities = new MachineTask2Entities())
                {
                    ProductMaster ProductMaster = (from c in entities.ProductMasters
                                                   where c.Id == productMasterId
                                                   select c).FirstOrDefault();
                    entities.ProductMasters.Remove(ProductMaster);
                    entities.SaveChanges();
                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                string Message = ex.GetBaseException().ToString();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Message);
            }
        }

    }
}