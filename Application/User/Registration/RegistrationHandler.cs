using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using EFData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Registration
{
	public class RegistrationHandler : IRequestHandler<RegistrationCommand, User>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;
		private readonly IJwtGenerator _jwtGenerator;
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public RegistrationHandler(DataContext context, UserManager<Domain.Entities.User> userManager, IJwtGenerator jwtGenerator, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_jwtGenerator = jwtGenerator;
			_mapper = mapper;
		}

		public async Task<User> Handle(RegistrationCommand request, CancellationToken cancellationToken)
		{
			if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
			{
				throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exist" });
			}

			var user = new Domain.Entities.User
							{
								DisplayName = request.DisplayName,
								Email = request.Email,
								Balance = 500
							};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded)
			{
				var userResponse = _mapper.Map<User>(user);
				userResponse.Token = _jwtGenerator.CreateToken(user);
				
				return userResponse;
			}

			throw new Exception("Client creation failed");
		}
	}
}