
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.DTOs.Responses;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService us) : ControllerBase
    {
       
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(LoginRequestDto request)
        {
            var user = await us.Register(request);
            if(!user.IsSuccess) return BadRequest(user.Error);
            return Ok(user.Value);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var r = await us.Login(request);
            if(!r.IsSuccess) return BadRequest(r.Error);
            return Ok(r.Value);
        }
        /*
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDto request)
        {
            var r = await us.Refresh(request);
            if(!r.IsSuccess) return BadRequest(r.Error);
            return Ok(r.Value);
        }
        */
        [Authorize]
        [HttpGet]
        public ActionResult<string> Test()
        {
            return Ok("dit is een test");
        }
        
    }
}
