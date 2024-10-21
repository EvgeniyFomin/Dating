namespace Dating.Core.Models.Pagination
{
    public class LikesFilteringParameters : PaginationParameters
    {
        public int UserId { get; set; }

        public required string Predicate { get; set; } = "liked";
    }
}
