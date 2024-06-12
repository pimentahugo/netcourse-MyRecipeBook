﻿using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : MyRecipeBookClassFixture
{
	private readonly string method = "login";

    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPasword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
		responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
	}

	[Theory]
	[ClassData(typeof(CultureInlineDataTest))]
	public async Task Error_Empty_Name(string culture)
	{
		var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(method, request, culture);

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var responseBody = await response.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();

		var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

		//apenas o nome vazio, entao apenas 1 mensagem de erro
		//verifica se o nome do erro esta na linguagem passando pelo culture
		erros.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

	}
}