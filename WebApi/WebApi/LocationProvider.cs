using System.Runtime.Caching;
using WebApi.Models;

namespace WebApi
{
	public class LocationProvider
	{
		private static ObjectCache cache = MemoryCache.Default;

		static LocationProvider()
		{
			CacheItemPolicy policy = new CacheItemPolicy();

			var locations = new List<Location>()
			{
				new Location { Id = 1, Name = "Home", City = "Downingtown", State = "PA", Zip = "19335" },
				new Location { Id = 2, Name = "Brenna", City = "Astoria", State = "NY", Zip = "11106" },
				new Location { Id = 3, Name = "Moira", City = "Savannah", State = "GA", Zip = "31405" },
				new Location { Id = 4, Name = "Kyle", City = "Thorndale", State = "PA", Zip = "19372" },
				new Location { Id = 5, Name = "Mom and Dad", City = "Levittown", State = "PA", Zip = "19057" }
			};

			cache.Set("locations", locations, policy);
		}

		public static List<Location> GetAll()
		{
			return cache["locations"] as List<Location>;
		}

		public static Location? GetById(int id)
		{
			var locations = cache["locations"] as List<Location>;
			return locations?.Find(l => l.Id == id);
		}

		public static void DeleteById(int id)
		{
			var locations = cache["locations"] as List<Location>;
			locations?.RemoveAll(l => l.Id == id);
		}

		public static void Create(Location location)
		{
			var locations = cache["locations"] as List<Location>;
			if (locations is null)
			{
				locations = new List<Location>();
				cache.Set("locations", locations, new CacheItemPolicy());
			}
			location.Id = GetNextId();
			locations.Add(location);
		}

		private static int GetNextId()
		{
			var locations = cache["locations"] as List<Location>;
			var max = locations?.Max(l => l.Id) ?? 0;

			return max + 1;
		}

	}
}
