using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;
public class RegisterUserTest : MyRecipeBookClassFixture
{
	private readonly string method = "user";

	public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) {}

	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var response = await DoPost(method: method, request: request);

		response.StatusCode.Should().Be(HttpStatusCode.Created);

		await using var responseBody = await response.Content.ReadAsStreamAsync(); //Lendo a resposta

		var responseData = await JsonDocument.ParseAsync(responseBody); //Passando para JSON

		responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name); //json sempre retorna propriedade em common case
		responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
	}

	[Theory]
	[ClassData(typeof(CultureInlineDataTest))]
	public async Task Error_Empty_Name(string culture)
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		var response = await DoPost(method: method, request: request, culture: culture);

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
