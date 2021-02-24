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

namespace Application.Features.Transaction.GetAllByUser
{
	public class GetAllByUserHandler : IRequestHandler<GetAllByUserQuery, IList<TransactionResponse>>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public GetAllByUserHandler(DataContext context, UserManager<Domain.Entities.User> userManager, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}
		
		public async Task<IList<TransactionResponse>> Handle(GetAllByUserQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(query.UserEmail);
			if (user == null)
			{
				throw new RestException(HttpStatusCode.Unauthorized, "User not found");
			}

			var dbQuery = _context.UserTransactionInfos.Where(t => t.UserId == user.Id);
			if (query.DateOperation.HasValue)
				dbQuery = dbQuery.Where(x => x.Transaction.OperationDate.Date == query.DateOperation.GetValueOrDefault().Date);
			if (!string.IsNullOrEmpty(query.CorrespondentName))
				dbQuery = dbQuery.Where(x => x.Transaction.ReceiverUser.DisplayName.Contains(query.CorrespondentName));
			if (query.TransactionAmount.HasValue)
				dbQuery = dbQuery.Where(x => x.Transaction.Amount == query.TransactionAmount.GetValueOrDefault());

			var result = dbQuery.Select(x => new TransactionResponse()
			{
				DateOperation = x.Transaction.OperationDate,
				CorrespondentName = x.Transaction.ReceiverUserId == user.Id ? x.Transaction.User.DisplayName : x.Transaction.ReceiverUser.DisplayName,
				TransactionAmount = x.Transaction.ReceiverUserId == user.Id ? x.Amount : -x.Amount,
				ResultingBalance = x.FinalBalance
			});
			
			if (!string.IsNullOrEmpty(query.SortColumn))
			{
				switch (query.SortColumn)
				{
					case "DateOperation":
						result = result.OrderBy(x => x.DateOperation);
						break;
					case "CorrespondentName":
						result = result.OrderBy(x => x.CorrespondentName);
						break;
					case "TransactionAmount":
						result = result.OrderBy(x => x.TransactionAmount);
						break;
				}
			}
			else
			{
				result = result.OrderByDescending(x => x.DateOperation);
			}
			
			return await result.ToListAsync(cancellationToken: cancellationToken);
		}	
	}
}