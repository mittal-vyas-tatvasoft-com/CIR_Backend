namespace CIR.Core.Entities.Users

{
    public partial class User
    {
        public long Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Email { get; set; }

        public long? SalutationLookupItemId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public long RoleId { get; set; }

        public bool Enabled { get; set; }

        public DateTime? LastLogOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastEditedOn { get; set; }

        public bool ResetRequired { get; set; }

        public bool DefaultAdminUser { get; set; }

        public string TimeZone { get; set; } = null!;

        public long CultureLcid { get; set; }

        public string? EmployeeId { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? ScheduledActiveChange { get; set; }

        public int LoginAttempts { get; set; }

        public string? CompanyName { get; set; }

        public long? PortalThemeId { get; set; }
    }
}
