using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class UserUpdatePasswordRequest
    {
        [Required]
        [RegularExpression(@"^.{8,}$")]
        [StringLength(64)]
        public string Password { get; set; }
    }
}