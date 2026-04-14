using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Api.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateRefreshToken(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshToken);
        }
        public async Task RevokedToken(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshToken);
            if(token != null)
            {
                token.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}
