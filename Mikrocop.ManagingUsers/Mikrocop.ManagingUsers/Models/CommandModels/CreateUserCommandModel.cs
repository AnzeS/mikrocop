namespace Mikrocop.ManagingUsers.Models.CommandModels
{
    public class CreateUserCommandModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Language { get; set; }
        public string? Culture { get; set; }
        public string? Password { get; set; }
    }
}
