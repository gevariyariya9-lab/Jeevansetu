
using System.ComponentModel.DataAnnotations;

namespace JeevanSetu.Models
{
    public class Hospital
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Location { get; set; }

        public bool CriticalStatus { get; set; }

        [StringLength(100)]
        public string? Username { get; set; }

        [StringLength(256)]
        public string? PasswordHash { get; set; }

        public bool HasAccount { get; set; } = false;
    }
}
