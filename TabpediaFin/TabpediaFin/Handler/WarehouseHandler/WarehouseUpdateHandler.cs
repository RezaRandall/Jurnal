namespace TabpediaFin.Handler.WarehouseHandler;

public class WarehouseUpdateHandler : IRequestHandler<WarehouseUpdateDto, RowResponse<WarehouseFetchDto>>
{
    private readonly FinContext _context;
    private readonly IWarehouseCacheRemover _cacheRemover;

    public WarehouseUpdateHandler(FinContext db, IWarehouseCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<WarehouseFetchDto>> Handle(WarehouseUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<WarehouseFetchDto>();

        try
        {
            var Warehouse = await _context.Warehouse.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (Warehouse == null || Warehouse.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

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
            
            _cacheRemover.RemoveCache();
            
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

public class WarehouseUpdateValidator : AbstractValidator<WarehouseUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public WarehouseUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(
                async (model, name, cancellation) =>
                {
                    return await IsUniqueName(model, name, cancellation);
                }
            ).WithMessage("Name must be unique");

        RuleFor(x => x.Description).MaximumLength(250);
        RuleFor(x => x.CityName).MaximumLength(250);
        RuleFor(x => x.PostalCode).MaximumLength(250);
        RuleFor(x => x.Phone).MaximumLength(250);
        RuleFor(x => x.Fax).MaximumLength(250);
        RuleFor(x => x.Email).MaximumLength(250);
        RuleFor(x => x.ContactPersonName).MaximumLength(250);
    }

    public async Task<bool> IsUniqueName(WarehouseUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsWarehouseNameExist(name, model.Id);
        return !isExist;
    }
}
