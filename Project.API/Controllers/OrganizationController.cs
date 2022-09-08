using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.BLL.Abstract;
using Project.DTO.DTOs.OrganizationDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogActionFilter))]
    [AllowAnonymous]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [SwaggerOperation(Summary = "get organizations")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<OrganizationToListDTO>))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _organizationService.GetAsync());
        }

        [SwaggerOperation(Summary = "get organization")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(OrganizationToListDTO))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _organizationService.GetAsync(id));
        }

        [SwaggerOperation(Summary = "create organization")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrganizationToAddOrUpdateDTO dto)
        {
            return Ok(await _organizationService.AddAsync(dto));
        }

        [SwaggerOperation(Summary = "update organization")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrganizationToAddOrUpdateDTO dto)
        {
            return Ok(await _organizationService.UpdateAsync(id, dto));
        }

        [SwaggerOperation(Summary = "delete organization")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(void))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _organizationService.DeleteAsync(id);
            return Ok();
        }
    }
}

