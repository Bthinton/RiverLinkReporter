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
using RiverLinkReporter.service.Data;
using RiverLinkReporter.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly ITransactionService _TransactionService;
        private readonly ILogger<TransactionController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;
        private readonly RiverLinkReporter_JWTSettings _TokenOptions;

        public TransactionController(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            ITransactionService TransactionService,
            UserManager<IdentityUser> userManager,
            ILogger<TransactionController> logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService  )
        {
            _Context = Context;
            //_Mapper = Mapper;
            _TransactionService = TransactionService;
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
        [Route("api/v1/Transactions", Name = "GetAllTransactions")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Transaction> returnValue = null;
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
                returnValue = await _TransactionService.GetAll();
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
        [Route("api/v1/Transaction", Name = "AddTransaction")]
        public async Task<IActionResult> Add(Transaction transaction)
        {
            Transaction returnValue = null;

            try
            {
                returnValue = await _TransactionService.Add(transaction);
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
        [Route("api/v1/Transaction/{id}", Name = "DeleteTransaction")]
        public async Task<IActionResult> Delete(int id)
        {
            int returnValue;

            try
            {
                returnValue = await _TransactionService.Delete(id);
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

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Read")]
        [HttpGet]
        [Route("api/v1/Transaction/{id}", Name = "GetTransactionById")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            Transaction returnValue = null;
            
            try
            {
                returnValue = _Context.Transactions.Find(id);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Read Unexpected Error: {ex}");
                return StatusCode(500, $"Read Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Read complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Update")]
        [HttpPut]
        [Route("api/v1/Transaction", Name = "UpdateTransaction")]
        public async Task<IActionResult> Update(Transaction transaction)
        {
            Transaction returnValue = null;

            try
            {
                returnValue = await _TransactionService.Update(transaction);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Update Unexpected Error: {ex}");
                return StatusCode(500, $"Update Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"Update complete.");
            return Ok(returnValue);
        }
    }
}