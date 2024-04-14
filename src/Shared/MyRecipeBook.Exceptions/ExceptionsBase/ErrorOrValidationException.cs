namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class ErrorOrValidationException : MyRecipeBookException
{
	public IList<string> ErrorMessages { get; set; }

    public ErrorOrValidationException(IList<string> erros)
    {
        ErrorMessages = erros;
    }
}
