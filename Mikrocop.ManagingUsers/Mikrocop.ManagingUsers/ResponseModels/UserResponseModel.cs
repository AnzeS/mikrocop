namespace Mikrocop.ManagingUsers.ResponseModels
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string Culture { get; set; } = null!;
    }
}
