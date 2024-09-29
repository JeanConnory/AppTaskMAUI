using AppTask.API.Libraries.Constants;
using AppTask.API.Libraries.Emails;
using AppTask.API.Libraries.Text;
using AppTask.Database.Repositories;
using AppTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserModelRepository _repository;
        private EmailAccessToken _email;

        public UsersController(IUserModelRepository repository, EmailAccessToken email)
        {
            _repository = repository;
            _email = email;
        }

        [HttpGet]
        public IActionResult GetAccessToken(string email) 
        {
            var user = _repository.GetByEmail(email);
            if (user == null)
            {
                user = new UserModel()
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    AccessToken = string.Empty.GenerateHash(4),
                    AccessTokenCreated = DateTimeOffset.Now,
                    Created = DateTimeOffset.Now
                };

                _repository.Add(user);

            }
            else
            {
                user.AccessToken = string.Empty.GenerateHash(4);
                user.AccessTokenCreated = DateTimeOffset.Now;

                _repository.Update(user);
            }

            _email.Send(user);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult ValidateAccessToken(UserModel userModel)
        {
            var user = _repository.GetByEmail(userModel.Email);
            if (userModel == null)
                return NotFound();

            if(user.AccessToken == userModel.AccessToken)
            {
                var tokenLimitHours = user.AccessTokenCreated.Add(Config.LimitAccessTokenCreated);
                var serverHours = DateTimeOffset.Now;

                if (serverHours <= tokenLimitHours)
                {
                    return Ok(user);
                }
            }

            return BadRequest("AccessToken Inválido ou vencido");
        }
    }
}
