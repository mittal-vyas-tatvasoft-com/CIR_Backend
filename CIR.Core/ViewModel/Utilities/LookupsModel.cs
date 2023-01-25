using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.ViewModel.Utilities
{
    public class LookupsModel
    {
        public List<CulturalCodesModel> CulturalCodesList { get; set; } = new();
    }

    [NotMapped]
    public class CulturalCodesModel
    {
        public long CultureId { get; set; }
        public string CultureDisplayText { get; set; } = null!;
        public long SystemCodeId { get; set; }
        public string Code { get; set; } = null!;
    }

    public class UserLookupItemModel
    {
        public long LookupItemId { get; set; }
        public long LookupItemTextId { get; set; }
        public string LookupItemText { get; set; }
        public long SystemCodeId { get; set; }
        public string SystemCodeName { get; set; }
    }
}
