using ContactsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase //kontroler umozliwiajacy uzyskiwanie danych z serwera na temat konta uzytkownika
    {
        private readonly IAuthService _authService; //serwis autoryzacji

        public UserProfileController(IAuthService authService)
        {
            _authService = authService;
        }
        [Authorize]
        [HttpGet]
        [Route("GetAccountDetail")]
        public async Task<Object> GetUserProfile()
        {
            var email = User.Claims.First(c => c.Type == "Email").Value; //szukanie uzytkownika po emailu
            var accountDetail = await _authService.GetAccountDetail(email); //uzyskiwanie danych o koncie z serwisu autoryzujacego
            return Ok(accountDetail);
        }
    }
}
