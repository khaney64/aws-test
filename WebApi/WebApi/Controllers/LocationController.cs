using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		// GET: api/<LocationController>
		[HttpGet]
		public IEnumerable<Location> Get()
		{
			return LocationProvider.GetAll();
		}

		// GET api/<LocationController>/5
		[HttpGet("{id}")]
		public Location? Get(int id)
		{
			return LocationProvider.GetById(id);
		}

		// POST api/<LocationController>
		[HttpPost]
		public void Post([FromBody] Location value)
		{
			LocationProvider.Create(value);
		}

		// PUT api/<LocationController>
		[HttpPut()]
		public void Put([FromBody] Location value)
		{
			var location = LocationProvider.GetById(value.Id);
			if (location == null)
			{
				LocationProvider.Create(value);
			}
			else
			{
				location.Name = value.Name;
				location.City = value.City;
				location.State = value.State;
				location.Zip = value.Zip;
			}
		}

		// DELETE api/<LocationController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			LocationProvider.DeleteById(id);
		}
	}
}