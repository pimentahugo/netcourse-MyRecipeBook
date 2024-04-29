﻿using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.User.Register;
public class RegisterUserUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var useCase = CreateUseCase();

		var result = await useCase.Execute(request);

		result.Should().NotBeNull();
		result.Name.Should().Be(request.Name);
	}

	[Fact]
	public async Task Error_Email_Already_Registered()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var useCase = CreateUseCase(request.Email);

		Func<Task> act = async () => await useCase.Execute(request);

		(await act.Should().ThrowAsync<ErrorOrValidationException>())
			.Where(exception => exception.ErrorMessages.Count == 1 && exception.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTRED));
	}

	[Fact]
	public async Task Error_Name_Empty()
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		var useCase = CreateUseCase();

		Func<Task> act = async () => await useCase.Execute(request);

		(await act.Should().ThrowAsync<ErrorOrValidationException>())
			.Where(exception => exception.ErrorMessages.Count == 1 && exception.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
	}

	private RegisterUserUseCase CreateUseCase(string? email = null)
	{
		var mapper = MapperBuilder.Build();
		var passwordEncripter = PasswordEncripterBuilder.Build();
		var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
		var unitOfWork = UnitOfWorkBuilder.Build();
		var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

		if(string.IsNullOrEmpty(email) == false)
		{
			readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);
		}

		return new RegisterUserUseCase(writeRepository, readOnlyRepositoryBuilder.Build(), mapper, unitOfWork, passwordEncripter);
	}
}