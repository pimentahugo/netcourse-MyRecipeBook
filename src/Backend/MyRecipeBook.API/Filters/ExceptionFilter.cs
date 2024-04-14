﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace MyRecipeBook.API.Filters;
public class ExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		if (context.Exception is MyRecipeBookException)
		{
			HandleProjectException(context);
		}
		else
		{
			ThrowUnkownException(context);
		}
	}

	private void HandleProjectException(ExceptionContext context)
	{
		if (context.Exception is ErrorOrValidationException)
		{
			var exception = context.Exception as ErrorOrValidationException;

			context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
		}
	}
	private void ThrowUnkownException(ExceptionContext context)
	{
		context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKOWN_ERROR));
	}
}