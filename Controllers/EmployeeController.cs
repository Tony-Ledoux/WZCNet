using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WZCNet.Contexts;
using WZCNet.Entities;
using WZCNet.Models.Creation;
using WZCNet.Services;

namespace WZCNet.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        private readonly IEmployeeService _srv = service;

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var Employees = await _srv.GetEmployeesAsync();
            return Ok(Employees);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            var Employee = await _srv.GetEmployeeDetailsFromIdAsync(id);
            return Ok(Employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewEmployee(EmployeeCreationDTO input)
        {
           //var employee = await _srv.CreateEmployeeAsync(input);
           //return Created("",employee);
           return Ok(input);
        }

    }
}
