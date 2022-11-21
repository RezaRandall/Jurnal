using MediatR;
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

public class ItemDeleteHandler
{
    public class Command : IRequest
    {
        public Command(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class CommandValidator : AbstractValidator<ItemDeleteHandler.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly FinContext _context;

        public Handler(FinContext db)
        {
            _context = db;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var expense = await _context.Item.FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

            if (expense == null)
            {
                throw new Exception("Not Found");
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }





}

//[Table("Item")]
//public class ItemDeleteDto : IRequest<RowResponse<ItemDto>>
//{
//    public int Id { get; set; } = 0;
//}