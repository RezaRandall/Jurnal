﻿namespace TabpediaFin.Dto
{
    [Table("ContactPerson")]
    public class ContactPersonDto : BaseDto
    {
        public int ContactId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Others { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        //public string creator { get; set; } = string.Empty;
        //public string editor { get; set; } = string.Empty;
        //public int CreatedUid { get; set; }
        //public int UpdateUid { get; set; }
        //public DateTime CreatedUtc { get; set; }
        //public DateTime UpdatedUtc { get; set; }
    }
}
