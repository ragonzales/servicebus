namespace PeruCompras.IntegracionMef.Transversal.Common.Models
{
    public class Email
    {
        public string Host { get; set; }
        public string From { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Alias { get; set; }        
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
