using System;

namespace Application.Features.Transaction.GetAllByUser
{
	public class TransactionResponse
	{
		public DateTime DateOperation { get; set; }
		
		public string CorrespondentName { get; set; }
		
		public decimal TransactionAmount { get; set; }
		
		public decimal ResultingBalance { get; set; }
	}
}