﻿using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using Microsoft.EntityFrameworkCore;

namespace CIR.Common.Data
{
    public class CIRDbContext : DbContext
    {
        public CIRDbContext(DbContextOptions<CIRDbContext> options) : base(options) { }
        public DbSet<User> Users
        {
            get;
            set;
        }
        public DbSet<Currency> Currencies
        {
            get;
            set;
        }
        public DbSet<CountryCode> CountryCodes
        {
            get;
            set;
        }
        public DbSet<GlobalConfigurationCurrency> GlobalConfigurationCurrencies
        {
            get;
            set;
        }
    }
}
