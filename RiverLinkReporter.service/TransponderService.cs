using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RiverLinkReport.Models;
using RiverLinkReporter.models;
using RiverLinkReporter.service.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;
using RiverLinkReporter.Service;

namespace RiverLinkReporter.service
{
    public interface ITransponderService
    {
        Task<IEnumerable<Transponder>> GetAll();

        Task<Transponder> Add(Transponder transponder);

        Task<int> Delete(int id);

        //Task<int> Read(int id);

        Task<Transponder> Update(Transponder transponder);
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

        public async Task<int> Delete(int id)
        {
            _Context.Transponders.Remove(_Context.Transponders.Find(id));
            await _Context.SaveChangesAsync();
            return id;
        }

        //public async Task<int> Read(int id)
        //{
        //    Transponder transponder = _Context.Transponders.Find(id);
        //    return vehicle;
        //}

        public async Task<Transponder> Update(Transponder transponder)
        {
            _Context.Entry(transponder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _Context.SaveChangesAsync();
            return transponder;
        }
    }
}
