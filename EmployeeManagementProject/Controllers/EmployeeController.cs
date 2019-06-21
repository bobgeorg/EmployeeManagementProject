using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagementProject.Models;
using PagedList;
using EmployeeManagementProject.ViewModels;
using System.Data.Entity.Infrastructure;


namespace EmployeeManagementProject.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index(int? id, string sortOrder,  string searchString)
        {
            //code for sortingViewModel.Employees view either by surname or hiring date
            
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                       
            var ViewModel = new EmployeeIndexData();

            ViewModel.Employees = from s in db.Employees.Include(i => i.Skills)
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Applying searching functionality
               ViewModel.Employees =ViewModel.Employees.Where(s => s.Employee_Surname.Contains(searchString)
                                       || s.Employee_Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                   ViewModel.Employees =ViewModel.Employees.OrderByDescending(s => s.Employee_Surname);
                    break;
                case "Date":
                   ViewModel.Employees =ViewModel.Employees.OrderBy(s => s.Hiring_Date);
                    break;
                case "date_desc":
                   ViewModel.Employees =ViewModel.Employees.OrderByDescending(s => s.Hiring_Date);
                    break;
                default:
                   ViewModel.Employees =ViewModel.Employees.OrderBy(s => s.Employee_Surname);
                    break;
            }

            if (id != null)
            {
                if (!(ViewModel.Skills == null))
                {
                    ViewModel.Skills = null;
                }
                else
                {
                    ViewBag.EmployeeId = id.Value;
                    ViewModel.Skills = ViewModel.Employees.Where(
                        i => i.Employee_id == id.Value).Single().Skills;
                }
            }

            return View(ViewModel);
            
        }

      


        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            var employee = new Employee();
            employee.Skills = new List<Skill>();
            PopulateAssignedSkillData(employee);
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[MultipleSubmitionHelper (Name = "action",Argument = "Create")]
        public ActionResult Create([Bind(Include = "Employee_id,Employee_Name,Employee_Surname,Employee_Details,Hiring_Date")] Employee employee,string[] selectedSkills)
        {
            
            
                if (selectedSkills != null)
                {
                    employee.Skills = new List<Skill>();
                    foreach (var skill in selectedSkills)
                    {
                        var skillToAdd = db.Skills.Find(int.Parse(skill));
                        employee.Skills.Add(skillToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                PopulateAssignedSkillData(employee);
                return View(employee);
            }

        [HttpPost,ActionName("AddSkill")]
        [ValidateAntiForgeryToken]
        //[MultipleSubmitionHelper(Name = "action", Argument = "AddSkill")]
        public ActionResult AddSkill([Bind(Include = "Skill_Name,Skill_Description")]Skill skill)
        {
            var employee = new Employee();
            employee.Skills = new List<Skill>();

            if (ModelState.IsValid)
            {
                skill.Skill_Creation_Date = DateTime.Today;
                db.Skills.Add(skill);
                db.SaveChanges();
                PopulateAssignedSkillData(employee);
                return View("Create",employee);
            }
            PopulateAssignedSkillData(employee);
            return View("Create",employee);
        }

        public ActionResult UpdateCheckBoxList()
        {
            var employee = new Employee();
            employee.Skills = new List<Skill>();
            PopulateAssignedSkillData(employee);
            return PartialView("_UpdateCheckboxList", employee);
        }

        public ActionResult AddNonExistingSkill()
        {
            return PartialView("_AddNonExistingSkill");
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Include(i => i.Skills)
                               .Where(i => i.Employee_id == id)
                               .Single();
            PopulateAssignedSkillData(employee);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        private void PopulateAssignedSkillData(Employee employee)
        {
            var allSkills = db.Skills;
            var employeeSkills = new HashSet<int>(employee.Skills.Select(c => c.Skill_id));
            var viewModel = new List<AssignedSkillData>();
            foreach (var skill in allSkills)
            {
                viewModel.Add(new AssignedSkillData
                {
                    SkillID = skill.Skill_id,
                    SkillName= skill.Skill_Name,
                    Assigned = employeeSkills.Contains(skill.Skill_id)
                });
            }
            ViewBag.Skills = viewModel;
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,string[] selectedSkills)
        {
           
           var employeeToUpdate = db.Employees
                .Include(i => i.Skills)
                .Where(i => i.Employee_id == id)
                .Single();

            UpdateEmployeeSkills(selectedSkills, employeeToUpdate);

           if( TryUpdateModel(employeeToUpdate,"",new string[] { "Employee_Name", "Employee_Surname", "Employee_Details", "Hiring_Date" }))
            {
                try
                {

                    UpdateEmployeeSkills(selectedSkills, employeeToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedSkillData(employeeToUpdate);
            return View(employeeToUpdate);
         }

        private void UpdateEmployeeSkills(string[] selectedSkills,     Employee employeeToUpdate)
        {
            if (selectedSkills == null)
            {
                employeeToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedSkillsHS = new HashSet<string>(selectedSkills);
            var employeeSkills = new HashSet<int>
                (employeeToUpdate.Skills.Select(c => c.Skill_id));
            foreach (var skill in db.Skills)
            {
                if (selectedSkillsHS.Contains(skill.Skill_id.ToString()))
                {
                    if (!employeeSkills.Contains(skill.Skill_id))
                    {
                        employeeToUpdate.Skills.Add(skill);
                    }
                }
                else
                {
                    if (employeeSkills.Contains(skill.Skill_id))
                    {
                        employeeToUpdate.Skills.Remove(skill);
                    }
                }
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
