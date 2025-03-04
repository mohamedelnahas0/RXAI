namespace RXAI.Dtos.TR
{
    public class UpdateTradeDto
    {
        public string Name { get; set; }

        public string PharmaceuticalForm { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityStock { get; set; }
        public string ManufactureCountry { get; set; }
    }
}
