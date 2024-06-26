using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ContactsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

public interface IAuthService
{
    Task<string> Authenticate(string email, string password);
    Task<AccountDetail> GetAccountDetail(string email);
}

public class AuthService : IAuthService
{
    private readonly AccountDetailContext _context; //odwolanie do bazy danych
    private readonly ApplicationSettings _appSettings; //korzystanie z ustawien aplikacji
    private readonly IPasswordHasher<AccountDetail> _passwordHasher; //korzystanie z hashowania hasel

    public AuthService(AccountDetailContext context, IOptions<ApplicationSettings> appSettings, IPasswordHasher<AccountDetail> passwordHasher)
    {
        _context = context;
        _appSettings = appSettings.Value;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> Authenticate(string email, string password)
    {
        var user = await _context.AccountDetails.FirstOrDefaultAsync(u => u.Email == email); //szukanie uzytkownika po emailu

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        //tworzenie tokena jwt
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JWT_Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.AccountID.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<AccountDetail> GetAccountDetail(string email)
    {
        return await _context.AccountDetails.FirstOrDefaultAsync(u => u.Email == email); //szukanie w bazie danych uzytkownika po emailu
    }
}
