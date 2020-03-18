using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Simple
{
	public class Program
	{
		public static Task Main(string[] args) => CreateHostBuilder2(args).Build().RunAsync();

		//? EF Core uses this method at design time to access the DbContext
		public static IHostBuilder CreateHostBuilder2(string[] args)
			=> Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(
					webBuilder =>
						webBuilder
							  .UseWebRoot(@"C:\SinjulMSBH\Codes\PathSettings\AltStaticFiles")
							  .UseStartup<Startup>()
				);

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseEnvironment("Development")
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseContentRoot(Assembly.GetEntryAssembly().Location)
				.UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly()
					.Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\"))))
				//.UseContentRoot(@"D:\SinjulMSBH\New\BK\SampleCodes\09\ASPNETCore5Preview1\Demo")
				.ConfigureWebHostDefaults(webBuilder =>
				{
					//webBuilder.UseContentRoot("D:\\SinjulMSBH\\New\\BK\\SampleCodes\\09\\ASPNETCore5Preview1\\Demo");
					webBuilder.UseWebRoot(@"C:\SinjulMSBH\Codes\PathSettings\AltStaticFiles");

					webBuilder.CaptureStartupErrors(true);
					webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");

					webBuilder.UseUrls("http://*:5000;https://localhost:5001");

					webBuilder.UseStartup<Startup>();
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.Configure<HostOptions>(option =>
					{
						option.ShutdownTimeout = System.TimeSpan.FromSeconds(40);
					});
				})
			;
	}
}
