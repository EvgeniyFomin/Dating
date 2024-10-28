using Dating.Core.Enums;

namespace Dating.Core.Models.Pagination
{
    public class MessageParameters : PaginationParameters
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public required Container Container { get; set; } = Container.Unread;
    }
}
