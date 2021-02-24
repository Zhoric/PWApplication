using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        
        public decimal Balance { get; set; }
        
        [NotMapped] 
        public override string UserName
        {
            get => Email;
            set => Email = value;
        }
    }
}