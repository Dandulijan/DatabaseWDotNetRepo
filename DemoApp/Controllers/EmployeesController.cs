using DemoApp.Data;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DemoApp.Controllers
{
    public class EmployeesController : Controller
    {
        private AppDBContext db;
        public EmployeesController(AppDBContext _db)
        {
            db = _db;
        }
        static List<Employee> DeletedEmployees = new List<Employee>();
        
        

        public IActionResult Index()
        {
            
           ViewBag.count = db.Employees.Where(x => x.IsDeleted == true).Count();
            return View(db.Employees.Where(x=>x.IsDeleted==false).OrderByDescending(x=>x.HDate));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            emp.isActive = true;
            db.Employees.Add(emp);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id==null)
            {
                return RedirectToAction(nameof(Index));
            }

            var empData = db.Employees.Find(id);
            if (empData != null)
            {
                //return RedirectToAction(nameof(Details),empData);
                return View(empData);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var empData = db.Employees.Find(id);
            if (empData != null)
            {
                //return RedirectToAction(nameof(Details),empData);
                return View(empData);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
                db.Update(emp);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
        }
        //this will work directly 
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{

        //    var emp = db.Employees.Find(id);

        //    if (emp != null && id!=null)
        //    {
        //        db.Employees.Remove(emp);
        //        db.SaveChanges();
        //        return View(emp);
        //    }
        //    else return RedirectToAction(nameof(Index));
        //}
        //------------------------------------------------------
        //this needs post 
        [HttpGet]
        public IActionResult Delete(int? id)
        {

            var emp = db.Employees.Find(id);

            if (emp != null && id != null)
            {
                return View(emp);
            }
            else return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ConfirmDelete(Employee emp)
        {

            var empData = db.Employees.Find(emp.EmployeeId);

            if (empData != null)
            {
                db.Employees.Remove(empData);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult SoftDelete(int? id)
        {

            var emp = db.Employees.Find(id);

            if (emp != null &&  id != null)
            {
                DeletedEmployees.Add(emp);
                emp.IsDeleted = true;
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(Index));
        }

        public IActionResult Active(int? id)
        {
            var emp = db.Employees.Find(id);
            if (emp != null && id != null)
            {
                emp.isActive = true;
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(Index));
        }

        public IActionResult Restore()
        {

            return View(DeletedEmployees);

        }

        [HttpPost]
        public IActionResult RestoreEmp()
        {
            foreach (var item in DeletedEmployees)
            {

                item.IsDeleted = false;
                Console.WriteLine(item);
                db.Employees.Add(item);
       
            }


            // Clear static list after restore
            DeletedEmployees.Clear();

            return RedirectToAction(nameof(Index));
        }


    }
}
