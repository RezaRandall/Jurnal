﻿using System.Runtime.Serialization;

namespace TabpediaFin.Dto
{
    //public class GetContactDto : IRequest<List<Contact>>
    //{
    //    public string? sortby { get; set; } = string.Empty;
    //    public string? valsort { get; set; } = string.Empty;
    //    public string? searchby { get; set; } = string.Empty;
    //    public string? valsearch { get; set; } = string.Empty;
    //    public int? jumlah_data { get; set; } = 5;
    //    public int? offset { get; set; } = 0;
    //    [JsonIgnore]
    //    public int? TenantId { get; set; } = 0;
    //}
    //public class GetCustomerListQuery : IRequest<List<Contact>>
    //{
    //    public string? sortby { get; set; } = string.Empty;
    //    public string? valsort { get; set; } = string.Empty;
    //    public string? searchby { get; set; } = string.Empty;
    //    public string? valsearch { get; set; } = string.Empty;
    //    public int? jumlah_data { get; set; } = 5;
    //    public int? offset { get; set; } = 0;
    //    [JsonIgnore]
    //    public int? TenantId { get; set; } = 0;
    //}
    //public class GetCustomerQuery : IRequest<Contact>
    //{
    //    public int? Id { get; set; } = 0;
    //    public int? TenantId { get; set; } = 0;
    //}
    public class DeleteCustomer : IRequest<bool>
    {
        public int? Id { get; set; } = 0;
        public int? TenantId { get; set; } = 0;
    }

    //public class AddCustomer : IRequest<Contact>
    //{
    //    public string Name { get; set; }
    //    public string Address { get; set; }
    //    public string CityName { get; set; }
    //    public string PostalCode { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public string Fax { get; set; }
    //    public string Website { get; set; }
    //    public string Npwp { get; set; }
    //    public int GroupId { get; set; }
    //    public string Notes { get; set; }
    //    public int? CreatedUid { get; set; } = 0;
    //    public int? TenantId { get; set; } = 0;
    //}

    //public class UpdateCustomer : IRequest<Contact>
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Address { get; set; }
    //    public string CityName { get; set; }
    //    public string PostalCode { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public string Fax { get; set; }
    //    public string Website { get; set; }
    //    public string Npwp { get; set; }
    //    public int GroupId { get; set; }
    //    public string Notes { get; set; }
    //    public int? UpdatedUid { get; set; } = 0;
    //    public int? TenantId { get; set; } = 0;
    //}
}
