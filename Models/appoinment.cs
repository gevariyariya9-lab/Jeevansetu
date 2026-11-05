
namespace JeevanSetu.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DonorId { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string DonationType { get; set; } = string.Empty;

        public string DonationLocation { get; set; } = string.Empty;

        public bool DonationConfirmed { get; set; } = false;

        public Donor? Donor { get; set; }
    }
}
