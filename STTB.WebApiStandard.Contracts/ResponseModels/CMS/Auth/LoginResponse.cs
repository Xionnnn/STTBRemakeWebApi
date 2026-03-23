namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
