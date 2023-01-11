namespace CIR.Core.Entities
{
    public partial class Culture
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Name { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public bool Enabled { get; set; }

        public string NativeName { get; set; } = null!;
    }
}
