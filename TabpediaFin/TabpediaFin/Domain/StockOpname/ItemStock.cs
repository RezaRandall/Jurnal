﻿namespace TabpediaFin.Domain
{
    public class ItemStock : BaseEntity
    {
        public int Id { get; set; } = 0;
        public int WarehouseId { get; set; } = 0;
        public int ItemId { get; set; } = 0;
        public double Quantity { get; set; }
    }
}