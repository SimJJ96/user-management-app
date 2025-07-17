namespace UserManagement.API.DTOs
{
    public class PageRequestDto
    {
        public int PageNumber { get; set; } = 1;  // default to 1
        public int PageSize { get; set; } = 10;   // default to 10
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc"; // or "desc"
        public string? Search { get; set; } // optional filtering
    }
}
