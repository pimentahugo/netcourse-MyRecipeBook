namespace MyRecipeBook.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
	public Task<bool> ExistsActiveUsersWithEmail(string email);
}
