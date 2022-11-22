namespace TabpediaFin.Handler.WarehouseHandler;

public class WarehouseUpdateHandler : IRequestHandler<WarehouseUpdateDto, RowResponse<WarehouseFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public WarehouseUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<WarehouseFetchDto>> Handle(WarehouseUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<WarehouseFetchDto>();

        try
        {
            var Warehouse = await _context.Warehouse.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            Warehouse.Name = request.Name;
            Warehouse.Description = request.Description;
            Warehouse.Address = request.Address;
            Warehouse.CityName = request.CityName;
            Warehouse.PostalCode = request.PostalCode;
            Warehouse.Email = request.Email;
            Warehouse.Phone = request.Phone;
            Warehouse.Fax = request.Fax;
            Warehouse.ContactPersonName = request.ContactPersonName;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new WarehouseFetchDto()
            {
                Id = request.Id,
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

            result.IsOk= true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk= false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}


public class WarehouseUpdateDto : IRequest<RowResponse<WarehouseFetchDto>>
{
    public int Id { get; set; }
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
