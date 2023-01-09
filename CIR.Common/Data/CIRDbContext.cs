using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.User;
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
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<GlobalConfigurationCutOffTime> GlobalConfigurationCutOffTimes { get; set; }

    }
}
