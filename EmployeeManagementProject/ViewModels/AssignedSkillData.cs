using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeManagementProject.Models;

namespace EmployeeManagementProject.ViewModels
{
    public class AssignedSkillData
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public bool Assigned { get; set; }
    }
}