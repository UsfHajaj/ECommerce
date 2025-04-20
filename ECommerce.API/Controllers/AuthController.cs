using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,IJwtService jwtService,IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userDTO.UserName;
                user.Email = userDTO.Email;

                IdentityResult result = await _userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok(new { message = "Account created successfully with role." });
                }
                return BadRequest(result.Errors.FirstOrDefault()!.Description.ToString());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(loginDTO.Email);

                if(user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                    if (result)
                    {
                        var token = _jwtService.GenerateJwtToken(user);
                        return Ok(new 
                        { 
                            token= token,
                            expiration = DateTime.UtcNow.AddDays(1),
                        });
                    }
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Email confirmation failed" });
            }
            var confirmtionLinkForFront= $"http://localhost:4200/auth/confirm-email?userId={userId}&token={token}";
            return Ok(new { message = "Email confirmed successfully" });
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

            // Assume a method for sending email exists
            //await SendEmailAsync(user.Email, "Confirm your email", confirmationLink);

            return Ok(token);
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Status = "Error", Message = "Invalid password reset link." });
            }

            // Return success response for valid tokens
            //return Ok(new { Status = "Success", Message = "Password reset link is valid.", Token = token, Email = email }); 
            return Redirect($"http://localhost:4200/auth/reset-password?token={token}&email={email}");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null) return NotFound("User not found");
                var decodeToken = WebUtility.UrlDecode(resetPasswordDto.Token);
                var result = await _userManager.ResetPasswordAsync(user, decodeToken, resetPasswordDto.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Password reset successfully" });
                }
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            if (model.CurrentPassword == null || model.NewPassword == null)
            {
                return BadRequest(new { message = "Current password and new password are required" });
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if(!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            return Ok(new { message = "Password changed successfully" });
        }
    }
}
