
using System.ComponentModel.DataAnnotations;

namespace JeevanSetu.Models
{
    public class Donor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string BloodGroup { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; } = true;
[StringLength(256)]
public string? PasswordHash { get; set; }

public bool HasAccount { get; set; } = false;
    }
}