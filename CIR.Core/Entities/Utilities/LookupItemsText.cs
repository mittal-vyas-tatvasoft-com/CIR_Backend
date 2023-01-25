namespace CIR.Core.Entities.Utilities
{
    public partial class LookupItemsText
    {
        public long Id { get; set; }

        public long LookupItemId { get; set; }

        public long CultureId { get; set; }

        public int DisplayOrder { get; set; }

        public string Text { get; set; } = null!;

        public bool Active { get; set; }
    }


}


