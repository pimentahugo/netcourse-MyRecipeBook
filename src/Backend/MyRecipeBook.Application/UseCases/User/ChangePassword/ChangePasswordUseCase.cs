using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly IUserUpdateOnlyRepository _repository;
	private readonly IPasswordEncripter _passwordEncripter;
	private readonly IUnitOfWork _unitOfWork;

	public ChangePasswordUseCase(
		ILoggedUser loggedUser,
		IPasswordEncripter passwordEncripter,
		IUserUpdateOnlyRepository repository,
		IUnitOfWork unitOfWork)
	{
		_loggedUser = loggedUser;
		_repository = repository;
		_passwordEncripter = passwordEncripter;
		_unitOfWork = unitOfWork;
	}
	public async Task Execute(RequestChangePasswordJson request)
	{
		var loggedUser = await _loggedUser.User();

		Validate(request, loggedUser);

		var user = await _repository.GetById(loggedUser.Id);

		user.Password = _passwordEncripter.Encrypt(request.NewPassword);

		_repository.Update(user);

		await _unitOfWork.Commit();
	}

	private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
	{
		var result = new ChangePasswordValidator().Validate(request);

		var currentPasswordEncripted = _passwordEncripter.Encrypt(request.Password);

		if(!currentPasswordEncripted.Equals(loggedUser.Password))
		{
			result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
		}

		if(!result.IsValid)
		{
			throw new ErrorOrValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
		}
	}
}

