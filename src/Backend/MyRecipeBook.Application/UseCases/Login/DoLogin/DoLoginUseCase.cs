﻿using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
	private readonly IUserReadOnlyRepository _repository;
	private readonly IPasswordEncripter _passwordEncripter;
	private readonly IAccessTokenGenerator _accessTokenGenerator;
    public DoLoginUseCase(IUserReadOnlyRepository repository,
		IPasswordEncripter passwordEncripter,
		IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
		_passwordEncripter = passwordEncripter;
		_accessTokenGenerator = accessTokenGenerator;

    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
	{
		var encripterPassword = _passwordEncripter.Encrypt(request.Password);

		var user = await _repository.GetByEmailAndPassword(request.Email, encripterPassword) ?? throw new InvalidLoginException();

		return new ResponseRegisteredUserJson {
			Name = user.Name,
			Tokens = new ResponseTokensJson
			{
				AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
			}
		};
	}
}

