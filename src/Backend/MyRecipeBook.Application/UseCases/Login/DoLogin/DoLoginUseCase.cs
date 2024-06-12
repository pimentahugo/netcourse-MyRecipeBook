﻿using MyRecipeBook.Application.Services.Crytography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
	private readonly IUserReadOnlyRepository _repository;
	private readonly PasswordEncripter _passwordEncripter;

    public DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter)
    {
        _repository = repository;
		_passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
	{
		var encripterPassword = _passwordEncripter.Encrypt(request.Password);

		var user = await _repository.GetByEmailAndPassword(request.Email, encripterPassword) ?? throw new InvalidLoginException();

		return new ResponseRegisteredUserJson {
			Name = user.Name
		};
	}
}

