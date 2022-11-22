using MediatR;
using Microsoft.EntityFrameworkCore;
using TabpediaFin.Handler.ContactHandler;
using TabpediaFin.Infrastructure.Data;

namespace TabpediaFin.Handler.Item;

//public class ItemDeleteHandler : IRequestHandler<ItemDeleteDto, bool>
//{
//    private readonly FinContext _context;

//    public ItemDeleteHandler(FinContext db)
//    {
//        _context = db;
//    }

//    public class CommandValidator : AbstractValidator<ItemDto>
//    {
//        public CommandValidator()
//        {
//            RuleFor(x => x.Id).NotNull().NotEmpty();
//        }
//    }

//    public async Task<RowResponse<ItemDto>> Handle(ItemDeleteDto req, CancellationToken cancellationToken)
//    {
//        //var result = new RowResponse<ItemDto>();

//        //var item = await _context.Item.FirstAsync(x => x.Id == req.Id, cancellationToken);
//        //if (item == null)
//        //{
//        //    throw new Exception("Not Found");
//        //}
//        //else 
//        //{
//        //    throw new Exception("Item with id ");
//        //}
//        //await _context.SaveChangesAsync(cancellationToken);
//        //return result;
//        var result = await _context.ItemDeleteHandler(req);
//        return result;
//    }

//}

//[Table("Item")]
//public class ItemDeleteDto : IRequest<bool>
//{
//    public int Id { get; set; } = 0;
//}

//public class ItemDeleteHandler
//{
//    public class Command : IRequest
//    {
//        public Command(int id)
//        {
//            Id = id;
//        }

//        public int Id { get; set; }
//    }

//    public class CommandValidator : AbstractValidator<ItemDeleteHandler.Command>
//    {
//        public CommandValidator()
//        {
//            RuleFor(x => x.Id).NotNull().NotEmpty();
//        }
//    }

//    public class Handler : IRequestHandler<Command, Unit>
//    {
//        private readonly FinContext _context;

//        public Handler(FinContext db)
//        {
//            _context = db;
//        }

//        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
//        {
//            var expense = await _context.Item.FirstOrDefaultAsync(
//                x => x.Id == request.Id,
//                cancellationToken);

//            if (expense == null)
//            {
//                throw new Exception("Not Found");
//            }

//            await _context.SaveChangesAsync(cancellationToken);
//            return Unit.Value;
//        }
//    }

//}



//public class ItemDeleteHandler
//{
//    public class Command : IRequest
//    {
//        public int Id { get; set; }
//    }

//    public class Validator : AbstractValidator<Command>
//    {
//        public Validator()
//        {
//            RuleFor(c => c.Id).GreaterThan(0);
//        }
//    }


//    public class CommandHandler : IRequestHandler<Command, Unit>
//    {
//        private readonly FinContext _db;
//        public CommandHandler(FinContext db) => _db = db;
//        public async Task<Unit> Handle(Command req, CancellationToken cancellationToken)
//        {
//            var response = new RowResponse<ItemDto>();

//            var itemData = await _db.Item.FindAsync(req.Id);
//            if (itemData == null)
//            {
//                response.IsOk = false;
//                response.ErrorMessage = "Data not found";
//                return Unit.Value;
//            }
//            else 
//            {
//                response.IsOk = true;
//                response.ErrorMessage = "Item with id " + req.Id + " has been deleted" ;
//            }

//            _db.Item.Remove(itemData);
//            await _db.SaveChangesAsync(cancellationToken);
//            return Unit.Value;

//        }
//    }
//}

public class ItemDeleteHandler : IRequestHandler<ItemDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(ItemDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var itemData = await _context.Item.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (itemData != null)
            {
                _context.Item.Remove(itemData);
                result.IsOk = true;
                result.ErrorMessage = "Item with id " + request.Id + " has been deleted";
            }
            if (itemData == null)
            {
                result.IsOk = false;
                result.ErrorMessage = "Data not found";
            }
                await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}

[Table("Item")]
public class ItemDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; } = 0;
    public int TenantId { get; set; } = 0;
}

