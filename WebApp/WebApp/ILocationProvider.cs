using WebApp.Models;

namespace WebApp;

public interface ILocationProvider
{
	Task<List<Location>> GetAll();
	Task<Location?> GetById(int id);
	Task DeleteById(int id);
	Task Create(Location location);
	Task Update(Location location);
}