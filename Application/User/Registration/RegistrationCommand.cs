﻿using MediatR;

namespace Application.User.Registration
{
	public class RegistrationCommand : IRequest<User>
	{
		public string DisplayName { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }
		
		public string Password2 { get; set; }
	}
}