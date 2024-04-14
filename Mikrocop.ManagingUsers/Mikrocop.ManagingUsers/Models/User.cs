using Mikrocop.ManagingUsers.Helpers;
using Mikrocop.ManagingUsers.Models.CommandModels;
using Mikrocop.ManagingUsers.ResponseModels;

namespace Mikrocop.ManagingUsers.Models
{
    public class User
    {
        #region Constructors

        protected User() { }

        private User(CreateUserCommandModel cmd)
        {
            Id = cmd.Id;
            UserName = cmd.UserName!;
            FullName = cmd.FullName!;
            Email = cmd.Email!;
            MobileNumber = cmd.MobileNumber!;
            Language = cmd.Language!;
            Culture = cmd.Culture!;
            PasswordSalt = Hash.GenerateSalt();
            Password = Hash.HashPassword(cmd.Password!, PasswordSalt);
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }
        public string UserName { get; private set; } = null!;
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string MobileNumber { get; private set; } = null!;
        public string Language { get; private set; } = null!;
        public string Culture { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        #endregion

        #region Bussines logic

        private static void Validate(CreateUserCommandModel cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.UserName))
                throw new ArgumentException("User name cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.FullName))
                throw new ArgumentException("Full name cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.Email))
                throw new ArgumentException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.MobileNumber))
                throw new ArgumentException("Mobile number cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.Language))
                throw new ArgumentException("Language cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.Culture))
                throw new ArgumentException("Culture cannot be empty.");
            if (string.IsNullOrWhiteSpace(cmd.Password))
                throw new ArgumentException("Password cannot be empty.");
        }

        public static User Create(CreateUserCommandModel cmd)
        {
            if (cmd.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.");
            Validate(cmd);

            return new User(cmd);
        }

        public void Update(CreateUserCommandModel cmd)
        {
            Validate(cmd);
            UserName = cmd.UserName!;
            FullName = cmd.FullName!;
            Email = cmd.Email!;
            MobileNumber = cmd.MobileNumber!;
            Language = cmd.Language!;
            Culture = cmd.Culture!;
            Password = Hash.HashPassword(cmd.Password!, PasswordSalt);
        }

        public bool ValidatePassword(string password)
        {
            if (password == null)
                throw new ArgumentException("Password cannot be empty.");
            var hashedPassword = Hash.HashPassword(password, PasswordSalt);
            return hashedPassword == Password;
        }

        public UserResponseModel ToResponseModel()
        {
            return new UserResponseModel
            {
                Id = Id,
                Email = Email,
                MobileNumber = MobileNumber,
                Language = Language,
                Culture = Culture,
                UserName = UserName,
                FullName = FullName
            };
        }

        #endregion
    }
}
