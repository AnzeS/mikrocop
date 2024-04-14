using Mikrocop.ManagingUsers.IntegrationTests.Infrastructure;
using Mikrocop.ManagingUsers.Models.CommandModels;
using Mikrocop.ManagingUsers.ResponseModels;
using Newtonsoft.Json;
using System.Text;

namespace Mikrocop.ManagingUsers.IntegrationTests
{
    public class UserContorllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserContorllerTests(CustomWebApplicationFactory<Program> fact)
        {
            _client = fact.CreateClient();
            _client.DefaultRequestHeaders.Add("API-Key", "0bc5eb1a-b29a-4803-9a53-b3ce170819e9");
            _client.DefaultRequestHeaders.Add("User-Agent", "test");
        }

        #region Add

        [Fact]
        public async Task<UserResponseModel> Add_ValidRequest_ReturnsOk()
        {
            // Arrange
            var userRequest = new CreateUserRequestModel
            {
                Culture = "en-US",
                Email = "test@test.com",
                FullName = "Test",
                Language = "eng",
                MobileNumber = "040-123-123",
                Password = "Password",
                UserName = "Test"
            };

            var userRequestContent = new StringContent(JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users", userRequestContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserResponseModel>(responseData);
            Assert.NotNull(user);
            return user;
        }

        [Fact]
        public async Task Add_InValidRequest_ReturnsBadRequest()
        {
            // Arrange
            var userRequest = new CreateUserRequestModel
            {
                Culture = null,
                Email = null,
                FullName = null,
                Language = null,
                MobileNumber = null,
                Password = null,
                UserName = null
            };

            var userRequestContent = new StringContent(JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users", userRequestContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("Culture", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("Email", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("FullName", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("Language", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("MobileNumber", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("Password", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("UserName", responseData, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();

            var userUpdateRequest = new UpdateUserRequestModel
            {
                Id = user.Id,
                Culture = user.Culture,
                Email = "asd@asd.asd",
                FullName = user.FullName,
                Language = user.Language,
                MobileNumber = user.MobileNumber,
                UserName = user.UserName,
                Password = "122445"
            };
            var userUpdateRequestContent = new StringContent(JsonConvert.SerializeObject(userUpdateRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("api/Users", userUpdateRequestContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserResponseModel>(responseData);

            Assert.NotNull(user);
        }

        [Fact]
        public async Task Update_InValidRequest_ReturnsBadRequest()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();

            var userUpdateRequest = new UpdateUserRequestModel
            {
                Id = user.Id,
                Culture = user.Culture,
                Email = null,
                FullName = user.FullName,
                Language = null,
                MobileNumber = user.MobileNumber,
                UserName = user.UserName,
                Password = null
            };
            var userUpdateRequestContent = new StringContent(JsonConvert.SerializeObject(userUpdateRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("api/Users", userUpdateRequestContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("Language", responseData, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("Password", responseData, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task Update_InValidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userUpdateRequest = new UpdateUserRequestModel
            {
                Id = Guid.NewGuid(),
                Culture = "en-US",
                Email = "test@test.com",
                FullName = "Test",
                Language = "eng",
                MobileNumber = "040-123-123",
                Password = "Password",
                UserName = "Test"
            };
            var userUpdateRequestContent = new StringContent(JsonConvert.SerializeObject(userUpdateRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("api/Users", userUpdateRequestContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Equal("User does not exists.", responseData);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ExistingUser_ReturnsOk()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();

            // Act
            var response = await _client.DeleteAsync($"api/Users?userId={user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_NonExistingUser_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"api/Users?userId={userId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Equal("User does not exists.", responseData);
        }

        #endregion

        #region Get

        [Fact]
        public async Task Get_ExistingUser_ReturnsOk()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();

            // Act
            var response = await _client.GetAsync($"api/Users?userId={user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserResponseModel>(responseData);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task Get_NonExistingUser_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"api/Users?userId={userId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Equal("User does not exists.", responseData);
        }

        #endregion

        #region Validate

        [Fact]
        public async Task Validate_ValidPassword_ReturnsOk()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();
            var requestModel = new ValidateUserPasswordRequestModel
            {
                Id = user.Id,
                Password = "Password"
            };
            var requestModelContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users/Validate", requestModelContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var valid = JsonConvert.DeserializeObject<bool>(responseData);
            Assert.True(valid);
        }

        [Fact]
        public async Task Validate_InValidPassword_ReturnsOk()
        {
            // Arrange
            var user = await Add_ValidRequest_ReturnsOk();
            var requestModel = new ValidateUserPasswordRequestModel
            {
                Id = user.Id,
                Password = "Password1"
            };
            var requestModelContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users/Validate", requestModelContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var valid = JsonConvert.DeserializeObject<bool>(responseData);
            Assert.False(valid);
        }

        [Fact]
        public async Task Validate_UserNotExists_ReturnsOk()
        {
            // Arrange
            var requestModel = new ValidateUserPasswordRequestModel
            {
                Id = Guid.NewGuid(),
                Password = "Password1"
            };
            var requestModelContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users/Validate", requestModelContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Equal("User does not exists.", responseData);
        }

        #endregion
    }
}