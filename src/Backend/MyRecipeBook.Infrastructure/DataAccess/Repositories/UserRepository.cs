﻿using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
	private readonly MyRecipeBookDbContext _dbContext;

	public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

	public async Task<bool> ExistsActiveUsersWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

	public async Task<User?> GetByEmailAndPassword(string email, string password)
	{
		return await _dbContext
			.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password) && user.Active);
	}
}

