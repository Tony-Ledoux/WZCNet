using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WZCNet.src.Application.DTOs.Requests.User;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("{accountId:int}/employee")]
        public async Task<IActionResult> AddEmployeeToUser(int accountId, AddEmployeeRequestDto input)
        {
            var result = await userService.AddEmployeeToUser(input.EmployeeId, accountId);
            if(!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
