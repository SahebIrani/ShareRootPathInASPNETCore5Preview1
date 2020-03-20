using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simple.Data;
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
			IWebHostEnvironment webHostEnvironment,
			ApplicationDbContext applicationDbContext
		)
		{
			_logger = logger;
			WebHostEnvironment = webHostEnvironment;
			ApplicationDbContext = applicationDbContext;
		}

		private readonly ILogger<HomeController> _logger;
		public IWebHostEnvironment WebHostEnvironment { get; }
		public ApplicationDbContext ApplicationDbContext { get; }


		[HttpGet(nameof(EFCore5CreateDbCommandQueryStringAsync))]
		public async ValueTask<OkResult> EFCore5CreateDbCommandQueryStringAsync(CancellationToken ct = default)
		{
			//? Simple way to get generated SQL
			const string city = "Sari";
			IQueryable<Address> query = ApplicationDbContext.Addresses.Where(c => c.City == city);
			string queryString = query.ToQueryString();

			Console.WriteLine(queryString);
			_logger.LogInformation(queryString);

			//? This will work in simple cases, but the translation to a query string and back to a command loses some information. For example,
			//? if a transaction is being used then the code above would need to find that transaction and associate itself.
			DbConnection connection = ApplicationDbContext.Database.GetDbConnection();
			using (DbCommand command = connection.CreateCommand())
			{
				command.CommandText = query.ToQueryString();

				await connection.OpenAsync(ct);

				using (DbDataReader results = await command.ExecuteReaderAsync(ct))
				{
				}

				await connection.CloseAsync();
			}

			//? Instead, EF Core 5.0 introduces CreateDbCommand which creates and configures a DbCommand just as EF does to execute the query. For example:
			DbConnection connection2 = ApplicationDbContext.Database.GetDbConnection();
			using (DbCommand command = query.CreateDbCommand())
			{
				await connection2.OpenAsync(ct);

				using (DbDataReader results = command.ExecuteReader())
				{
				}

				await connection2.CloseAsync();
			}

			return Ok();
		}

		[HttpGet(nameof(EFCore5DemoAsync))]
		public async ValueTask<OkResult> EFCore5DemoAsync(CancellationToken ct = default)
		{
			//? Simple way to get generated SQL
			IQueryable<Address> query = ApplicationDbContext.Addresses.Where(c => c.City == "Sari");
			string queryString = query.ToQueryString();

			Console.WriteLine(queryString);
			_logger.LogInformation(queryString);

			//? Query translations for more DateTime constructs
			int count = await ApplicationDbContext.Addresses
				.CountAsync(c => c.CreateDate >= EF.Functions.DateFromParts(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), ct);

			//? Query translations for more byte array constructs
			IEnumerable<Customer> blogs =
				await ApplicationDbContext.Customers.Where(e => e.Picture.Contains((byte)127)).ToListAsync(ct);

			//? Query translation for Reverse
			IEnumerable<Customer> getReverse =
				await ApplicationDbContext.Customers.OrderBy(e => e.CustomerId).Reverse().ToListAsync(ct);

			//? Query translation for bitwise operators
			//? &(bitwise AND)
			//? | (bitwise OR)
			//? ~(bitwise NOT)
			//? ^(bitwise XOR)
			//? << (bitwise left shift)
			//? >> (bitwise right shift)
			//? >>> (bitwise unsigned right shift)
			//? &= (bitwise AND assignment)
			//? |= (bitwise OR assignment)
			//? ^= (bitwise XOR assignment)
			//? <<= (bitwise left shift and assignment)
			//? >>= (bitwise right shift and assignment)
			//? >>>= (bitwise unsigned right shift and assignment)
			IEnumerable<Customer> getNegated =
				await ApplicationDbContext.Customers.Where(o => ~o.CustomerId == -2).ToListAsync(ct);

			return Ok();
		}

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
