namespace MyRecipeBook.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
	public Task<bool> ExistsActiveUsersWithEmail(string email);
	public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
	public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
}
