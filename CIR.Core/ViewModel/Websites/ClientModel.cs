using System.ComponentModel.DataAnnotations;

namespace CIR.Core.ViewModel.Websites
{
    public class ClientModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long SubsiteId { get; set; }
        public string Code { get; set; }
        public string? Domain { get; set; }

        public string? Description { get; set; }
        public bool Stopped { get; set; }
        public bool EmailStopped { get; set; }
    }
}
