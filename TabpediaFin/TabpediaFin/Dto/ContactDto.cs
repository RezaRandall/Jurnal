namespace TabpediaFin.Dto
{
    [Table("Contact")]
    public class ContactDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public string groupName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
        //public string creator { get; set; } = string.Empty;
        //public string editor { get; set; } = string.Empty;
        //public int CreatedUid { get; set; }
        //public int UpdateUid { get; set; }
        //public DateTime CreatedUtc { get; set; }
        //public DateTime UpdatedUtc { get; set; }
        public List<ContactAddressDto> ContactAddressList { get; set; }
        public List<ContactPersonDto> ContactPersonList { get; set; }
    }

    public class contactqueryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class filterrow {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public bool SortDesc { get; set; }
    }
    
}
