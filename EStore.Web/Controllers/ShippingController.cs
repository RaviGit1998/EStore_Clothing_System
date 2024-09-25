using EStore.Application.Interfaces;
using EStore.Application.Services;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.NewFolder;
using EStore.Domain.EntityDtos.OrderDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
          _shippingService = shippingService;
        }
      

        [HttpPost]
        public async Task<IActionResult> CreateShipping(ShippingDto shippingDto)
        {
            if (shippingDto == null) return BadRequest("shipping Cannot be null");
            try
            {
                var shippingdetails = await _shippingService.Createshipping(shippingDto);

                return Ok(shippingdetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
