using Application.Dtos.ApplicationUser;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfiniTech.Controllers.v1
{
    [ApiController]
    [Route("api/UserAuth")]
    [ApiVersion("1.0")]
    public class ApplicationUserController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        // POST : /api/UserAuthController/Register
        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostAppUser(AppUserForCreationDto user)
        {
            var appUser = new ApplicationUser()
            {
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                UserName = user.Email
            };
            try
            {
                var result = await userManager.CreateAsync(appUser, user.Password);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(AppUserLoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var TokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(3),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:JWTSecrets")))
                        , SecurityAlgorithms.HmacSha256Signature
                    ),
                };
                var TokenHandler = new JwtSecurityTokenHandler();
                var securitytoken = TokenHandler.CreateToken(TokenDescriptor);
                var token = TokenHandler.WriteToken(securitytoken);
                return Ok(new { token, user.Id });
            }
            else
                return BadRequest(new { message = "UserName Or Password is Incorrect" });
        }
        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(AppUserPasswordChangeDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var currentuserid = User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            if (user.Id != currentuserid)
                return Unauthorized();

            if (user is not null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var res = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                if (res.Succeeded)
                    return Ok();
                else
                    return BadRequest();
            }

            return BadRequest();
        }
    }
}
