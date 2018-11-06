using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using RiverLinkReporter.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace RiverLinkReporter.service
{
    public interface ITransponderService
    {
        Task<IEnumerable<Transponder>> GetAll();

        Task<Transponder> Add(Transponder transponder);

        Task<Transponder> Delete(Transponder transponder);
    }

    public class TransponderService : ITransponderService
    {
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private ILogger<TransponderService> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly IEmailService _emailService;
        private readonly IRiverLinkReporter_JWTSettings _TokenOptions;


        private ModelStateDictionary _modelState;

        public TransponderService(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> UserManager,
            SignInManager<IdentityUser> SignInManager,
            ILogger<TransponderService> Logger,
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

        public async Task<IEnumerable<Transponder>> GetAll()
        {
            return _Context.Transponders;
        }

        public async Task<Transponder> Add(Transponder transponder)
        {
            _Context.Transponders.Add(transponder);
            await _Context.SaveChangesAsync();
            return transponder;
        }

        public async Task<Transponder> Delete(Transponder transponder)
        {
            _Context.Transponders.Remove(transponder);
            await _Context.SaveChangesAsync();
            return transponder;
        }
    }
}

