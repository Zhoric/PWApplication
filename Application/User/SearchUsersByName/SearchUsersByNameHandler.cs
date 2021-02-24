using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.User.GetUserInfo;
using AutoMapper;
using EFData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User.SearchUsersByName
{
	public class SearchUsersByNameHandler: IRequestHandler<SearchUsersByNameQuery, SearchUsersByNameResponse>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public SearchUsersByNameHandler(DataContext context, UserManager<Domain.Entities.User> userManager, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}
		
		public async Task<SearchUsersByNameResponse> Handle(SearchUsersByNameQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(query.UserEmail);
			var foundedUsers = await _context.Users.Where(u =>
					(string.IsNullOrEmpty(query.DisplayName) || u.DisplayName.Contains(query.DisplayName))
					&& u.Id != user.Id)
				.Select(u => _mapper.Map<UserInfo>(u))
				.ToListAsync(cancellationToken);
			
			return new SearchUsersByNameResponse()
			{
				Users = foundedUsers
			};
		}
	}
}