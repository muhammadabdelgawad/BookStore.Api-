namespace BookStore.Api.Helper
{
    public class Pagination<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();

    }
}
