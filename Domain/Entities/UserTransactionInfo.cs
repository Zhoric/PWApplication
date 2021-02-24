using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities
{
	public class UserTransactionInfo
	{
		[Key]
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		
		[Key]
		public Guid TransactionId { get; set; }
		[ForeignKey("TransactionId")]
		public Transaction Transaction { get; set; }
		
		public decimal Amount { get; set; }
		
		public decimal FinalBalance { get; set; }
	}
}