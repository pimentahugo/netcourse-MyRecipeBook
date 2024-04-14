﻿using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestedRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty();
        RuleFor(user => user.Email).EmailAddress();
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6);
    }
}

