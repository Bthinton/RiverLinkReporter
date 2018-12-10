using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReporter.models;
using RiverLinkReporter.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

//TODO Work queue

namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IRepository _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private readonly ILogger<VehicleController> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IEmailService _emailService;
        private readonly RiverLinkReporter_JWTSettings _TokenOptions;

        public VehicleController(
            IRepository Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> userManager,
            ILogger<VehicleController> logger,
            IOptions<RiverLinkReporter_JWTSettings> TokenOptions,
            IEmailService emailService  )
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
        [Route("api/v1/Vehicles", Name = "GetAllVehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            IEnumerable<Vehicle> returnValue = null;

            try
            {
                returnValue = _Context.GetAll<Vehicle>();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetAllVehicles Unexpected Error: {ex}");
                return StatusCode(500, $"GetAllVehicles Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetAllVehicles complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Add")]
        [HttpPost]
        [Route("api/v1/Vehicle", Name = "AddVehicle")]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            Vehicle returnValue = vehicle;

            try
            {
                _Context.Create(vehicle);
                _Context.SaveAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"AddVehicle Unexpected Error: {ex}");
                return StatusCode(500, $"AddVehicle Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"AddVehicle complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "DeleteById")]
        [HttpDelete]
        [Route("api/v1/Vehicle/{id}", Name = "DeleteVehicleById")]
        public async Task<IActionResult> DeleteVehicleById(int id)
        {
            int returnValue;

            try
            {
                _Context.Delete<Vehicle>(id);
                _Context.SaveAsync();
                returnValue = id;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"DeleteVehicleById Unexpected Error: {ex}");
                return StatusCode(500, $"DeleteVehicleById Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"DeleteVehicleById complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "GetById")]
        [HttpGet]
        [Route("api/v1/Vehicle/{id}", Name = "GetVehicleById")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            Vehicle returnValue = null;

            try
            {
                returnValue = _Context.GetById<Vehicle>(id);               
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetVehicleById Unexpected Error: {ex}");
                return StatusCode(500, $"GetVehicleById Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"GetVehicleById complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "Update")]
        [HttpPut]
        [Route("api/v1/Vehicle", Name = "UpdateVehicle")]
        public async Task<IActionResult> UpdateVehicle(Vehicle vehicle)
        {
            Vehicle returnValue = null;

            try
            {
                _Context.Update(vehicle);
                _Context.SaveAsync();
                returnValue = vehicle;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"UpdateVehicle Unexpected Error: {ex}");
                return StatusCode(500, $"UpdateVehicle Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"UpdateVehicle complete.");
            return Ok(returnValue);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "MarkDeleted")]
        [HttpPatch]
        [Route("api/v1/Vehicle", Name = "MarkVehicleDeleted")]
        public async Task<IActionResult> MarkVehicleDeleted(int id)
        {
            int returnValue;

            try
            {
                _Context.MarkDeleted<Vehicle>(id);
                _Context.SaveAsync();
                returnValue = id;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"MarkVehicleDeleted Unexpected Error: {ex}");
                return StatusCode(500, $"MarkVehicleDeleted Unexpected Error: {ex}");
            }

            //return the new certificate
            _Logger.LogInformation($"MarkVehicleDeleted complete.");
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

            try
            {
                returnValue = _Context.Get<Vehicle>(FilterByUserId(userId), orderBy: q => q.OrderBy(x => x.Transponder));
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

        public static Expression<Func<Vehicle, bool>> FilterByUserId(Guid userId)
        {
            return x => x.UserId == userId;
        }
    }
}