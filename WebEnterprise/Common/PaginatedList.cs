using Microsoft.EntityFrameworkCore;

namespace WebEnterprise.Common
{
    public class PaginatedList<T> : List<T>
    {
        public Pagination PagingInfo { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalRecords { get; private set; }
        public int PageSize { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrev { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, int totalPage)
        {
            PagingInfo = new Pagination(count, pageIndex, pageSize, totalPage);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var totalPage = (int)Math.Ceiling(count / (double)pageSize);
            pageIndex = pageIndex > totalPage ? totalPage : pageIndex;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize, totalPage);
        }
    }

    public class Pagination {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalRecords { get; private set; }
        public int PageSize { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrev { get; private set; }

        public Pagination(int count, int pageIndex, int pageSize, int totalPage)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = totalPage;
            TotalRecords = count;

            if (pageIndex > 1)
            {
                HasPrev = true;
            }
            else
            {
                HasPrev = false;
            }

            if (pageIndex < TotalPages)
            {
                HasNext = true;
            }
            else
            {
                HasNext = false;
            }
        }
    }
}
