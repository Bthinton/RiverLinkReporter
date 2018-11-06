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
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAll();

        Task<Transaction> Add(Transaction transaction);

        Task<Transaction> Delete(Transaction transaction);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _Context;
        private readonly RiverLinkReporterSettings _Settings;
        private readonly IMemoryCache _Cache;
        private ILogger<TransactionService> _Logger;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly IEmailService _emailService;
        private readonly IRiverLinkReporter_JWTSettings _TokenOptions;


        private ModelStateDictionary _modelState;

        public TransactionService(
            ApplicationDbContext Context,
            //IMapper Mapper,
            IOptions<RiverLinkReporterSettings> Settings,
            IMemoryCache MemoryCache,
            UserManager<IdentityUser> UserManager,
            SignInManager<IdentityUser> SignInManager,
            ILogger<TransactionService> Logger,
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

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return _Context.Transactions;
        }

        public async Task<Transaction> Add(Transaction transaction)
        {
            _Context.Transactions.Add(transaction);
            await _Context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> Delete(Transaction transaction)
        {
            _Context.Transactions.Remove(transaction);
            await _Context.SaveChangesAsync();
            return transaction;
        }
    }
}
