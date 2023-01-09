namespace CIR.Common.EmailGeneration
{
    public class EmailModel
    {
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public bool Enablessl { get; set; }
        public int port { get; set; }
        public string Host { get; set; }

    }
}
