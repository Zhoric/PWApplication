using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using EFData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Transaction.CommitTransaction
{
	public class CommitTransactionHandler : IRequestHandler<CommitTransactionCommand, bool>
	{
		private readonly UserManager<Domain.Entities.User> _userManager;
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CommitTransactionHandler(DataContext context, UserManager<Domain.Entities.User> userManager, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}
		
		public async Task<bool> Handle(CommitTransactionCommand command, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(command.UserEmail);

			if (user.Balance < command.Amount)
			{
				throw new RestException(HttpStatusCode.BadRequest, new { exception = "Not enough PW to commit transaction" });
			}
			
			if (user.Id == command.ReceiverUserId)
			{
				throw new RestException(HttpStatusCode.BadRequest, new { exception = "You can't transfer PW to your account" });
			}

			var transaction = new Domain.Entities.Transaction()
			{
				ReceiverUserId = command.ReceiverUserId,
				Amount = command.Amount,
				UserId = user.Id,
				OperationDate = DateTime.Now
			};
			_context.Transactions.Add(transaction);

			user.Balance -= transaction.Amount;
			await _userManager.UpdateAsync(user);
			await _context.UserTransactionInfos.AddAsync(new UserTransactionInfo()
			{
				UserId = user.Id,
				TransactionId = transaction.Id,
				Amount = transaction.Amount,
				FinalBalance = user.Balance
			}, cancellationToken);
			
			var receiverUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == transaction.ReceiverUserId, cancellationToken: cancellationToken);
			receiverUser.Balance += transaction.Amount;
			await _context.UserTransactionInfos.AddAsync(new UserTransactionInfo()
			{
				UserId = receiverUser.Id,
				TransactionId = transaction.Id,
				Amount = transaction.Amount,
				FinalBalance = receiverUser.Balance
			}, cancellationToken);
			
			await _context.SaveChangesAsync(cancellationToken);
			
			return true;
		}
	}
}