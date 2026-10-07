using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CreateLinkRequest
    {
        [Required, StringLength(2048), Url]
        public string TargetURL { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 3), RegularExpression("^[0-9a-zA-Z]+$")]
        public string? Alias { get; set; }
    }
}
