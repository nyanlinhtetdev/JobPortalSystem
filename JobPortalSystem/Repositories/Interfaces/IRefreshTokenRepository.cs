using JobPortal.Database.AppDbContextModels;

namespace JobPortalSystem.Api.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateRefreshToken(RefreshToken token);
        Task<RefreshToken?> GetRefreshToken(string refreshToken);
        Task RevokedToken(string refreshToken);
    }
}