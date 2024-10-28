namespace Dating.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public required string SenderName { get; set; }
        public required string RecipientName { get; set; }
        public required string Content { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReadDate { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        //navigation properties
        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;
        public int RecipientId { get; set; }
        public User Recipient { get; set; } = null!;
    }
}
