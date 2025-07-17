namespace UserManagement.API.DTOs
{
    public class PageResponseDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}
