using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EnvironmentController : Controller
    {
        // GET: EnvironmentController
        public ActionResult Index()
        {
            var variables = new List<EnvironmentVariable>();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                variables.Add(new EnvironmentVariable()
                {
                    Name = (string)de.Key,
                    Value = (string)(de.Value ?? "")
                });
            }

            return View(variables);
        }
    }
}
