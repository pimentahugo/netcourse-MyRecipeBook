﻿using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using FluentAssertions;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe;
public class RecipeValidatorTest
{
	[Fact]
	public void Success()
	{
		var request = RequestRecipeJsonBuilder.Build();

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Error_Invalid_Difficulty()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Difficulty = (Difficulty)1000;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
	}

	[Theory]
	[InlineData("    ")]
	[InlineData("")]
	[InlineData(null)]
	public void Error_Empty_Title(string title)
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Title = title;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
	}

	[Fact]
	public void Success_Cooking_Time_Null()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.CookingTime = null;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Success_Difficulty_Null()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Difficulty = null;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Success_DishTypes_Empty()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.DishTypes.Clear();

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Error_Invalid_DishTypes()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.DishTypes.Add((DishType)1000);

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
	}

	[Fact]
	public void Error_Empty_Ingredients()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Ingredients.Clear();

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
	}

	[Fact]
	public void Error_Empty_Instructions()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Instructions.Clear();

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
	}

	[Theory]
	[InlineData("    ")]
	[InlineData("")]
	[InlineData(null)]
	public void Error_Empty_Value_Ingredients(string ingredient)
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Ingredients.Add(ingredient);

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
	}

	[Fact]
	public void Error_Same_Step_Instructions()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Instructions.First().Step = request.Instructions.Last().Step;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
	}

	[Fact]
	public void Error_Negative_Step_Instructions()
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Instructions.First().Step = -1;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
	}

	[Theory]
	[InlineData("    ")]
	[InlineData("")]
	[InlineData(null)]
	public void Error_Empty_Value_Instruction(string instruction)
	{
		var request = RequestRecipeJsonBuilder.Build();
		request.Instructions.First().Text = instruction;

		var validator = new RecipeValidator();
		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
	}
}