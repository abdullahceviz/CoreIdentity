namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class ClaimViewModel
    {
        public string Issuer { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
