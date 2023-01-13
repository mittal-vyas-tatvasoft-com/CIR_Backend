using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.Users

{
	public partial class User
	{
		public long Id { get; set; }

		public string UserName { get; set; } = null!;
		[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Your password must be at least 8 characters long, contain at least one number and have a mixture of uppercase and lowercase letters")]
		public string Password { get; set; } = null!;

		[RegularExpression(@"^\\S+@\\S+\\.\\S+$", ErrorMessage = "Please Enter Valid Email Address")]
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

		[RegularExpression(@"^\\+?[1-9][0-9]{7,14}$", ErrorMessage = "Please Enter Valid Phone Number")]
		public string? PhoneNumber { get; set; }

		public DateTime? ScheduledActiveChange { get; set; }

		public int LoginAttempts { get; set; }

		public string? CompanyName { get; set; }

		public long? PortalThemeId { get; set; }

	}
}
