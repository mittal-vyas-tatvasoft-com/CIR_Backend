﻿using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Users;
using CIR.Core.Entities.Utilities;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
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
        public DbSet<Culture> Cultures
        {
            get;
            set;
        }
        public DbSet<SubSite> SubSites
        {
            get;
            set;
        }
        public DbSet<RolePrivileges> RolePrivileges
        {
            get;
            set;
        }
        public DbSet<RoleGrouping> RolesGroupings
        {
            get;
            set;
        }
        public DbSet<RoleGrouping2Culture> RoleGrouping2Cultures
        {
            get;
            set;
        }

        public DbSet<RoleGrouping2Permission> RoleGrouping2Permissions
        {
            get;
            set;
        }
        public DbSet<RoleGrouping2SubSite> RoleGrouping2SubSites
        {
            get;
            set;
        }
        public DbSet<GlobalConfigurationCutOffTime> GlobalConfigurationCutOffTimes { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<GlobalConfigurationFonts> GlobalConfigurationFonts { get; set; }
        public DbSet<GlobalConfigurationWeekends> Weekends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleGrouping2SubSite>().HasKey(x => new { x.RoleGroupingId, x.SubSiteId });
            modelBuilder.Entity<RoleGrouping2Permission>().HasKey(x => new { x.RoleGroupingId, x.PermissionEnumId });
            modelBuilder.Entity<RoleGrouping2Culture>().HasKey(x => new { x.RoleGroupingId, x.CultureLcid });
        }


        public DbSet<GlobalConfigurationEmails> GlobalConfigurationEmails
        {
            get;
            set;
        }
        public DbSet<GlobalConfigurationMessages> GlobalConfigurationMessages { get; set; }

        public DbSet<GlobalConfigurationFonts> Fonts { get; set; }
        public DbSet<GlobalConfigurationStyle> GlobalConfigurationStyles { get; set; }

        public DbSet<GlobalConfigurationReasons> GlobalConfigurationReasons { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Portal2GlobalConfigurationReasons> portal2GlobalConfigurationReasons { get; set; }
        public DbSet<Offices> offices { get; set; }
        public DbSet<Portals> portals { get; set; }
        public DbSet<PortalToGlobalConfigurationEmails> Portal2GlobalConfigurationEmails { get; set; }
        public DbSet<Portal2GlobalConfigurationCurrency> Portal2GlobalConfigurationCurrencies { get; set; }
        public DbSet<PortalServiceTypes> PortalServiceTypes { get; set; }
        public DbSet<Portal2GlobalConfigurationMessage> Portal2GlobalConfigurationMessages { get; set; }
        public DbSet<LookupItemsText> LookupItemsText { get; set; }
        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<LookupItem> LookupItems { get; set; }
        public DbSet<Portal2GlobalConfigurationCutOffTimes> Portal2GlobalConfigurationCutOffTimes { get; set; }
        public DbSet<Portal2GlobalConfigurationStyle> Portal2GlobalConfigurationStyles { get; set; }
    }
}


