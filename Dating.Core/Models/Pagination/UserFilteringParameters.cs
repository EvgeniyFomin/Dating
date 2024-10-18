using Dating.Core.Enums;

namespace Dating.Core.Models.Pagination
{
    public class UserFilteringParameters : PaginationParameters
    {
        // --  Filtering stuff --
        public Gender? Gender { get; set; }
        public string? CurrentUserName { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;

        // -- Sorting stuff --
        public string OrderBy { get; set; } = "lastActive";
    }
}
