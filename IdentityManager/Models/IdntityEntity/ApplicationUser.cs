using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityManager.Models.IdntityEntity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string Name {  get; set; }
    }
}
