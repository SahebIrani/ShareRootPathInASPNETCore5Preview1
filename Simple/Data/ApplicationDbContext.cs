using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Simple.Data
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
				new DbContextOptionsBuilder<ApplicationDbContext>();

			optionsBuilder
				.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DotNetCore5DemoDb;Trusted_Connection=True;MultipleActiveResultSets=true")
			;

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}

	public class ApplicationDbContext : IdentityDbContext 
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Customer> Customers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			#region Simple Logging
			//optionsBuilder.LogTo(Console.WriteLine);
			//? Set the minimum log level
			//optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
			//? Filter for only specific events:
			//? optionsBuilder.LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted });
			//? Filter for all events in specific categories:
			//? optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Name }, LogLevel.Information);
			//? Use a custom filter over event and level:
			optionsBuilder.LogTo(Console.WriteLine, (id, level) => id == RelationalEventId.CommandExecuting);
			#endregion
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//? A new IsRelational method has been added in addition to the existing IsSqlServer, IsSqlite, and IsInMemory. 
			//? if (Database.IsRelational())
			//{
			//	modelBuilder.Entity<Address>().HasNoKey();
			//}
			//else Console.WriteLine("Database != IsRelational");

			modelBuilder.Entity<Address>().HasNoKey();
		}
	}

	[Keyless]
	public class Address
	{
		public string Street { get; set; }
		public string City { get; set; }
		public int Zip { get; set; }
		public DateTime CreateDate { get; set; }
	}
	public class Customer
	{
		public int CustomerId { get; set; }
		public string FullName { get; set; }
		public byte[] Picture { get; set; }
	}
}
