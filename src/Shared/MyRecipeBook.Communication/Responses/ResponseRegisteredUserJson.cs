namespace MyRecipeBook.Communication.Responses;
public class ResponseRegisteredUserJson
{
    public ResponseTokensJson Tokens { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
}

