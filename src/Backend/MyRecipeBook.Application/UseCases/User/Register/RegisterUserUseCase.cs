﻿using AutoMapper;
using MyRecipeBook.Application.Services.Crytography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
	private readonly IUserWriteOnlyRepository _writeOnlyRepository;
	private readonly IUserReadOnlyRepository _readOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly PasswordEncripter _passwordEncripter;

	public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository,
		IUserReadOnlyRepository readOnlyRepository,
		IMapper mapper,
		IUnitOfWork unitOfWork,
		PasswordEncripter passwordEncripter)
	{
		_writeOnlyRepository = writeOnlyRepository;
		_readOnlyRepository = readOnlyRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_passwordEncripter = passwordEncripter;
	}
	public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
	{
		await Validate(request);

		var user = _mapper.Map<Domain.Entities.User>(request);
		user.Password = _passwordEncripter.Encrypt(request.Password);

		await _writeOnlyRepository.Add(user);

		await _unitOfWork.Commit();

		return new ResponseRegisteredUserJson
		{
			Name = user.Name,
		};
	}

	private async Task Validate(RequestRegisterUserJson request)
	{
		var validator = new RegisterUserValidator();
		var result = validator.Validate(request);

		var emailExist = await _readOnlyRepository.ExistsActiveUsersWithEmail(request.Email);

		if (emailExist)
			result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTRED));

		if (!result.IsValid)
		{
			var errorMessages = result.Errors.Select(error => error.ErrorMessage);
			throw new ErrorOrValidationException(errorMessages.ToList());
		}
	}
}

