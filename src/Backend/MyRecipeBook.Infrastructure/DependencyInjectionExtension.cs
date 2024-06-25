﻿using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;
public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddPasswordEncrypter(services, configuration);
		AddRepositories(services);
		AddLoggedUser(services);
		AddTokens(services, configuration);

		if (configuration.IsUnitTestEnviroment())
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
		services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
		services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
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

	private static void AddTokens(IServiceCollection services, IConfiguration configuration)
	{
		var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
		var signinKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

		services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
		services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signinKey!));
	}

	private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

	private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
	{
		var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
		services.AddScoped<IPasswordEncripter>(options => new Sha512Encripter(additionalKey!));
	}
}

