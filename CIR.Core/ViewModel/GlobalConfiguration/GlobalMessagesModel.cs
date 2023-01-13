namespace CIR.Core.ViewModel.GlobalConfiguration
{
    public class GlobalMessagesModel
    {
        public long Id { get; set; }

        public short Type { get; set; }

        public string Content { get; set; }

        public long CultureId { get; set; }
        public string CultureName { get; set; }
    }
}
