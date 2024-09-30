using Domain.DTO;
using Domain.Models;
using Domain.ReturnsModels;
using Domain.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Service.IServices;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatorController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IJwtService<JWTDataModel> jwtService;

        public AuthenticatorController(IAuthService authservice,IJwtService<JWTDataModel> jwtService)
        {
            this._authservice = authservice;
            this.jwtService = jwtService;
        }
        [HttpGet("RemoveToken")]
        public IActionResult RemoveToken()
        {
            string x = string.Empty;
            var IsFound =  HttpContext.Request.Cookies.TryGetValue("RefreshToken", out x);
            if(IsFound)
            {
                HttpContext.Response.Cookies.Delete("RefreshToken");
                _authservice.RemoveRefreshToken(x);
                return Ok();
            }
            else
            {
                return BadRequest("Cookies Not Defined");
            }
        }
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {

            string x = string.Empty;
            HttpContext.Request.Cookies.TryGetValue("RefreshToken", out x);
            if(string.IsNullOrEmpty(x))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            else
            {
                var UserRemoved =  _authservice.RemoveRefreshToken(x);
                if (UserRemoved != null)
                {
                    var genrate =  _authservice.GenerateRefresh(UserRemoved);
                    if(genrate !=null)
                    {
                        return Ok(genrate);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized);
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }

        }
        [HttpPost("Register")]
        public IActionResult Register(RegstirDto dto)
        {
            var response = _authservice.Register(dto);
            if (response.IsSucceeded)
            {
                return Ok(response);

            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpPost("Auth")]
        public IActionResult Auth(AuthDto dto)
        {
            var response =  _authservice.Auth(dto);
            if(response.IsSucceeded)
            {
                return Ok(response);

            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpPost("DecodeEnc")]
        public IActionResult DecodeEnc(DecodeJwt EncryptionJwt)
        {
            Console.WriteLine(EncryptionJwt);

            Console.WriteLine("test");
            var response = jwtService.Get(EncryptionJwt.EncodedJwt);
            if (response.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
