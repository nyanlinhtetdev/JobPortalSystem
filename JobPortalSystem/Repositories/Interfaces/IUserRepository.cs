using JobPortal.Database.AppDbContextModels;

namespace JobPortalSystem.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid userId);
        Task CreateUser(User newUser);
    }
}