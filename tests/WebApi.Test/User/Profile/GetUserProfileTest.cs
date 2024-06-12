﻿using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.User.Profile;
public class GetUserProfileTest : MyRecipeBookClassFixture
{
	private readonly string METHOD = "user";

	private readonly string _name;
	private readonly string _email;
	private readonly Guid _userIdentifier;

	public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
	{
		_name = factory.GetName();
		_email = factory.GetEmail();
		_userIdentifier = factory.GetUserIdentifier();
	}

	[Fact]
	public async Task Success()
	{
		var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

		var response = await DoGet(METHOD, token: token);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

		await using var responseBody = await response.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);
		responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name); //json sempre retorna propriedade em common case
		responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email); //json sempre retorna propriedade em common case
	}
}

