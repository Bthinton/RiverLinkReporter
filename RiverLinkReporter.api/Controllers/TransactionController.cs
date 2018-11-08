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
using System.Threading.Tasks;

namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IRepository _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly ITransactionService _TransactionService;
        private readonly ILogger<TransactionController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;
        private readonly RiverLinkReporter_JWTSettings _TokenOptions;

        public TransactionController(
            IRepository Context,
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
        public async Task<IActionResult> GetAllTransactions()
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
                returnValue = _Context.GetAll<Transaction>();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAllTransactions Unexpected Error: {ex}");
                return StatusCode(500, $"GetAllTransactions Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetAllTransactions complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Add")]
        [HttpPost]
        [Route("api/v1/Transaction", Name = "AddTransaction")]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            Transaction returnValue = transaction;

            try
            {
                _Context.Create(transaction);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"AddTransaction Unexpected Error: {ex}");
                return StatusCode(500, $"AddTransaction Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"AddTransaction complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Delete")]
        [HttpDelete]
        [Route("api/v1/Transaction/{id}", Name = "DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            int returnValue = id;

            try
            {
                _Context.Delete<Transaction>(id);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"DeleteTransaction Unexpected Error: {ex}");
                return StatusCode(500, $"DeleteTransaction Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"DeleteTransaction complete.");
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
                returnValue = _Context.GetById<Transaction>(id);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetTransactionById Unexpected Error: {ex}");
                return StatusCode(500, $"GetTransactionById Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetTransactionById complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Update")]
        [HttpPut]
        [Route("api/v1/Transaction", Name = "UpdateTransaction")]
        public async Task<IActionResult> UpdateTransaction(Transaction transaction)
        {
            Transaction returnValue = transaction;

            try
            {
                _Context.Update(transaction);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"UpdateTransaction Unexpected Error: {ex}");
                return StatusCode(500, $"UpdateTransaction Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"UpdateTransaction complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "GetAllByUserId")]
        [HttpGet]
        [Route("api/v1/Vehicles/{userId}", Name = "GetAllVehiclesByUserId")]
        public async Task<IActionResult> GetAllVehiclesByUserId(Guid userId)
        {
            IEnumerable<Vehicle> returnValue = null;
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
                returnValue = _Context.Get<Vehicle>(VehicleController.FilterByUserId(userId), orderBy: q => q.OrderBy(x => x.Transponder));
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAllVehiclesByUserId Unexpected Error: {ex}");
                return StatusCode(500, $"GetAllVehiclesByUserId Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetAllVehiclesByUserId complete.");
            return Ok(returnValue);
        }
    }
}