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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailController : ControllerBase
    {
        private readonly AccountDetailContext _context;
        private readonly IPasswordHasher<AccountDetail> _passwordHasher;
        private readonly ApplicationSettings _appSettings;

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
            accountDetail.Password = _passwordHasher.HashPassword(accountDetail, accountDetail.Password);
            _context.AccountDetails.Add(accountDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountDetail", new { id = accountDetail.AccountID }, accountDetail);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            // find user
            var user = await _context.AccountDetails.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Brak użytkownika." });
            }

            // user was found. check the password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest(new { message = "Niepoprawne hasło." });
            }

            // password was correct. you have user's details

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                        new Claim("AccountID",user.AccountID.ToString())
                    ]),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new { token });

            return Ok(new { message = "Pomyślne logowanie." });
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
