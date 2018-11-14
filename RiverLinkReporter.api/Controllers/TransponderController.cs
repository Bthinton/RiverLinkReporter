using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransponderController : ControllerBase
    {
        private readonly IRepository _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly ILogger<TransponderController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;
        private readonly RiverLinkReporter_JWTSettings _TokenOptions;

        public TransponderController(
            IRepository Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> userManager,
            ILogger<TransponderController> logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService)
        {
            _Context = Context;
            //_Mapper = Mapper;
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
        [SwaggerOperation(OperationId = "GetAll")]
        [HttpGet]
        [Route("api/v1/Transponders", Name = "GetAllTransponders")]
        public async Task<IActionResult> GetAllTransponders()
        {
            IEnumerable<Transponder> returnValue = null;
            //#region Validate Parameters

            //if (string.IsNullOrEmpty(Email))
            //    ModelState.AddModelError($"Register Error", $"The {nameof(Email)} cannot be zero");

            //if (!ModelState.IsValid)
            //{
            //    LogInvalidState();
            //    return BadRequest(ModelState);
            //}

            //_Logger.LogDebug($"ModelState is valid.");

            //#endregion Validate Parameters

            try
            {
                returnValue = _Context.GetAll<Transponder>();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAllTransponders Unexpected Error: {ex}");
                return StatusCode(500, $"GetAllTransponders Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetAllTransponders complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Add")]
        [HttpPost]
        [Route("api/v1/Transponder", Name = "AddTransponder")]
        public async Task<IActionResult> AddTransponder(Transponder transponder)
        {
            Transponder returnValue = transponder;

            try
            {
                _Context.Create(transponder);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"AddTransponder Unexpected Error: {ex}");
                return StatusCode(500, $"AddTransponder Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"AddTransponder complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Delete")]
        [HttpDelete]
        [Route("api/v1/Transponder/{id}", Name = "DeleteTransponderById")]
        public async Task<IActionResult> DeleteTransponderById(int id)
        {
            int returnValue = id;

            try
            {
                _Context.Delete<Transponder>(id);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"DeleteTransponderById Unexpected Error: {ex}");
                return StatusCode(500, $"DeleteTransponderById Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"DeleteTransponderById complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "GetByID")]
        [HttpGet]
        [Route("api/v1/Transponder/{id}", Name = "GetTransponderById")]
        public async Task<IActionResult> GetTransponderById(int id)
        {
            Transponder returnValue = null;

            try
            {
                returnValue = _Context.GetById<Transponder>(id);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetTransponderById Unexpected Error: {ex}");
                return StatusCode(500, $"GetTransponderById Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetTransponderById complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Update")]
        [HttpPut]
        [Route("api/v1/Transponder", Name = "UpdateTransponder")]
        public async Task<IActionResult> UpdateTransponder(Transponder transponder)
        {
            Transponder returnValue = transponder;

            try
            {
                _Context.Update(transponder);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"UpdateTransponder Unexpected Error: {ex}");
                return StatusCode(500, $"UpdateTransponder Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"UpdateTransponder complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "GetAllByUserId")]
        [HttpGet]
        [Route("api/v1/Transponders/{userId}", Name = "GetAllTranspondersByUserId")]
        public async Task<IActionResult> GetAllTranspondersByUserId(Guid userId)
        {
            IEnumerable<Transponder> returnValue = null;
            //#region Validate Parameters

            //if (string.IsNullOrEmpty(Email))
            //    ModelState.AddModelError($"Register Error", $"The {nameof(Email)} cannot be zero");

            //if (!ModelState.IsValid)
            //{
            //    LogInvalidState();
            //    return BadRequest(ModelState);
            //}

            //_Logger.LogDebug($"ModelState is valid.");

            //#endregion Validate Parameters

            try
            {
                returnValue = _Context.Get<Transponder>(FilterByUserId(userId), orderBy: q => q.OrderBy(x => x.TransponderNumber));
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAllTranspondersByUserId Unexpected Error: {ex}");
                return StatusCode(500, $"GetAllTranspondersByUserId Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetAllTranspondersByUserId complete.");
            return Ok(returnValue);
        }

        public static Expression<Func<Transponder, bool>> FilterByUserId(Guid userId)
        {
            return x => x.UserId == userId;
        }
    }
}