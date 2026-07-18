
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WZCNet.Contexts;
using WZCNet.Entities;
using WZCNet.Models.Creation;
using WZCNet.Services;

namespace WZCNet.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController(IEmployeeService service, WZCNetDbContext context) : ControllerBase
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
            var employee = await _srv.CreateEmployeeAsync(input);
            return Created("", employee);
            //return Ok(input);
        }
        [HttpPost("{id:int}/pin-generate")]
        public async Task<IActionResult> GeneratePinForEmployeeWithId(int id)
        {
            Random rnd = new();
            string new_pin = rnd.Next(0,1000000).ToString("D6");
            var employee = await context.Employees.Include(e => e.Pin).FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();
            if (employee.Pin != null)
            {
                // Update the existing record
                employee.Pin.PinHash = new_pin;
                employee.Pin.PinChangedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new if it doesn't exist
                employee.Pin = new EmployeeAuthentication { PinHash = new_pin };
            }
            await context.SaveChangesAsync();
            return Ok(employee);
        }

    }
}
