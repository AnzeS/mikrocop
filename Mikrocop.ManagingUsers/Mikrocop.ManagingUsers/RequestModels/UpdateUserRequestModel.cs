namespace Mikrocop.ManagingUsers.Models.CommandModels
{
    public class UpdateUserRequestModel
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string MobileNumber { get; set; }
        public required string Language { get; set; }
        public required string Culture { get; set; }
        public required string Password { get; set; }
    }
}
