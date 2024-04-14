﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Automapper;
using MyRecipeBook.Application.Services.Crytography;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;
public static class DependencyInjectionExtension
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddAutoMapper(services);
		AddUseCases(services);
		AddPasswordEncrypter(services, configuration);
	}

	private static void AddAutoMapper(IServiceCollection services)
	{
		services.AddScoped(options => new AutoMapper.MapperConfiguration(options =>
		{
			options.AddProfile(new AutoMapping());
		}).CreateMapper());
	}

	private static void AddUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
	}

	private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
	{
		var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
		services.AddScoped(options => new PasswordEncripter(additionalKey));
	}
}

