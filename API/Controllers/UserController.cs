using System.Security.Claims;
using System.Threading.Tasks;
using Application.User;
using Application.User.GetUserInfo;
using Application.User.Login;
using Application.User.Registration;
using Application.User.SearchUsersByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class UserController : BaseController
    {
		[AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(LoginQuery query)
        {
            return await Mediator.Send(query);
        }

		[AllowAnonymous]
		[HttpPost("registration")]
		public async Task<ActionResult<User>> RegistrationAsync(RegistrationCommand command)
		{
			return await Mediator.Send(command);
		}
		
		[HttpGet("userInfo")]
		public async Task<ActionResult<User>> GetUserInfoAsync()
		{
			var query = new GetUserInfoQuery()
			{
				Email =   User.FindFirstValue(ClaimTypes.NameIdentifier)
			};
			
			return await Mediator.Send(query);
		}
		
		[HttpGet("searchByName")]
		public async Task<ActionResult<SearchUsersByNameResponse>> SearchUsersByDisplayNameAsync([FromQuery] SearchUsersByNameQuery query)
		{
			query.UserEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return await Mediator.Send(query);
		}
    }
}
