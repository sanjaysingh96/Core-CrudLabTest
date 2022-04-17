using CoreLabTest.Models;
using CoreLabTest.MyDatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLabTest.Controllers
{
    
    public class WorkerController : Controller
    {
        [Authorize]
        public IActionResult show()
        {
            foodiesContext db = new foodiesContext();
            List<WorkerModel> wobj = new List<WorkerModel>();

            var res = db.Workers.ToList();
            foreach (var item in res)
            {
                wobj.Add(new WorkerModel
                {
                    Id=item.Id,
                    Name=item.Name,
                    Email=item.Email,
                    Salary=item.Salary,
                    Age=item.Age,
                    Address=item.Address
                });
            }
            return View(wobj);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(WorkerModel wobj)
        {
            foodiesContext db = new foodiesContext();
            Worker tbl = new Worker();
            tbl.Id = wobj.Id;
            tbl.Name = wobj.Name;
            tbl.Email = wobj.Email;
            tbl.Salary = wobj.Salary;
            tbl.Age = wobj.Age;
            tbl.Address = wobj.Address;

            if (wobj.Id == 0)
            {
                db.Workers.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                db.Entry(tbl).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Show","Worker");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            foodiesContext db = new foodiesContext();
            WorkerModel mobj = new WorkerModel();
            var edititem = db.Workers.Where(a => a.Id == id).First();
            mobj.Id = edititem.Id;
            mobj.Name = edititem.Name;
            mobj.Email = edititem.Email;
            mobj.Salary = edititem.Salary;
            mobj.Age = edititem.Age;
            mobj.Address = edititem.Address;


            return View("Add", mobj);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            foodiesContext db = new foodiesContext();
            var deleteitem = db.Workers.Where(a => a.Id == id).First();
            db.Workers.Remove(deleteitem);
            db.SaveChanges();
            return RedirectToAction("Show","Worker");
        }
    }
}
