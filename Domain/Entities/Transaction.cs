using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities
{
	public class Transaction
	{
		[Key]
		public Guid Id { get; set; }
		
		public DateTime OperationDate { get; set; }
		
		public decimal Amount { get; set; }
		
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		
		public string ReceiverUserId { get; set; }
		[ForeignKey("ReceiverUserId")]
		public User ReceiverUser { get; set; }
	}
}