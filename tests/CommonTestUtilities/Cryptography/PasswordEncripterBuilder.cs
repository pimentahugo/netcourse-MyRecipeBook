using MyRecipeBook.Application.Services.Crytography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
	public static PasswordEncripter Build() => new PasswordEncripter("abcd1234");
}

