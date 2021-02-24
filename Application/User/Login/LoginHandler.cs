using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, User>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;

		private readonly SignInManager<Domain.Entities.User> _signInManager;

		private readonly IJwtGenerator _jwtGenerator;
		
		private readonly IMapper _mapper;

		public LoginHandler(UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager, IJwtGenerator jwtGenerator, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtGenerator = jwtGenerator;
			_mapper = mapper;
		}

		public async Task<User> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				throw new RestException(HttpStatusCode.Unauthorized, "User not found");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{

				var userResponse = _mapper.Map<User>(user);
				userResponse.Token = _jwtGenerator.CreateToken(user);
				
				return userResponse;
			}

			throw new RestException(HttpStatusCode.Unauthorized, "Password is incorrect");
		}
	}
}
