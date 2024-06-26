﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Automapper;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using Sqids;

namespace MyRecipeBook.Application;
public static class DependencyInjectionExtension
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddAutoMapper(services, configuration);
		AddUseCases(services);
	}

	private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
	{
		var sqids = new SqidsEncoder<long>(new()
		{
			MinLength = 3,
			Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
		});

		services.AddScoped(options => new AutoMapper.MapperConfiguration(options =>
		{
			options.AddProfile(new AutoMapping(sqids));
		}).CreateMapper());
	}

	private static void AddUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
		services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
		services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
		services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
		services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
		services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
		services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
	}
}

