namespace Api.DTOs
{
    public class CreateLinkRequest
    {
        public string TargetURL { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public Guid? UserId { get; set; }
    }
}
