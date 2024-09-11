using EStore.Application.Interfaces;
using EStore.Domain.EntityDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserReq userReq)
        {
            //validation of model 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 
            }

            try
            {
                var newUser = await _userService.RegisterUser(userReq);

                if (newUser == null)
                {
                   //it will be null if usr already exists
                    return Conflict(new { message = "A user with this email already exists." });//returns 409
                }

                // Return the created user details with 201 Created status
                return CreatedAtAction(nameof(RegisterUser), new { id = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {             
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
