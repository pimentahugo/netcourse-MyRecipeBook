using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;
public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddRepositories(services);
		
		if(configuration.IsUnitTestEnviroment())
		{
			return;
		}

		AddDbContext(services, configuration);
		AddFluentMigrator(services, configuration);
	}

	private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
		{
			dbContextOptions.UseSqlServer(configuration.ConnectionString());
		});
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserReadOnlyRepository, UserRepository>();
		services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
	}

	private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
	{
		services.AddFluentMigratorCore().ConfigureRunner(options =>
		{
			options.AddSqlServer()
			.WithGlobalConnectionString(configuration.ConnectionString())
			.ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
		});
	}
}

