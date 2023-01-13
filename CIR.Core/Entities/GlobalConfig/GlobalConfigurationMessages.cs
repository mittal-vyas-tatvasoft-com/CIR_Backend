namespace CIR.Core.Entities.GlobalConfig
{
    public class GlobalConfigurationMessages
    {
        public long Id { get; set; }

        public short Type { get; set; }

        public string Content { get; set; } = null!;

        public long CultureId { get; set; }

    }
}
