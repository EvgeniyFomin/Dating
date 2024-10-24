namespace Dating.Core.Dtos
{
    public class MessageUserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string PhotoUrl { get; set; }
    }
}
