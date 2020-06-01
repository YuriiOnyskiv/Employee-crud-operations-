using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.NETMVCCRUD.Models;
using System.Data.Entity;
using System.Data.SqlClient;
namespace Asp.NETMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (MyDBModel db = new MyDBModel())
            {
                List<Employee> empList = db.Employee.ToList();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
            {
                using (MyDBModel db = new MyDBModel())
                {
                    return View(db.Employee.Where(x => x.EmployeeID==id).FirstOrDefault());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
            using (MyDBModel db = new MyDBModel())
            {
                if (emp.EmployeeID == 0)
                {
                    db.Employee.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (MyDBModel db = new MyDBModel())
            {
                Employee emp = db.Employee.Where(x => x.EmployeeID == id).FirstOrDefault();
                db.Employee.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin adm)
        {
                using (MyDBModel db = new MyDBModel())
                {
                try
                {
                    var check = db.Admin.Single(x => x.AdminName == adm.AdminName && x.AdminPassword == adm.AdminPassword);
                    if (check != null)
                    {
                        Session["AdminId"] = adm.AdminId.ToString();
                        Session["AdminName"] = adm.AdminName.ToString();
                        return RedirectToAction("Index");
                    }
                }catch(Exception)
                    {

                    TempData["Message"] = "Login failed - wrong Name or Password";
           
                    }
                }

            return View(adm);
        }
    }
}
                   
                    
                   
                    
            