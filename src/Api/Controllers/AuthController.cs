using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Requests.Auth;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ITokenService ts, IUserService us) : ControllerBase
    {
       
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(LoginRequestDto request)
        {
            var user = await us.Register(request);
            if(!user.IsSuccess) return BadRequest(user.Error);
            return Ok(user.Value);
        }
        /*
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequestDto request, ITokenService _ts)
        {
            if(user.UserName != request.UserName) return BadRequest("user not found");
            if(new PasswordHasher<AppUser>().VerifyHashedPassword(user,user.PasswordHash,request.Password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong Password");
            }
            var ts = new TokenClaimsDTO {
                UserName = user.UserName
            };
            string token = await _ts.CreateBearerToken(ts);
            return Ok(token);
        }
        [Authorize]
        [HttpGet]
        public ActionResult<string> Test()
        {
            return Ok("dit is een test");
        }
        */
    }
}
