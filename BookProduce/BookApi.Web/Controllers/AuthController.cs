using BookApi.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shirley.Book.Service.AuthServices;
using Shirley.Book.Model;

namespace BookApi.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        /// <summary>
        /// Login method controller
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Pwd))
            {
                Log.Information("UserName or Password is incorrect");
                return BadRequest(new BaseResponse { Message = "UserName or Password is incorrect", Result = false });
            }
           return Ok(_authServices.Login(userInfo));
        }


        [HttpPost("LoginPost")]
        public IActionResult LoginPost(string userName, string pwd)
        {
            return Ok();
        }

        [HttpPost("LoginPost2")]
        public IActionResult LoginPost2(UserInfo userInfo)
        {
            return Ok();
        }

    }
}
