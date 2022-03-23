using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class UserAuthorizeRequest
    {
        [Required]
        [RegularExpression(@"^[a-z0-9\._]{4,}$")]
        [StringLength(32)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^.{8,}$")]
        [StringLength(64)]
        public string Password { get; set; }
    }
}