namespace MyRecipeBook.Communication.Requests;
public class RequestInstructionJson
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
    public long RecipeId { get; set; }
}