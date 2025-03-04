namespace RXAI.Dtos.TR
{
    public class TradeNameDto
    {
        public string Skucode { get; set; }
        public string Name { get; set; }
        public string DrugBankID { get; set; }
        public string PharmaceuticalForm { get; set; }
        public decimal? Price { get; set; }
        public string Strength{ get; set; }
        public string StrengtUnit { get; set; }
        public int? QuantityStock { get; set; }
        public string ManufactureCountry { get; set; }
    }
}
