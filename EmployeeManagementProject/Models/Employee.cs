using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementProject.Models
{
    public class Employee
    {
        [Key]
        public int Employee_id { get; set; }

        [Display(Name = "Name:")]
        public string Employee_Name { get; set; }

        [Display(Name = "Surname:")]
        public string Employee_Surname { get; set; }

        [Display(Name = "Details:")]
        public string Employee_Details { get; set; }

        [Display(Name = "Hiring Date:")]
        public DateTime? Hiring_Date { get; set; }

        [Display(Name = "Skill Set:")]
        public virtual ICollection<Skill> Skills  { get; set; }
        public ApplicationUser User { get; set; }

      
    }
}