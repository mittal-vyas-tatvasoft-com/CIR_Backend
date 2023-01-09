namespace CIR.Core.Entities.Users
{
	public partial class Roles
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool AllPermissions { get; set; }
		public DateTime CreatedOn { get; set; }

	}
}
