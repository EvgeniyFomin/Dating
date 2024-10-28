namespace Dating.Core.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public required MessageUserDto Sender { get; set; }
        public required MessageUserDto Recipient { get; set; }
        public required string Content { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
