namespace Organisation_WebAPI.ViewModels
{
    public class PaginationResultVM <T>
    {
        public int CurrentPage { get; set; }
        public int TotalNoOfRecords { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PaginationResultVM(int currentPage, int totalNoOfRecords, int pageSize, int totalPages, List<T> items)
        {
            CurrentPage = currentPage;
            TotalNoOfRecords = totalNoOfRecords;
            PageSize = pageSize;
            TotalPages = totalPages;
            Items = items;
        }
    }
}
