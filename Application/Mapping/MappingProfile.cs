using Application.Features.Transaction.CommitTransaction;
using Application.User.GetUserInfo;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User.User, Domain.Entities.User>()
				.ReverseMap();

			CreateMap<UserInfo, Domain.Entities.User>()
				.ForMember(x => x.Id, opt => opt.MapFrom(x => x.UserId))
				.ReverseMap();
			
			CreateMap<Transaction, CommitTransactionCommand>()
				.ReverseMap();
		}
	}
}