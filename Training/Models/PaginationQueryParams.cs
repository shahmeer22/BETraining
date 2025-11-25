namespace Training.Models
{
    public class PaginationQueryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string Search { get; set; }
        public bool Descending { get; set; } = false;
    }
}
