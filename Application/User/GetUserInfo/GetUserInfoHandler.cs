using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using AutoMapper;
using EFData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User.GetUserInfo
{
	public class GetUserInfoHandler: IRequestHandler<GetUserInfoQuery, User>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;
		private readonly IMapper _mapper;

		public GetUserInfoHandler(UserManager<Domain.Entities.User> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_mapper = mapper;
		}
		
		public async Task<User> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(query.Email);
			if (user == null)
			{
				throw new RestException(HttpStatusCode.Unauthorized, "User not found");
			}

			return _mapper.Map<User>(user);
		}
	}
}