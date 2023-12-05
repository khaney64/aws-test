using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class LocationController : Controller
	{
		private readonly HttpClient client;
		private readonly ILocationProvider locationProvider;

		public LocationController(IHttpClientFactory httpClientFactory, IAppConfiguration configuration, ILocationProvider locationProvider)
		{
			client = httpClientFactory.CreateClient("api");
			client.BaseAddress = new Uri(configuration.WebApiUrl);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			this.locationProvider = locationProvider;
		}

		// GET: LocationController
		public async Task<ActionResult> Index()
		{
			var locations = new List<Location>();

			try
			{
				locations = await locationProvider.GetAll();
//				locations = await client.GetFromJsonAsync<List<Location>>("api/Location");
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Unexpected {nameof(Index)} error - {e.Message}");
			}

			return View(locations);
		}

		// GET: LocationController/Details/5
		public async Task<ActionResult> Details(int id)
		{
			var location = await locationProvider.GetById(id);
			if (location is null)
			{
				return View("Index");
			}
			return View(location);
		}

		// GET: LocationController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: LocationController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(IFormCollection collection)
		{
			try
			{
				var location = new Location()
				{
					Id = 0,
					Name = collection["Name"],
					City = collection["City"],
					State = collection["State"],
					Zip = collection["Zip"]
				};

				//var res = await client.PostAsJsonAsync<Location>("api/Location",location);

				await locationProvider.Create(location);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				Console.WriteLine($"Unexpected {nameof(Create)} error - {e.Message}");
				return View();
			}
		}

		// GET: LocationController/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			var location = await locationProvider.GetById(id);
			if (location is null)
			{
				return View("Index");
			}
			return View(location);
		}

		// POST: LocationController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, IFormCollection collection)
		{
			try
			{
				var location = new Location()
				{
					Id = id,
					Name = collection["Name"],
					City = collection["City"],
					State = collection["State"],
					Zip = collection["Zip"]
				};
				await locationProvider.Update(location);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: LocationController/Delete/5
		public async Task<ActionResult> Delete(int id)
		{
			var location = await locationProvider.GetById(id);
			if (location is null)
			{
				return View("Index");
			}
			return View(location);
		}

		// POST: LocationController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int id, IFormCollection collection)
		{
			try
			{
				await locationProvider.DeleteById(id);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Unexpected {nameof(Delete)} error - {e.Message}");
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
