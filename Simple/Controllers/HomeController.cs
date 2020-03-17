using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simple.Models;

namespace Simple.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			WebHostEnvironment = webHostEnvironment;
		}

		private readonly ILogger<HomeController> _logger;
		public IWebHostEnvironment WebHostEnvironment { get; }


		public IActionResult Index() => View();


		[HttpGet(nameof(RootPath))]
		public IActionResult RootPath()
		{
			string webRootPath = WebHostEnvironment.WebRootPath;
			string contentRootPath = WebHostEnvironment.ContentRootPath;

			string result = $"{ webRootPath} \n { contentRootPath }";

			_logger.LogInformation(result);

			return Content(result);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
