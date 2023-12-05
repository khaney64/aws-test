using System.Net.Http.Headers;
using WebApp.Models;

namespace WebApp
{
	public class WebApiLocationProvider : ILocationProvider
	{

		private readonly HttpClient client;

		public WebApiLocationProvider(IHttpClientFactory httpClientFactory, IAppConfiguration configuration)
		{
			client = httpClientFactory.CreateClient("api");
			client.BaseAddress = new Uri(configuration.WebApiUrl);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<List<Location>> GetAll()
		{
			try
			{
				return await client.GetFromJsonAsync<List<Location>>("api/Location") ?? new List<Location>();
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(GetAll)} error - {e.Message}");
				return new List<Location>();
			}
		}

		public async Task<Location?> GetById(int id)
		{
			try
			{
				return await client.GetFromJsonAsync<Location>($"api/Location/{id}");
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(GetById)} error - {e.Message}");
				return null;
			}
		}

		public async Task DeleteById(int id)
		{
			try
			{
				await client.DeleteAsync($"api/Location/{id}");
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(DeleteById)} error - {e.Message}");
			}
		}

		public async Task Create(Location location)
		{
			try
			{ 
				await client.PostAsJsonAsync<Location>($"api/Location", location);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(Create)} error - {e.Message}");
			}
		}

		public async Task Update(Location location)
		{
			try
			{
				await client.PutAsJsonAsync<Location>($"api/Location", location);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(Update)} error - {e.Message}");
			}
		}
	}
}
