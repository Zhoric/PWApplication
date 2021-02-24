using FluentValidation;

namespace Application.Features.Transaction.CommitTransaction
{
	public class CommitTransactionValidation : AbstractValidator<CommitTransactionCommand>
	{
		public CommitTransactionValidation()
		{
			RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
			RuleFor(x => x.ReceiverUserId).NotEmpty();
		}
	}
}