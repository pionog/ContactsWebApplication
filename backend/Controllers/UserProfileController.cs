﻿using ContactsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserProfileController(IAuthService authService)
        {
            _authService = authService;
        }
        [Authorize]
        [HttpGet]
        [Route("GetAccountDetail")]
        public async Task<Object> GetUserProfile()
        {
            var email = User.Claims.First(c => c.Type == "Email").Value;
            var accountDetail = await _authService.GetAccountDetail(email);
            return Ok(accountDetail);
        }
    }
}
