namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class CountryCode
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string CountryName { get; set; } = null!;
    }
}
