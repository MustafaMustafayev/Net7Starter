using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.ActionFilters;
using Project.Entity.Entities.Redis;
using Redis.OM;
using Redis.OM.Searching;

namespace Project.API.Controllers;

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

    [HttpPost]
    public async Task<Person> AddPerson([FromBody] Person person)
    {
        await _collection.InsertAsync(person);
        return person;
    }

    [HttpGet("filterAge")]
    public List<Person> FilterByAge([FromQuery] int minAge, [FromQuery] int maxAge)
    {
        return _collection.Where(x => x.Age >= minAge && x.Age <= maxAge).ToList();
    }

    [HttpGet("filterGeo")]
    public List<Person> FilterByGeo([FromQuery] double lon, [FromQuery] double lat, [FromQuery] double radius,
        [FromQuery] string unit)
    {
        return _collection.GeoFilter(x => x.Address!.Location, lon, lat, radius, Enum.Parse<GeoLocDistanceUnit>(unit))
            .ToList();
    }

    [HttpGet("filterName")]
    public List<Person> FilterByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        return _collection.Where(x => x.FirstName == firstName && x.LastName == lastName).ToList();
    }

    [HttpGet("postalCode")]
    public List<Person> FilterByPostalCode([FromQuery] string postalCode)
    {
        return _collection.Where(x => x.Address!.PostalCode == postalCode).ToList();
    }

    [HttpGet("fullText")]
    public List<Person> FilterByPersonalStatement([FromQuery] string text)
    {
        return _collection.Where(x => x.PersonalStatement == text).ToList();
    }

    [HttpGet("streetName")]
    public List<Person> FilterByStreetName([FromQuery] string streetName)
    {
        return _collection.Where(x => x.Address!.StreetName == streetName).ToList();
    }

    [HttpGet("skill")]
    public List<Person> FilterBySkill([FromQuery] string skill)
    {
        return _collection.Where(x => x.Skills.Contains(skill)).ToList();
    }

    [HttpPatch("updateAge/{id}")]
    public IActionResult UpdateAge([FromRoute] string id, [FromBody] int newAge)
    {
        foreach (var person in _collection.Where(x => x.Id == id)) person.Age = newAge;
        _collection.Save();
        return Accepted();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePerson([FromRoute] string id)
    {
        _provider.Connection.Unlink($"Person:{id}");
        return NoContent();
    }
}