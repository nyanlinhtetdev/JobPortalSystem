using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.Auth;
using JobPortalSystem.Api.Repositories.Interfaces;
using JobPortalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JobPortalSystem.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private readonly JwtOptions _jwtOptions;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IOptions<JwtOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<ApiResponse<object>> Register(RegisterRequestDto request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user != null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "User has already existed."
                };
            }

            var newUser = new User
            {
                Email = request.Email,
                Role = request.Role
            };

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(newUser, request.Password);

            newUser.PasswordHash = hashedPassword;

            await _userRepository.CreateUser(newUser);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "User has successfully registered."
            };
        }

        public async Task<TokenResponseDto?> Login(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if(user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<Guid?> ValidateRefreshToken(string refreshToken)
        {
            Guid userId;
            var token = await _refreshTokenRepository.GetRefreshToken(refreshToken);
            if(token is null || token.IsRevoked == true || token.ExpiryDate < DateTime.UtcNow)
            {
                return null;  
            }
            userId = token.UserId;

            await _refreshTokenRepository.RevokedToken(refreshToken);

            return userId;
        }

        public async Task<TokenResponseDto?> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await ValidateRefreshToken(request.RefreshToken);
            if(result is null)
            {
                return null;
            }
            Guid userId = result.Value;

            var user = await _userRepository.GetUserById(userId);
            if (user is null)
            {
                return null;
            }
            return new TokenResponseDto {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();

            var token = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepository.CreateRefreshToken(token);

            return refreshToken;
        }

        public async Task Logout(Guid userId)
        {
            var tokens = await _refreshTokenRepository.GetByUserIdAsync(userId);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }
            await _refreshTokenRepository.SaveChangesAsync();
        }
    }
}
