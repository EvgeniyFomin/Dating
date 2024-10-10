using Microsoft.EntityFrameworkCore;

namespace Dating.Core.Models.Pagination
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, PaginationParameters parameters)
        {
            CurrentPage = parameters.PageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)parameters.PageSize);
            PageSize = parameters.PageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, PaginationParameters parameters)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();

            return new PagedList<T>(items, count, parameters);
        }
    }
}
