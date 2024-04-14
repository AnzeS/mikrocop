using Microsoft.AspNetCore.Mvc;
using Mikrocop.ManagingUsers.Models;
using Mikrocop.ManagingUsers.Models.CommandModels;

namespace Mikrocop.ManagingUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly AdministrationContext _administrationContext;
        public UsersController(AdministrationContext administrationContext)
        {
            _administrationContext = administrationContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequestModel userRequest)
        {
            var user = Models.User.Create(new CreateUserCommandModel
            {
                Id = Guid.NewGuid(),
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                Culture = userRequest.Culture,
                FullName = userRequest.FullName,
                Language = userRequest.Language,
                MobileNumber = userRequest.MobileNumber,
                Password = userRequest.Password
            });
            await _administrationContext.Users.AddAsync(user);
            await _administrationContext.SaveChangesAsync();
            return Ok(user.ToResponseModel());
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequestModel userRequest)
        {
            var user = _administrationContext.Users.FirstOrDefault(x => x.Id == userRequest.Id);
            if (user == null)
            {
                return BadRequest("User does not exists.");
            }
            user.Update(new CreateUserCommandModel
            {
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                Culture = userRequest.Culture,
                FullName = userRequest.FullName,
                Language = userRequest.Language,
                MobileNumber = userRequest.MobileNumber,
                Password = userRequest.Password
            });

            _administrationContext.Users.Update(user);
            await _administrationContext.SaveChangesAsync();
            return Ok(user.ToResponseModel());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = _administrationContext.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return BadRequest("User does not exists.");
            }

            _administrationContext.Users.Remove(user);
            await _administrationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult Get(Guid userId)
        {
            var user = _administrationContext.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return BadRequest("User does not exists.");
            }

            return Ok(user.ToResponseModel());
        }

        [HttpPost]
        [Route("Validate")]
        public IActionResult ValidatePassword(ValidateUserPasswordRequestModel userRequest)
        {
            var user = _administrationContext.Users.FirstOrDefault(x => x.Id == userRequest.Id);
            if (user == null)
            {
                return BadRequest("User does not exists.");
            }

            var valid = user.ValidatePassword(userRequest.Password);
            return Ok(valid);
        }
    }
}
