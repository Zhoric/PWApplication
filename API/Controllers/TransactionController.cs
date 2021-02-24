using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Transaction.CheckTransactionAmount;
using Application.Features.Transaction.CommitTransaction;
using Application.Features.Transaction.GetAllByUser;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class TransactionController : BaseController
	{
		[HttpGet("checkAmount")]
		public async Task<object> CheckTransactionAmountAsync([FromQuery] CheckTransactionQuery query)
		{
			query.UserEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return await Mediator.Send(query);
		}
		
		[HttpPost]
		public async Task<IActionResult> PostAsync(CommitTransactionCommand command)
		{
			command.UserEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return Ok(await Mediator.Send(command));
		}
		
		[HttpGet]
		public async Task<IActionResult> GetAllAsync([FromQuery] GetAllByUserQuery query)
		{
			query.UserEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var transactions = await Mediator.Send(query);
			return Ok(transactions);
		}
	}
}