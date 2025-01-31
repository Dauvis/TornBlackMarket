﻿using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TornBlackMarket.Data.Abstraction
{
    public class DataStoreRepository<T>
    {
        private readonly SqlConnection _database;

        private readonly ILogger<T> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DataStoreRepository(SqlConnection database, ILogger<T> logger, IServiceProvider serviceProvider, IMapper mapper, IConfiguration configuration)
        {
            _database = database;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Initialize()
        {

        }

        protected SqlConnection Connection => _database;
        protected ILogger<T> Logger => _logger;
        protected IServiceProvider ServiceProvider => _serviceProvider;
        protected IMapper Mapper => _mapper;
        protected IConfiguration Configuration => _configuration;
    }
}