using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;

namespace RiverLinkReporter.Service
{
    public interface IUserService
    {
        Task<IdentityResult> Register(string Email, string Password);

        Task<string> Login(string Email, string Password, bool RememberMe);

        Task<IdentityUser> ForgotPassword(string Email);

        Task<string> ResetPassword(string Email, string Password, string ConfirmPassword);

        Task<string> Logout();
    }

    public class UserService : IUserService
    {
        //private readonly IMapper _Mapper;
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private ILogger<UserService> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly IEmailService _emailService;
        private readonly IRiverLinkReporter_JWTSettings _TokenOptions;


        private ModelStateDictionary _modelState;

        public UserService(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> UserManager,
            SignInManager<IdentityUser> SignInManager,
            ILogger<UserService> Logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService
        )
        {
            _Context = Context;
            //_Mapper = Mapper;
            _Settings = Settings.Value;
            _Cache = MemoryCache ?? new MemoryCache(new MemoryCacheOptions());
            _Logger = Logger;
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _emailService = emailService;
            this._TokenOptions = TokenOptions.Value;
        }

        protected bool ValidateUser(string Email, string Password)
        {
            if (Email.Trim().Length == 0)
                throw new Exception("Email is required.");
            if (Password.Trim().Length == 0)
                throw new Exception("Password is required.");
            return true;
        }

        private string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_TokenOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _TokenOptions.Issuer,
                _TokenOptions.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Login(string Email, string Password, bool RememberMe)
        {
            string returnvalue = null;
            if (ValidateUser(Email, Password))
            {
                var user = _UserManager.FindByEmailAsync(Email);
                if (user != null && !user.Result.EmailConfirmed)
                {
                    _Logger.LogInformation("Email is not confirmed");
                    return returnvalue;
                }

                var result =
                    await _SignInManager.PasswordSignInAsync(Email, Password, RememberMe, _Settings.LockoutOnFailure);
                if (result.Succeeded)
                {
                    _Logger.LogInformation("User logged in.");
                    return GenerateToken(Email);
                }

                if (result.RequiresTwoFactor) _Logger.LogInformation("User requires 2FA.");
                if (result.IsLockedOut)
                {
                    _Logger.LogWarning("User account locked out.");
                }
                else
                {
                    var errorMessage = $"Login failed. Please check your username and password and try again.";
                    throw new Exception(errorMessage);
                }
            }

            return returnvalue;
        }

        public async Task<IdentityResult> Register(string Email, string Password)
        {
            IdentityResult returnvalue = null;

            if (ValidateUser(Email, Password))
            {
                var user = new IdentityUser {UserName = Email, Email = Email};
                returnvalue = await _UserManager.CreateAsync(user, Password);
                if (returnvalue.Succeeded)
                {
                    _Logger.LogInformation("User created a new account with password.");

                    var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

                    //build the url
                    var callbackUrl = $"{_Settings.ConfirmEmailUrl}?userId={user.Id}&code={code}";

                    await _emailService.SendEmailAsync(Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                }
                else
                {
                    var errorMessage = $"Registration Error: {Environment.NewLine}";

                    foreach (var error in returnvalue.Errors) errorMessage += error.Description + Environment.NewLine;

                    throw new Exception(errorMessage);
                }

                return returnvalue;
            }

            throw new Exception("Error: Model state is invalid, username and password are required.");
        }

        public async Task<IdentityUser> ForgotPassword(string Email)
        {
            IdentityUser returnvalue = null;
            if (Email != null)
            {
                returnvalue = await _UserManager.FindByEmailAsync(Email);
                if (!returnvalue.EmailConfirmed) return returnvalue;
                var code = await _UserManager.GeneratePasswordResetTokenAsync(returnvalue);

                var callbackUrl = $"{_Settings.ForgotPasswordUrl}?userId={returnvalue.Id}&code={code}";

                await _emailService.SendEmailAsync(Email, "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                _Logger.LogInformation("Reset Password Email Sent.");
            }
            else
            {
                var errorMessage = $"Password Reset Email Error: {Environment.NewLine}";

                throw new Exception(errorMessage);
            }

            return returnvalue;
        }

        public async Task<string> ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            string returnvalue = null;
            if (ValidateUser(Email, Password))
            {
                var user = await _UserManager.FindByEmailAsync(Email);
                var code = await _UserManager.GeneratePasswordResetTokenAsync(user);

                var result = await _UserManager.ResetPasswordAsync(user, code, Password);
                if (result.Succeeded) _Logger.LogInformation("User password reset.");
            }
            else
            {
                var errorMessage = $"Password Reset Error: {Environment.NewLine}";

                throw new Exception(errorMessage);
            }

            return returnvalue;
        }

        public async Task<string> Logout()
        {
            string returnvalue = null;
            await _SignInManager.SignOutAsync();
            _Logger.LogInformation("User logged out.");
            return returnvalue;
        }
    }
}