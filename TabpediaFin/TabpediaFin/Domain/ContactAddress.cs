﻿namespace TabpediaFin.Domain
{
    public class ContactAddress : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int ContactId { get; set; } = 0;
        public string AddressName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
    }
}
