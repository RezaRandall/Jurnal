﻿namespace TabpediaFin.Domain
{
    public class ExpenseCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public int AccountId { get; set; }
    }
}
