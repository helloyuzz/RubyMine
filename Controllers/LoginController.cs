using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RubyMine.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {
        private readonly IJwtAuth jwtAuth;
        public LoginController(IJwtAuth jwtAuth) {
            this.jwtAuth = jwtAuth;
        }
        // GET: api/<AuthController>
        //[HttpGet]
        //public IEnumerable<string> Get() {
        //    return new string[] { "username", "password" };
        //}
        [AllowAnonymous]
        // POST api/<AuthController>
        [HttpPost("Authentication")]
        public IActionResult Authentication([FromBody] UserCredential userCredential) {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
