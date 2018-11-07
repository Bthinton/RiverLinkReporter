using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RiverLinkReporter.Service
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAll();

        Task<Vehicle> Add(Vehicle vehicle);

        Task<int> Delete(int id);

        //Task<int> Read(int id);

        Task<Vehicle> Update(Vehicle vehicle);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private ILogger<VehicleService> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly IEmailService _emailService;
        private readonly IRiverLinkReporter_JWTSettings _TokenOptions;


        private ModelStateDictionary _modelState;

        public VehicleService(
            ApplicationDbContext Context,
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
            return _Context.Vehicles;
        }

        public async Task<Vehicle> Add(Vehicle vehicle)
        {
            _Context.Vehicles.Add(vehicle);
            await _Context.SaveChangesAsync();         
            return vehicle;
        }

        public async Task<int> Delete(int id)
        {
            _Context.Vehicles.Remove(_Context.Vehicles.Find(id));
            await _Context.SaveChangesAsync();
            return id;
        }

        //public async Task<int> Read(int id)
        //{
        //    Vehicle vehicle = _Context.Vehicles.Find(id);
        //    return vehicle;
        //}

        public async Task<Vehicle> Update(Vehicle vehicle)
        {
            _Context.Entry(vehicle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _Context.SaveChangesAsync();
            return vehicle;
        }
    }
}
