
namespace JeevanSetu.Models
{
    public class BloodInventory
    {
        public int Id { get; set; }

        public string BloodType { get; set; } = string.Empty;

        public int? Quantity { get; set; }

        public int HospitalId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public DateTime LastChecked { get; set; }

        public Hospital? Hospital { get; set; }
    }
}
