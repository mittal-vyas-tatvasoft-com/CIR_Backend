namespace CIR.Core.Entities.Website
{
    public class PortalServiceTypes
    {
        public long Id { get; set; }
        public bool Enabled { get; set; }
        public decimal Cost { get; set; }
        public long PortalId { get; set; }
        public short Type { get; set; }
    }
}
