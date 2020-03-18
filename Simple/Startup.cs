using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using Simple.Data;
using Simple.Services;

namespace Simple
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddControllersWithViews();
			services.AddRazorPages();

			services.AddSingleton<IPrintService, PrintService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();

			string WebRootPath = Configuration["PathSettings:WebRootPath"];
			if (string.IsNullOrEmpty(WebRootPath)) WebRootPath = Environment.CurrentDirectory;

			bool showUrls = Configuration.GetSection("PathSettings").GetValue<bool>("ShowUrls");
			bool usedefaultFiles = Configuration.GetSection("PathSettings").GetValue<bool>("UseDefaultFiles");

			string defaultFiles = Configuration["PathSettings:DefaultFiles"];
			if (string.IsNullOrEmpty(defaultFiles)) defaultFiles = "index.html,default.htm,default.html";

			if (showUrls)
				app.Use(async (context, next) =>
				{
					string url =
						$"{context.Request.Scheme}" +
						$"://{context.Request.Host}" +
						$"{context.Request.Path}" +
						$"{context.Request.QueryString}"
					;

					Console.WriteLine(url);

					await next();
				});

			if (usedefaultFiles)
				app.UseDefaultFiles(new DefaultFilesOptions
				{
					FileProvider = new PhysicalFileProvider(WebRootPath),
					DefaultFileNames = new List<string>(defaultFiles.Split(',', ';'))
				});

			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
				RequestPath = new PathString(string.Empty)
			});

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
