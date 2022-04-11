using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class UserRefreshRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}