using System.Threading;
using System.Threading.Tasks;
using Application.Features.Transaction.CommitTransaction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Transaction.CheckTransactionAmount
{
	public class CheckTransactionAmountHandler : IRequestHandler<CheckTransactionQuery, bool>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;

		public CheckTransactionAmountHandler(UserManager<Domain.Entities.User> userManager)
		{
			_userManager = userManager;
		}
		
		public async Task<bool> Handle(CheckTransactionQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(query.UserEmail);

			return user.Balance >= query.Amount;
		}
	}
}