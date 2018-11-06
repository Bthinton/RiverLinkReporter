using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.service;
using RiverLinkReporter.service.Data;
using RiverLinkReporter.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransponderController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly ITransponderService _TransponderService;
        private readonly ILogger<TransponderController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;
        private readonly RiverLinkReporter_JWTSettings _TokenOptions;

        public TransponderController(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            ITransponderService TransponderService,
            UserManager<IdentityUser> userManager,
            ILogger<TransponderController> logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService)
        {
            _Context = Context;
            //_Mapper = Mapper;
            _TransponderService = TransponderService;
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
        [Route("api/v1/Vehicles", Name = "GetAll")]
        public async Task<IActionResult> GetAll()
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
                returnValue = await _TransponderService.GetAll();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAll Unexpected Error: {ex}");
                return StatusCode(500, $"GetAll Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Register complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Add")]
        [HttpPost]
        [Route("api/v1/Vehicle/Add", Name = "Add")]
        public async Task<IActionResult> Add(Transponder transponder)
        {
            Transponder returnValue = null;

            try
            {
                returnValue = await _TransponderService.Add(transponder);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Add Unexpected Error: {ex}");
                return StatusCode(500, $"Add Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Add complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Delete")]
        [HttpDelete]
        [Route("api/v1/Transponder/Delete", Name = "Delete")]
        public async Task<IActionResult> Delete(Transponder transponder)
        {
            Transponder returnValue = null;

            try
            {
                returnValue = await _TransponderService.Delete(transponder);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Delete Unexpected Error: {ex}");
                return StatusCode(500, $"Delete Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Delete complete.");
            return Ok(returnValue);
        }
    }
}