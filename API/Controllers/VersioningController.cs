using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class VersioningController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new SuccessResult());
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(new SuccessResult());
        }
    }
}
