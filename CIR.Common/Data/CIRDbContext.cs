using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.ViewModel.GlobalConfig;
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
		public DbSet<GlobalMessagesModel> GlobalConfigurationMessages { get; set; }
		public DbSet<GlobalConfigurationCutOffTime> GlobalConfigurationCutOffTimes { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleGrouping2SubSite>().HasKey(x => new { x.RoleGroupingId, x.SubSiteId });
            modelBuilder.Entity<RoleGrouping2Permission>().HasKey(x => new { x.RoleGroupingId, x.PermissionEnumId });
            modelBuilder.Entity<RoleGrouping2Culture>().HasKey(x => new { x.RoleGroupingId, x.CultureLcid });
        }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<Fonts> Fonts { get; set; }
    }
}

