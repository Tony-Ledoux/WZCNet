

using Microsoft.AspNetCore.Mvc;
using WZCNet.src.Application.DTOs.Creation;
using WZCNet.src.Application.Services;


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
            var result = await _srv.GetEmployeeDetailsFromIdAsync(id);
            if(!result.IsSuccess) return Problem(result.Error!,statusCode:404);
            return Ok(result.Value);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewEmployee(EmployeeCreationDTO input)
        {
            var employee = await _srv.CreateEmployeeAsync(input);
            if(!employee.IsSuccess) return Problem(employee.Error!, statusCode: 400);
            return Created("", employee);
            //return Ok(input);
        }
    }
}
