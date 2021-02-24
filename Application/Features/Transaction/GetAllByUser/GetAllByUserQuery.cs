using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Transaction.GetAllByUser
{
	public class GetAllByUserQuery : IRequest<IList<TransactionResponse>>
	{
		public DateTime? DateOperation { get; set; }
		
		public string CorrespondentName { get; set; }
		
		public decimal? TransactionAmount { get; set; }
		
		public string SortColumn { get; set; }
		
		[JsonIgnore]
		public string UserEmail { get; set; }
	}
}