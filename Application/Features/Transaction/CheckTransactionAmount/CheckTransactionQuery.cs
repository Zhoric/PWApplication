using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Transaction.CheckTransactionAmount
{
	public class CheckTransactionQuery : IRequest<bool>
	{
		public decimal Amount { get; set; }
		
		[JsonIgnore]
		public string UserEmail { get; set; }
	}
}