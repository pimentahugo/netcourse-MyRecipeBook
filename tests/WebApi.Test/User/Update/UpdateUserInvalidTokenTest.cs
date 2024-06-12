using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;
public class UpdateUserInvalidToken : MyRecipeBookClassFixture
{
	private const string METHOD = "user";

	public UpdateUserInvalidToken(CustomWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Error_Token_Invalid()
	{
		var request = RequestUpdateJsonBuilder.Build();

		var response = await DoPut(METHOD, request, token: "tokenInvalid");

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task Error_Without_Token()
	{
		var request = RequestUpdateJsonBuilder.Build();

		var response = await DoPut(METHOD, request, token: string.Empty);

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}
}

