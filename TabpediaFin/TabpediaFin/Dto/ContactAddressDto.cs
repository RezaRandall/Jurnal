namespace TabpediaFin.Dto
{
    [Table("ContactAddress")]
    public class ContactAddressDto : BaseDto
    {
        public int ContactId { get; set; } = 0;
        public string AddressName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public string AddresType { get; set; } = string.Empty; 
        public string Notes { get; set; } = string.Empty;
        //public string creator { get; set; } = string.Empty;
        //public string editor { get; set; } = string.Empty;
        //public int CreatedUid { get; set; }
        //public int UpdateUid { get; set; }
        //public DateTime CreatedUtc { get; set; }
        //public DateTime UpdatedUtc { get; set; }
    }
}
