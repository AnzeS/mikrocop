namespace Mikrocop.ManagingUsers.Models.CommandModels
{
    public class ValidateUserPasswordRequestModel
    {
        public required Guid Id { get; set; }
        public required string Password { get; set; }
    }
}
