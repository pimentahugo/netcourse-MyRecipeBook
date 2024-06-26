﻿using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;
public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for(var index = 0; index < instructions.Count;index++)
        {
            instructions.ElementAt(index).Step = index + 1;
        }

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }

    private void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if(!result.IsValid)
        {
            throw new ErrorOrValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}

