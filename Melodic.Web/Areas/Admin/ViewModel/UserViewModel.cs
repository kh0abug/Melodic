namespace Melodic.Web.Areas.Admin.ViewModel
{
    public class UserViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
