using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class UserRefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}