namespace DesafioStone.Entities
{
    public class PaginationMetadata
    {
        public PaginationMetadata(long totalCount, int currentPage, int rowsPerPage, int rowsInCurrentPage)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            RowsPerPage = rowsPerPage;
            RowsInCurrentPage = rowsInCurrentPage;
            TotalPages = (int) Math.Ceiling(totalCount / (double)rowsPerPage);
        }

        public long TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int RowsPerPage { get; set; }
        public int RowsInCurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
