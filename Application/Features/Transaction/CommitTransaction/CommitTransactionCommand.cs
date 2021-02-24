using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Transaction.CommitTransaction
{
	public class CommitTransactionCommand : IRequest<bool>
	{
		public string ReceiverUserId { get; set; }
		
		public decimal Amount { get; set; }
		
		[JsonIgnore]
		public string UserEmail { get; set; }
	}
}