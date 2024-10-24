namespace Dating.Core.Dtos
{
    public class CreateMessageDto
    {
        public int RecipientId { get; set; }
        public required string Content { get; set; }
    }
}
