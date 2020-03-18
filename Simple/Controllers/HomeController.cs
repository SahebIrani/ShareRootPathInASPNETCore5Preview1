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
	static class MyClass
	{
		private static void Test() { }

	}

	public class HomeController : Controller
	{
		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			WebHostEnvironment = webHostEnvironment;
		}

		private readonly ILogger<HomeController> _logger;
		public IWebHostEnvironment WebHostEnvironment { get; }


		[HttpGet(nameof(VS16_5_Test))]
		public string VS16_5_Test()
		{
			int i = 13;

			switch (i)
			{
				case 0:
					switch (i)
					{
						case 0:
							return "0";
					}

					break;
				case 1:
					switch (i)
					{
						case 1:
							return "1";
					}

					break;
				default:
					break;
					//throw i switch
					//{
					//	_ => new InvalidOperationException(),
					//};
			}


			string oldS = $"{4681317222631354044.ToString("N0").PadLeft(4)} .. !!!!";
			string newS = $"{4681317222631354044,4:N0} .. !!!!";

			double redius = 1.13;
			double area = AreaMethod(redius);
			static double AreaMethod(double redius) => 3.14 * redius * redius;

			string result = $"{newS} \n {area}";

			return result;
		}

		public ViewResult Index() => View();


		[HttpGet(nameof(RootPath))]
		public ContentResult RootPath()
		{
			string webRootPath = WebHostEnvironment.WebRootPath;
			string contentRootPath = WebHostEnvironment.ContentRootPath;

			string result = $"{ webRootPath} \n { contentRootPath }";

			_logger.LogInformation(result);

			return Content(result);
		}

		public ViewResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public ViewResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
