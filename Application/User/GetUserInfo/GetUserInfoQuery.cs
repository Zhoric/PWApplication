using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.User.GetUserInfo
{
	public class GetUserInfoQuery : IRequest<User>
	{
		public string Email { get; set; }
	}
}