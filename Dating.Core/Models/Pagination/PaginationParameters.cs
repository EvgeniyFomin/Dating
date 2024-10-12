using Dating.Core.Enums;

namespace Dating.Core.Models.Pagination
{
    public class PaginationParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // --  Filtering stuff --
        public Gender? Gender { get; set; }
        public string? CurrentUserName { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
    }
}