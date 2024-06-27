﻿
using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Repositories.Recipe;
public interface IRecipeReadOnlyRepository
{
	Task<IList<Domain.Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);
	Task<Domain.Entities.Recipe>? GetById(Entities.User user, long recipeId);
}

