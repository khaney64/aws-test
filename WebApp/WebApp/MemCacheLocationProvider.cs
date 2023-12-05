using System.Runtime.Caching;
using WebApp.Models;

namespace WebApp
{
	public class MemCacheLocationProvider : ILocationProvider
	{
		public static ObjectCache cache = MemoryCache.Default;

		public MemCacheLocationProvider()
		{
			CacheItemPolicy policy = new CacheItemPolicy();

			var locations = new Dictionary<int,Location>()
			{
				{ 1, new Location { Id = 1, Name = "Home", City = "Downingtown", State = "PA", Zip = "19335" }},
				{ 2, new Location { Id = 2, Name = "Brenna", City = "Astoria", State = "NY", Zip = "11106" }},
				{ 3, new Location { Id = 3, Name = "Moira", City = "Savannah", State = "GA", Zip = "31405" }},
				{ 4, new Location { Id = 4, Name = "Kyle", City = "Thorndale", State = "PA", Zip = "19372" }},
				{ 5, new Location { Id = 5, Name = "Mom and Dad", City = "Levittown", State = "PA", Zip = "19057" }}
			};

			cache.Set("locations", locations, policy);
		}

		public Task<List<Location>> GetAll()
		{
			return Task.FromResult(GetCachedLocations().Values.ToList());
		}

		public Task<Location?> GetById(int id)
		{

			var locations = GetCachedLocations();

			var location = locations.TryGetValue(id, out var loc) ? loc : null;

			return Task.FromResult(location);
		}

		public Task DeleteById(int id)
		{
			var locations = GetCachedLocations();
			locations.Remove(id);
			UpdateCachedLocations(locations);
			
			return Task.CompletedTask;
		}

		public Task Create(Location location)
		{
			var locations = GetCachedLocations();
			location.Id = GetNextId();
			locations.Add(location.Id,location);

			UpdateCachedLocations(locations);

			return Task.CompletedTask;
		}

		public async Task Update(Location location)
		{
			var sourceLocation = await GetById(location.Id);
			if (sourceLocation is not null)
			{
				sourceLocation.Name = location.Name;
				sourceLocation.City = location.City;
				sourceLocation.State = location.State;
				sourceLocation.Zip = location.Zip;

				var locations = GetCachedLocations();

				locations[location.Id] = sourceLocation;
				UpdateCachedLocations(locations);
			}
		}

		private int GetNextId()
		{
			var locations = GetCachedLocations();
			var max = locations?.Values.Max(l => l.Id) ?? 0;

			return max + 1;
		}

		Dictionary<int, Location> GetCachedLocations()
		{
			var locations = cache["locations"] as Dictionary<int, Location>;
			if (locations is null)
			{
				locations = new Dictionary<int, Location>();
				UpdateCachedLocations(locations);
			}

			return locations;
		}

		void UpdateCachedLocations(Dictionary<int, Location> locations)
		{
			cache.Set("locations", locations, new CacheItemPolicy());
		}
	}
}
