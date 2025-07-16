namespace UserManagement.API.DTOs
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? ZipCode { get; set; }
    }
}
