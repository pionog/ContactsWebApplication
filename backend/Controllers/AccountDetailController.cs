using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace ContactsWebApplication.Controllers
{
    //kontroler ktory umozliwia odpowiednie przekierowania na podstrony i wykorzystywanie prawidlowych metod takich jak GET czy POST


    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailController : ControllerBase
    {
        private readonly AccountDetailContext _context; //korzystanie z informacji o danych z konta
        private readonly IPasswordHasher<AccountDetail> _passwordHasher; //korzystanie z funkcji hashujacaej hasla
        private readonly ApplicationSettings _appSettings; //korzystanie z ustawien aplikacji (tajny klucz jwt)

        public AccountDetailController(AccountDetailContext context, IPasswordHasher<AccountDetail> passwordHasher, IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings.Value;
        }

        // GET: api/AccountDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDetail>>> GetAccountDetails()
        {
            return await _context.AccountDetails.ToListAsync();
        }

        // GET: api/AccountDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetail>> GetAccountDetail(int id)
        {
            var accountDetail = await _context.AccountDetails.FindAsync(id);

            if (accountDetail == null)
            {
                return NotFound();
            }

            return accountDetail;
        }

        // PUT: api/AccountDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountDetail(int id, AccountDetail accountDetail)
        {
            if (id != accountDetail.AccountID)
            {
                return BadRequest();
            }

            _context.Entry(accountDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccountDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<AccountDetail>> PostAccountDetail(AccountDetail accountDetail)
        {
            accountDetail.Password = _passwordHasher.HashPassword(accountDetail, accountDetail.Password); //ustawienie zahashowanego hasla do objektu konta
            _context.AccountDetails.Add(accountDetail); //dodawanie nowego konta do bazy danych
            await _context.SaveChangesAsync(); //zapisanie tego dodania

            return CreatedAtAction("GetAccountDetail", new { id = accountDetail.AccountID }, accountDetail); //zwrocenie informacji, jakie zapisal serwer
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            //szukanie uzytkownika po jego emailu
            var user = await _context.AccountDetails.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Brak użytkownika." });
            }

            //sprawdzanie hasha hasla
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest(new { message = "Niepoprawne hasło." });
            }

            //prawidlowy uzytkownik, tworzenie opisu tokenu uwierzytelniajacego

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                        new Claim("AccountID",user.AccountID.ToString()), //zawarcie informacji o id konta w tokenie
                        new Claim("Email", user.Email.ToString()) //zawarcie informacji o emailu w tokenie
                    ]),
                Expires = DateTime.UtcNow.AddDays(1), //ustawienie waznosci tokena na jeden dzien
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature) //wykorzystanie tajnego klucza z appsettings
            };
            var tokenHandler = new JwtSecurityTokenHandler(); //tworzenie zmiennej obslugujacej token
            var securityToken = tokenHandler.CreateToken(tokenDescriptor); //tworzenie wlasciwego tokena
            var token = tokenHandler.WriteToken(securityToken); //zmienna przechowujaca token w prawidlowym formacie
            return Ok(new { token });
        }

        // DELETE: api/AccountDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountDetail(int id)
        {
            var accountDetail = await _context.AccountDetails.FindAsync(id);
            if (accountDetail == null)
            {
                return NotFound();
            }

            _context.AccountDetails.Remove(accountDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountDetailExists(int id)
        {
            return _context.AccountDetails.Any(e => e.AccountID == id);
        }
    }
}
