using EStore.Application.Interfaces;
using EStore.Application.Services;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
using EStore.Domain.EntityDtos.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
     
        public LoginController(ILoginService loginService )
        {
            _loginService = loginService;
           
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginReq login)
        {
            var token = await _loginService.ProvideToken(login);


            if (string.IsNullOrEmpty(token))
            {
              // if token is emoty it returns  401 
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new { token });
        }      

    }
}
