namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class Currency
    {
        public long Id { get; set; }

        public string CodeName { get; set; } = null!;

        public string? Symbol { get; set; }
    }
}
