namespace TabpediaFin.Handler.ExportImportItemHandler
{
    public class ItemImportHandler : IRequestHandler<ItemImportInsertListDto, PagedListResponse<ItemReadFileListDto>>
    {
        private readonly FinContext _context;

        public ItemImportHandler(FinContext db)
        {
            _context = db;
        }
        public async Task<PagedListResponse<ItemReadFileListDto>> Handle(ItemImportInsertListDto request, CancellationToken cancellationToken)
        {
            var result = new PagedListResponse<ItemReadFileListDto>();
            List<Domain.Item> valueadd = new List<Domain.Item>();
            List<ItemReadFileListDto> row = new List<ItemReadFileListDto>();
            try
            {
                if (request.ItemList.Count > 0)
                {
                    foreach (ItemImportInsertDto item in request.ItemList)
                    {
                        valueadd.Add(new Domain.Item
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Code = item.Code,
                            Barcode = item.Barcode,
                            UnitMeasureId = item.UnitMeasureId,
                            AverageCost = item.AverageCost,
                            Cost = item.Cost,
                            Price = item.Price,
                            IsSales = item.IsSales,
                            IsPurchase = item.IsPurchase,
                            IsStock = item.IsStock,
                            StockMin = item.StockMin,
                            IsArchived = item.IsArchived,
                            ImageFileName = item.ImageFileName,
                            Notes = item.Notes,
                        });
                        row.Add(new ItemReadFileListDto
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Code = item.Code,
                            Barcode = item.Barcode,
                            UnitMeasureId = item.UnitMeasureId,
                            AverageCost = item.AverageCost,
                            Cost = item.Cost,
                            Price = item.Price,
                            IsSales = item.IsSales,
                            IsPurchase = item.IsPurchase,
                            IsStock = item.IsStock,
                            StockMin = item.StockMin,
                            IsArchived = item.IsArchived,
                            ImageFileName = item.ImageFileName,
                            Notes = item.Notes,
                        });
                    }
                }
                await _context.Item.AddRangeAsync(valueadd, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                result.RecordCount = row.Count();

                result.IsOk = true;
                result.List = row;
                result.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

    public class ItemImportInsertDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int UnitMeasureId { get; set; } = 0;
        public int AverageCost { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public int Price { get; set; } = 0;
        public bool IsSales { get; set; } = true;
        public bool IsPurchase { get; set; } = true;
        public bool IsStock { get; set; } = true;
        public int StockMin { get; set; } = 0;
        public bool IsArchived { get; set; } = true;
        public string ImageFileName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class ItemImportInsertListDto : IRequest<PagedListResponse<ItemReadFileListDto>>
    {
        public List<ItemImportInsertDto> ItemList { get; set; }
    }
}
