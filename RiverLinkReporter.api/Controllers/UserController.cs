using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using RiverLinkReporter.service;

namespace RiverLinkReporter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //private readonly IMapper _Mapper;
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly IUserService _UserService;
        private readonly ILogger<UserController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;

        private readonly RiverLinkReporter_JWTSettings _TokenOptions;
        //private NLog.ILogger _Logger => LogManager.GetLogger(this.GetType().FullName);

        public UserController(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            IUserService UserService,
            UserManager<IdentityUser> userManager,
            ILogger<UserController> logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService
        )
        {
            _Context = Context;
            //_Mapper = Mapper;
            _UserService = UserService;
            _UserManager = userManager;
            _Settings = Settings.Value;
            _Cache = MemoryCache ?? new MemoryCache(new MemoryCacheOptions());
            _Logger = logger;
            _emailService = emailService;
            _TokenOptions = TokenOptions.Value;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Register")]
        [HttpPost]
        [Route("api/v1/Register", Name = "Register")]
        public async Task<IActionResult> Register(string Email, string Password)
        {
            _Logger.LogInformation($"Registering user {Email}...");
            IdentityResult returnValue = null;

            #region Validate Parameters

            if (string.IsNullOrEmpty(Email))
                ModelState.AddModelError($"Register Error", $"The {nameof(Email)} cannot be zero");

            if (!ModelState.IsValid)
            {
                LogInvalidState();
                return BadRequest(ModelState);
            }

            _Logger.LogDebug($"ModelState is valid.");

            #endregion Validate Parameters

            try
            {
                returnValue = await _UserService.Register(Email, Password);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Register Unexpected Error: {ex}");
                return StatusCode(500, $"Register Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Register complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Login")]
        [HttpPost]
        [Route("api/v1/Login", Name = "Login")]
        public async Task<IActionResult> Login(string Email, string Password, bool RemeberMe)
        {
            _Logger.LogInformation($"Login for user: {Email}...");
            string returnValue = null;

            #region Validate Parameters

            if (string.IsNullOrEmpty(Email))
                ModelState.AddModelError($"Login Error", $"The {nameof(Email)} cannot be empty");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError($"Login Error", $"The {nameof(Password)} cannot be empty");

            if (!ModelState.IsValid)
            {
                LogInvalidState();
                return BadRequest(ModelState);
            }

            _Logger.LogDebug($"ModelState is valid.");

            #endregion Validate Parameters

            try
            {
                returnValue = await _UserService.Login(Email, Password, RemeberMe);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Login Unexpected Error: {ex}");
                return StatusCode(500, $"Login Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Login complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "ForgotPassword")]
        [HttpPost]
        [Route("api/v1/ForgotPassword", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            _Logger.LogInformation($"Send email to reset password for user: {Email}...");
            IdentityUser returnValue = null;

            #region Validate Parameters

            if (string.IsNullOrEmpty(Email))
                ModelState.AddModelError($"Password Retrieval Error", $"The {nameof(Email)} cannot be empty");

            if (!ModelState.IsValid)
            {
                LogInvalidState();
                return BadRequest(ModelState);
            }

            _Logger.LogDebug($"ModelState is valid.");

            #endregion Validate Parameters

            try
            {
                returnValue = await _UserService.ForgotPassword(Email);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"ForgotPassword Unexpected Error: {ex}");
                return StatusCode(500, $"ForgotPassword Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Password Reset Email Sent.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "ResetPassword")]
        [HttpPost]
        [Route("api/v1/ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            _Logger.LogInformation($"Reset password for user: {Email}...");
            string returnValue = null;

            #region Validate Parameters

            if (string.IsNullOrEmpty(Email))
                ModelState.AddModelError($"Password reset Error", $"The {nameof(Email)} cannot be empty");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError($"Password reset Error", $"The {nameof(Password)} cannot be empty");

            if (string.IsNullOrEmpty(ConfirmPassword))
                ModelState.AddModelError($"Password reset Error", $"The {nameof(ConfirmPassword)} cannot be empty");

            if (!ModelState.IsValid)
            {
                LogInvalidState();
                return BadRequest(ModelState);
            }

            _Logger.LogDebug($"ModelState is valid.");

            #endregion Validate Parameters

            try
            {
                returnValue = await _UserService.ResetPassword(Email, Password, ConfirmPassword);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"ResetPassword Unexpected Error: {ex}");
                return StatusCode(500, $"ResetPassword Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Password Reset.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Logout")]
        [HttpPost]
        [Route("api/v1/Logout", Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            _Logger.LogInformation($"Logging out current user");
            string returnValue = null;
            try
            {
                returnValue = await _UserService.Logout();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Logout Unexpected Error: {ex}");
                return StatusCode(500, $"Logout Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Logout complete.");
            return Ok(returnValue);
        }

        private void LogInvalidState()
        {
            var errors = "";

            foreach (var modelState in ViewData.ModelState.Values)
            foreach (var error in modelState.Errors)
                errors += error + " ";
            _Logger.LogError($"Invalid ModelState: {errors}");
        }
    }
}