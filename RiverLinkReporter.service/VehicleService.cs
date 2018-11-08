using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace RiverLinkReporter.Service
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAll();

        Task<Vehicle> Add(Vehicle vehicle);

        Task<int> Delete(int id);

        Task<Vehicle> Read(int id);

        Task<Vehicle> Update(Vehicle vehicle);

        Task MarkDeleted(int id);
    }

    public class VehicleService : IVehicleService
    {
        private readonly IRepository _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private ILogger<VehicleService> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly IEmailService _emailService;
        private readonly IRiverLinkReporter_JWTSettings _TokenOptions;


        private ModelStateDictionary _modelState;

        public VehicleService(
            IRepository Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> UserManager,
            SignInManager<IdentityUser> SignInManager,
            ILogger<VehicleService> Logger,
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

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return _Context.GetAll<Vehicle>();
        }

        public async Task<Vehicle> Add(Vehicle vehicle)
        {
            _Context.Create(vehicle);
            _Context.SaveAsync();
            return vehicle;
        }

        public async Task<int> Delete(int id)
        {
            _Context.Delete<Vehicle>(id);
            return id;
        }

        public async Task<Vehicle> Read(int id)
        {
            Vehicle vehicle = _Context.GetById<Vehicle>(id);
            return vehicle;
        }

        public async Task<Vehicle> Update(Vehicle vehicle)
        {
            _Context.Update(vehicle);
            return vehicle;
        }

        public async Task MarkDeleted(int id)
        {
            _Context.MarkDeleted<Vehicle>(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllByUserId(System.Guid userId)
        {            
            return _Context.Get<Vehicle>(FilterByUserId(userId), orderBy: query => query.OrderBy(x => x.Transponder));
        }

        Expression<Func<Vehicle, bool>> FilterByUserId(System.Guid userId)
        {
            return x => x.UserId == userId;
        }      
    }
}
