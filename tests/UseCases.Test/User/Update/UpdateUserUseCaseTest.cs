﻿using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.User.Update;
public class UpdateUserUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		(var user, var _) = UserBuilder.Build();

		var request = RequestUpdateJsonBuilder.Build();

		var useCase = CreateUseCase(user);

		Func<Task> act = async () => await useCase.Execute(request);

		await act.Should().NotThrowAsync();

		user.Name.Should().Be(request.Name);
		user.Email.Should().Be(request.Email);
	}

	[Fact]
	public async Task Error_Name_Empty()
	{
		(var user, var _) = UserBuilder.Build();

		var request = RequestUpdateJsonBuilder.Build();
		request.Name = string.Empty;

		var useCase = CreateUseCase(user);

		Func<Task> act = async () => await useCase.Execute(request);

		await act.Should().ThrowAsync<ErrorOrValidationException>().Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

		user.Name.Should().NotBe(request.Name);
		user.Name.Should().NotBe(request.Email);
	}

	[Fact]
	public async Task Error_Email_Already_Registered()
	{
		(var user, var _) = UserBuilder.Build();

		var request = RequestUpdateJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Email);

		Func<Task> act = async () => await useCase.Execute(request);

		await act.Should().ThrowAsync<ErrorOrValidationException>().Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTRED));

		user.Name.Should().NotBe(request.Name);
		user.Email.Should().NotBe(request.Email);
	}

	private UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
	{
		var unitOfWork = UnitOfWorkBuilder.Build();
		var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

		if (!string.IsNullOrEmpty(email))
			userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email!);

		return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
	}
}

