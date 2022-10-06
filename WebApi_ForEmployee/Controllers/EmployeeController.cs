using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi_ForEmployees.ViewModels;
namespace WebApi_ForEmployees.Controllers
{
    public class EmployeeController : ApiController
    {

        public IHttpActionResult GetAllEmployees()
        {
            EMPLOYEEEntities eMPLOYEEEntities = new EMPLOYEEEntities();
            List<Employee> employees = null;

            employees = eMPLOYEEEntities.EmployeeDetails.Select(c => new Employee { Department=c.Department, EmpId=c.EmpId, EmpName=c.EmpName, Job=c.Job, Salary=c.Salary }).ToList<Employee>();
            if (employees.Count == 0)
                return NotFound();
            else
            return Ok(employees);


        }

        public IHttpActionResult GetEmployeeById(int empId)
        {
            EMPLOYEEEntities eMPLOYEEEntities = new EMPLOYEEEntities();
            Employee employee = null;

            employee = eMPLOYEEEntities.EmployeeDetails.Where(v=>v.EmpId==empId).Select(c => new Employee { Department = c.Department, EmpId = c.EmpId, EmpName = c.EmpName, Job = c.Job, Salary = c.Salary }).FirstOrDefault<Employee>();
            if (employee==null)
                return NotFound();
            else
                return Ok(employee);


        }
    }
}
