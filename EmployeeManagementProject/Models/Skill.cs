using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementProject.Models
{
    public class Skill
    {
        [Key]
        public int Skill_id  { get; set; }

        public string Skill_Name { get; set; }
        public string Skill_Description { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Skill_Creation_Date { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public ApplicationUser User { get; set; }
    }
}