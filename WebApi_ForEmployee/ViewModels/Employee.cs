using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi_ForEmployees.ViewModels
{
    public class Employee
    {
        public string EmpName { get; set; }
        public int EmpId { get; set; }
        public string Department { get; set; }
        public Nullable<int> Salary { get; set; }
        public string Job { get; set; }
    }
}