using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventApi.DTO
{
    public class InitialUser
    {
        public string Username { get; set; } = null!;
        public string Passwordhash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
