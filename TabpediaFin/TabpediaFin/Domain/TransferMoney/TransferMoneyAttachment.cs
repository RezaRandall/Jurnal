﻿namespace TabpediaFin.Domain.TransferMoney;

public class TransferMoneyAttachment : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int TransId { get; set; }
}