using System.Diagnostics;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Simple.Models;
using Simple.Services;

namespace Simple.Controllers
{
	static class MyClass
	{
		private static void Test() { }
	}

	public class HomeController : Controller
	{
		public HomeController(
			ILogger<HomeController> logger,
			IWebHostEnvironment webHostEnvironment
		)
		{
			_logger = logger;
			WebHostEnvironment = webHostEnvironment;
		}

		private readonly ILogger<HomeController> _logger;
		public IWebHostEnvironment WebHostEnvironment { get; }


		[HttpGet(nameof(VS16_5_Test))]
		public string VS16_5_Test([FromServices] IPrintService printService)
		{
			static string GetSwitch(int i = 13)
			{
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
						return "Default";
						//break;
						//throw i switch
						//{
						//	_ => new InvalidOperationException(),
						//};
				}

				return default;
			}

			string getSwitch = GetSwitch(1);

			string oldS = $"{4681317222631354044.ToString("N0").PadLeft(4)} .. !!!!";
			string newS = $"{4681317222631354044,4:N0} .. !!!!";

			double redius = 1.13;
			double area = AreaMethod(redius);
			static double AreaMethod(double redius) => 3.14 * redius * redius;

			string printServiceResult = printService.Print();

			string result = 
				$"\n\t GetSwitch: {getSwitch} \n" +
				$"\t NewS: {newS} \n" +
				$"\t Area: {area} \n" +
				$"\t PrintService: {printServiceResult}"
			;

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
