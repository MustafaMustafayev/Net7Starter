using API.ActionFilters;
using DTO.Responses;
using ENTITIES.Entities.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;
using Redis.OM.Searching;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize]
public class PersonController : Controller
{
    private readonly RedisCollection<Person> _collection;
    private readonly RedisConnectionProvider _provider;

    public PersonController(RedisConnectionProvider provider)
    {
        _provider = provider;
        _collection = (RedisCollection<Person>)provider.RedisCollection<Person>();
    }

    [SwaggerOperation(Summary = "add person to redis")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] Person person)
    {
        await _collection.InsertAsync(person);
        return Ok(new SuccessResult());
    }

    [SwaggerOperation(Summary = "filter by age")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("filterAge")]
    public async Task<IActionResult> FilterByAge([FromQuery] int minAge, [FromQuery] int maxAge)
    {
        var datas = _collection.Where(x => x.Age >= minAge && x.Age <= maxAge).ToList();
        return Ok(new SuccessDataResult<List<Person>>(datas));
    }

    [SwaggerOperation(Summary = "filter by geo")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("filterGeo")]
    public IActionResult FilterByGeo([FromQuery] double lon, [FromQuery] double lat, [FromQuery] double radius,
        [FromQuery] string unit)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection
            .GeoFilter(x => x.Address!.Location, lon, lat, radius, Enum.Parse<GeoLocDistanceUnit>(unit)).ToList()));
    }

    [SwaggerOperation(Summary = "filter by name")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("filterName")]
    public IActionResult FilterByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection
            .Where(x => x.FirstName == firstName && x.LastName == lastName).ToList()));
    }

    [SwaggerOperation(Summary = "filter by postal code")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("postalCode")]
    public IActionResult FilterByPostalCode([FromQuery] string postalCode)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection.Where(x => x.Address!.PostalCode == postalCode)
            .ToList()));
    }

    [SwaggerOperation(Summary = "filter by full text")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("fullText")]
    public IActionResult FilterByPersonalStatement([FromQuery] string text)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection.Where(x => x.PersonalStatement == text).ToList()));
    }

    [SwaggerOperation(Summary = "filter by street name")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("streetName")]
    public IActionResult FilterByStreetName([FromQuery] string streetName)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection.Where(x => x.Address!.StreetName == streetName)
            .ToList()));
    }

    [SwaggerOperation(Summary = "filter by skill")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpGet("skill")]
    public IActionResult FilterBySkill([FromQuery] string skill)
    {
        return Ok(new SuccessDataResult<List<Person>>(_collection.Where(x => x.Skills.Contains(skill)).ToList()));
    }

    [SwaggerOperation(Summary = "update person to redis")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpPatch("updateAge/{id}")]
    public IActionResult UpdateAge([FromRoute] string id, [FromBody] int newAge)
    {
        foreach (var person in _collection.Where(x => x.Id == id)) person.Age = newAge;
        _collection.Save();
        return Ok(new SuccessResult());
    }

    [SwaggerOperation(Summary = "delete person from redis")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [HttpDelete("{id}")]
    public IActionResult DeletePerson([FromRoute] string id)
    {
        _provider.Connection.Unlink($"Person:{id}");
        return Ok(new SuccessResult());
    }
}