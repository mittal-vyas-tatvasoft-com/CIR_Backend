using System.ComponentModel.DataAnnotations;

namespace CIR.Core.ViewModel.Website
{
    public class PortalModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public long ClientId { get; set; }

        public string? Directory { get; set; }
        public string? Domain { get; set; }
        public string? Description { get; set; }

        [Required]
        public bool Stopped { get; set; }

        [Required]
        public bool EmailStopped { get; set; }


        public bool CreateResponse { get; set; }


        [Required]
        public bool CountReturnIdentifier { get; set; }

        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string? SystemEmailFromAddress { get; set; }

        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string? BccEmailAddress { get; set; }

        [Required]
        public long CurrencyId { get; set; }

        [Required]
        public long CountryId { get; set; }

        [Required]
        public long CultureId { get; set; }

        [Required]
        public short IntegrationLevel { get; set; }
        public string Entity { get; set; }
        public string Account { get; set; }

        [Required]
        public bool PostalServiceTypeEnabled { get; set; }

        [Required]
        public decimal PostalServiceTypeCost { get; set; }

        [Required]
        public bool DropOffServiceTypeEnabled { get; set; }

        [Required]
        public decimal DropOffServiceTypeCost { get; set; }

        [Required]
        public bool CollectionServiceTypeEnabled { get; set; }

        [Required]
        public decimal CollectionServiceTypeCost { get; set; }
    }

    public class ClientPortalsModel
    {
        public long ClientId { get; set; }
        public long Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class PortalSubModel
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
    }
}
