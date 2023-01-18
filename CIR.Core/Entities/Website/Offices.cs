namespace CIR.Core.Entities.Websites
{
	public partial class Offices
	{
		public long Id { get; set; }

		public string? Name { get; set; }

		public string AddressLine1 { get; set; } = null!;

		public string? AddressLine2 { get; set; }

		public string TownCity { get; set; } = null!;

		public string? StateCounty { get; set; }

		public long CountryCode { get; set; }

		public string? Postcode { get; set; }

		public string? TelNo { get; set; }

		public string? FaxNo { get; set; }

		public string? Email { get; set; }

		public string? Website { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? LastEditedOn { get; set; }

		public bool Enabled { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longitude { get; set; }

		public string? RegisteredNo { get; set; }

		public string? Description { get; set; }

		public long? AssetId { get; set; }

		public short AddressType { get; set; }

		public long? StoreId { get; set; }

		public bool? IsDefault { get; set; }

	}
}
