using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access;
public abstract class JwtTokenHandler
{
	protected SymmetricSecurityKey SecurityKey(string signinKey)
	{
		var bytes = Encoding.UTF8.GetBytes(signinKey);

		return new SymmetricSecurityKey(bytes);
	}
}

