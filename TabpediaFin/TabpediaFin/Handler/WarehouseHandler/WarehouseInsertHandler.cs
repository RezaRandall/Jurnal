using TabpediaFin.Handler.WarehouseHandler;

namespace TabpediaFin.Handler.WarehouseHandler;

public class WarehouseInsertHandler : IRequestHandler<WarehouseInsertDto, RowResponse<WarehouseFetchDto>>
{
    private readonly FinContext _context;

    public WarehouseInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<WarehouseFetchDto>> Handle(WarehouseInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<WarehouseFetchDto>();

        var Warehouse = new Warehouse()
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            CityName = request.CityName,
            PostalCode = request.PostalCode,
            Email = request.Email,
            Phone = request.Phone,
            Fax = request.Fax,
            ContactPersonName = request.ContactPersonName,

        };

        try
        {
            await _context.Warehouse.AddAsync(Warehouse, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new WarehouseFetchDto()
            {
                Id = Warehouse.Id,
                Name = Warehouse.Name,
                Description = Warehouse.Description,
                Address = Warehouse.Address,
                CityName = Warehouse.CityName,
                PostalCode = Warehouse.PostalCode,
                Email = Warehouse.Email,
                Phone = Warehouse.Phone,
                Fax = Warehouse.Fax,
                ContactPersonName = Warehouse.ContactPersonName,

            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}



public class WarehouseInsertDto : IRequest<RowResponse<WarehouseFetchDto>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string ContactPersonName { get; set; } = string.Empty;

}
