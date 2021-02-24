using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.User.SearchUsersByName
{
	public class SearchUsersByNameQuery : IRequest<SearchUsersByNameResponse>
	{
		public string DisplayName { get; set; }
		
		[JsonIgnore]
		public string UserEmail { get; set; }
	}
}