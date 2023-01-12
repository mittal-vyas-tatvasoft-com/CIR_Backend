namespace CIR.Core.Entities.GlobalConfig
{
    public class GlobalConfigurationReasons
    {
        public long Id { get; set; }

        public short Type { get; set; }

        public bool Enabled { get; set; }

        public string Content { get; set; } = null!;
    }
}
