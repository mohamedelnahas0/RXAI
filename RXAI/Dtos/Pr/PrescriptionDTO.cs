namespace RXAI.Dtos.Pr
{
    public class PrescriptionDTO
    {
        public int PrescriptionID { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime? DispenseDate { get; set; }
    }
}
