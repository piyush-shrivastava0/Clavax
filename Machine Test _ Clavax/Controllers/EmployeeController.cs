using Machine_Test___Clavax.DB;
using Machine_Test___Clavax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machine_Test___Clavax.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private Machine_Test___ClavaxDBEntities _dbContext = new Machine_Test___ClavaxDBEntities();

        // GET: State_Master
        public ActionResult StateList()
        {
            return View(_dbContext.State_Master.ToList());
        }

        // GET: State_Master/Create
        public ActionResult CreateState()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateState(State_Master state_Master)
        {
            if (ModelState.IsValid)
            {
                _dbContext.State_Master.Add(state_Master);
                _dbContext.SaveChanges();
                return RedirectToAction("StateList");
            }

            return View(state_Master);
        }

        // GET: State_Master/Edit/5
        public ActionResult EditState(int? id)
        {
          
            State_Master state_Master = _dbContext.State_Master.Find(id);
            
            return View(state_Master);
        }

        [HttpPost]
        public ActionResult EditState(State_Master state_Master)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(state_Master).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("StateList");
            }
            return View(state_Master);
        }

        // State_Master/Delete/id
        public ActionResult DeleteState(int? id)
        {
            State_Master state_Master = _dbContext.State_Master.Find(id);
            _dbContext.State_Master.Remove(state_Master);
            _dbContext.SaveChanges();
            return RedirectToAction("StateList");
        }


        // GET: Hobby_Master
        public ActionResult HobbyList()
        {
            return View(_dbContext.Hobby_Master.ToList());
        }

        // GET: Hobby_Master/Create
        public ActionResult CreateHobby()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateHobby(Hobby_Master hobby_Master)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Hobby_Master.Add(hobby_Master);
                _dbContext.SaveChanges();
                return RedirectToAction("HobbyList");
            }

            return View(hobby_Master);
        }

        // GET: Hobby_Master/Edit/id
        public ActionResult EditHobby(int? id)
        {
           
            Hobby_Master hobby_Master = _dbContext.Hobby_Master.Find(id);
           
            return View(hobby_Master);
        }

        [HttpPost]
       
        public ActionResult EditHobby(Hobby_Master hobby_Master)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(hobby_Master).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("HobbyList");
            }
            return View(hobby_Master);
        }

        // Hobby_Master/Delete/id
        public ActionResult DeleteHobby(int? id)
        {
            Hobby_Master hobby_Master = _dbContext.Hobby_Master.Find(id);
            _dbContext.Hobby_Master.Remove(hobby_Master);
            _dbContext.SaveChanges();
            return RedirectToAction("HobbyList");
        }

        // GET: Employee_Registration
        public ActionResult EmployeeList()
        {
            var employee_Registration = _dbContext.Employee_Registration.Include(e => e.State_Master).Include(c=>c.Employee_Hobby);
            return View(employee_Registration.ToList());
        }

        // GET: Employee_Registration/Create
        public ActionResult CreateRegistration()
        {
            var model = new employeeRegistration_Model();
            var hobbyList = new List<hobbyModel>();
            ViewBag.State_fid = new SelectList(_dbContext.State_Master, "StateID", "StateName");
           var x = _dbContext.Hobby_Master.ToList();
           foreach(var i in x)
            {
                var hobby = new hobbyModel();
                hobby.hobbyID = i.hobbyID;
                hobby.hobbyName = i.hobbyName;
                hobbyList.Add(hobby);
            }
            model.hobbiesc = hobbyList;
            ViewBag.hobby = x;
            return View(model);
        }

        [HttpPost]
        
        public ActionResult CreateRegistration(employeeRegistration_Model employee_Registration)
        {
            if (ModelState.IsValid)
            {
                var Emp_reg = new Employee_Registration();
                Emp_reg.Address = employee_Registration.Address;
                Emp_reg.Employee_DOB = employee_Registration.Employee_DOB;
                Emp_reg.Employee_ID = employee_Registration.Employee_ID;
                Emp_reg.Employee_Name = employee_Registration.Employee_Name;
                Emp_reg.Gender = employee_Registration.Gender;
                Emp_reg.Hobbies = employee_Registration.Hobbies;
                Emp_reg.State_fid = employee_Registration.State_fid;
                
                _dbContext.Employee_Registration.Add(Emp_reg);
                _dbContext.SaveChanges();
                var empId = Emp_reg.Employee_ID;

                var hobbyList = new List<Employee_Hobby>();
                foreach(var eh in employee_Registration.hobbiesc)
                {
                    if (eh.IsChecked)
                    {
                        var emp_hobby = new Employee_Hobby();
                        emp_hobby.emp_fid = empId;
                        emp_hobby.hobby_fid = eh.hobbyID;
                        //hobbyList.Add(emp_hobby);
                        _dbContext.Employee_Hobby.Add(emp_hobby);
                        _dbContext.SaveChanges();
                    }
                   
                }
                
                return RedirectToAction("EmployeeList");
            }

            ViewBag.State_fid = new SelectList(_dbContext.State_Master, "StateID", "StateName", employee_Registration.State_fid);
            return  View(employee_Registration);
        }

        // GET: Employee_Registration/Edit/id
        public ActionResult EditRegistration(int? id)
        {
            var hobbyList = new List<hobbyModel>();
            var model = new employeeRegistration_Model();
            Employee_Registration employee_Registration = _dbContext.Employee_Registration.Find(id);
            
            ViewBag.State_fid = new SelectList(_dbContext.State_Master, "StateID", "StateName", employee_Registration.State_fid);

            model.Address = employee_Registration.Address;
            model.Employee_DOB = (DateTime)employee_Registration.Employee_DOB;
            model.Employee_ID = employee_Registration.Employee_ID;
            model.Employee_Name = employee_Registration.Employee_Name;
            model.Gender = employee_Registration.Gender;
            model.State_fid = (int)employee_Registration.State_fid;

            var x = _dbContext.Hobby_Master.ToList();
            var emphobby= _dbContext.Employee_Hobby.Where(m=>m.emp_fid==id).ToList();
            foreach (var i in x)
            {
                var hobby = new hobbyModel();
                hobby.hobbyID = i.hobbyID;
                hobby.hobbyName = i.hobbyName;
                foreach(var eh in emphobby)
                {
                    if (eh.hobby_fid == i.hobbyID)
                    {
                        hobby.IsChecked = true;
                    }
                }
                hobbyList.Add(hobby);
            }
            model.hobbiesc = hobbyList;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditRegistration(employeeRegistration_Model employee_Registration)
        {
            if (ModelState.IsValid)
            {
                var Emp_reg = new Employee_Registration();
                Emp_reg.Address = employee_Registration.Address;
                Emp_reg.Employee_DOB = employee_Registration.Employee_DOB;
                Emp_reg.Employee_ID = employee_Registration.Employee_ID;
                Emp_reg.Employee_Name = employee_Registration.Employee_Name;
                Emp_reg.Gender = employee_Registration.Gender;
                Emp_reg.Hobbies = employee_Registration.Hobbies;
                Emp_reg.State_fid = employee_Registration.State_fid;

                _dbContext.Entry(Emp_reg).State = EntityState.Modified;
                _dbContext.SaveChanges();

                var delete = _dbContext.Employee_Hobby.Where(m=>m.emp_fid==employee_Registration.Employee_ID).ToList();
                if (delete.Count > 0)
                {
                    _dbContext.Employee_Hobby.RemoveRange(delete);
                    _dbContext.SaveChanges();
                }
               

                var hobbyList = new List<Employee_Hobby>();
                foreach (var eh in employee_Registration.hobbiesc)
                {
                    if (eh.IsChecked)
                    {
                        var emp_hobby = new Employee_Hobby();
                        emp_hobby.emp_fid = employee_Registration.Employee_ID;
                        emp_hobby.hobby_fid = eh.hobbyID;
                        //hobbyList.Add(emp_hobby);
                        _dbContext.Employee_Hobby.Add(emp_hobby);
                        _dbContext.SaveChanges();
                    }

                }
                return RedirectToAction("EmployeeList");
            }
            ViewBag.State_fid = new SelectList(_dbContext.State_Master, "StateID", "StateName", employee_Registration.State_fid);
            return View(employee_Registration);
        }

        // Employee_Registration/Delete/id
        public ActionResult Delete(int? id)
        {
            var employee_Hobby = _dbContext.Employee_Hobby.Where(m=>m.emp_fid==id).ToList();
            if (employee_Hobby.Count > 0)
            {
                _dbContext.Employee_Hobby.RemoveRange(employee_Hobby);
                _dbContext.SaveChanges();
            }
           
            Employee_Registration employee_Registration = _dbContext.Employee_Registration.Find(id);
            _dbContext.Employee_Registration.Remove(employee_Registration);
            _dbContext.SaveChanges();
            return RedirectToAction("EmployeeList");
        }
    }
}