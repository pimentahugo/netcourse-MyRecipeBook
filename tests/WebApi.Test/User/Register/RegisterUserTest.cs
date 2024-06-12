using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;
public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _httpClient;
	public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var response = await _httpClient.PostAsJsonAsync("User", request);

		response.StatusCode.Should().Be(HttpStatusCode.Created);

		await using var responseBody = await response.Content.ReadAsStreamAsync(); //Lendo a resposta

		var responseData = await JsonDocument.ParseAsync(responseBody); //Passando para JSON

		responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name); //json sempre retorna propriedade em common case
	}

	[Theory]
	[ClassData(typeof(CultureInlineDataTest))]
	public async Task Error_Empty_Name(string culture)
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		if(_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
		{
			_httpClient.DefaultRequestHeaders.Remove("Accept-Language");
		}

		_httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

		var response = await _httpClient.PostAsJsonAsync("User", request);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responseBody = await response.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();

		var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

		//apenas o nome vazio, entao apenas 1 mensagem de erro
		//verifica se o nome do erro esta na linguagem passando pelo culture
		erros.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

	}
}
