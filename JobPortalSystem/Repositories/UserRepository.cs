using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Auth;
using JobPortalSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<User?> GetUserById(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task CreateUser(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
