using FluentValidation;

namespace Application.Features.Transaction.CheckTransactionAmount
{
	public class CheckTransactionValidation : AbstractValidator<CheckTransactionQuery>
	{
		public CheckTransactionValidation()
		{
			RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
		}
	}
}