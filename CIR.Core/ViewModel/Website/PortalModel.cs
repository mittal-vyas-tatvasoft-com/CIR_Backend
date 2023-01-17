using System.ComponentModel.DataAnnotations;

namespace CIR.Core.ViewModel.Website
{
    public class PortalModel
    {
        [Required]
        public string DisplayName { get; set; }

        public string? Directory { get; set; }
        public string? Description { get; set; }

        [Required]
        public bool Stopped { get; set; }

        [Required]
        public bool EmailStopped { get; set; }


        public bool CreateResponse { get; set; }


        [Required]
        public bool CountReturnIdentifier { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please Enter Valid Email Address")]
        public string? SystemEmailFromAddress { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please Enter Valid Email Address")]
        public string? BccemailAddress { get; set; }

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

}
