using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeManagementProject.Models;

namespace EmployeeManagementProject.ViewModels
{
    public class EmployeeIndexData
    {
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}