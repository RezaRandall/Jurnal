using static TabpediaFin.Dto.UnitMeasureDto;

namespace TabpediaFin.Handler;

public class PaymentTermFetchHandler : IQueryByIdHandler<PaymentTermDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentTermFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<PaymentTermDto>> Handle(QueryByIdDto<PaymentTermDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<PaymentTermDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<PaymentTermDto>(request.Id, _currentUser);

                response.IsOk = true;
                response.Row = row;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.Row = null;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

    public class GetListPaymentTermHandler : IRequestHandler<GetPaymentTermListQuery, List<PaymentTerm>>
    {
        private readonly IPaymentTermRepository _paymentTermRepository;
        public GetListPaymentTermHandler(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }

        public async Task<List<PaymentTerm>> Handle(GetPaymentTermListQuery request, CancellationToken cancellationToken)
        {
            var result = await _paymentTermRepository.GetPaymentTermList(request);
            return result;
        }
    }

    //public class GetPaymentTermHandler : IRequestHandler<GetPaymentTermQuery, PaymentTerm>
    //{
    //    private readonly IPaymentTermRepository _paymentTermRepository;
    //    public GetPaymentTermHandler(IPaymentTermRepository paymentTermRepository)
    //    {
    //        _paymentTermRepository = paymentTermRepository;
    //    }

    //    public async Task<PaymentTerm> Handle(GetPaymentTermQuery request, CancellationToken cancellationToken)
    //    {
    //        var result = await _paymentTermRepository.GetPaymentTermById(request);
    //        return result;
    //    }
    //}

    public class DeletePaymentTermHandler : IRequestHandler<DeletePaymentTerm, bool>
    {
        private readonly IPaymentTermRepository _paymentTermRepository;
        public DeletePaymentTermHandler(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }

        public async Task<bool> Handle(DeletePaymentTerm request, CancellationToken cancellationToken)
        {
            var result = await _paymentTermRepository.DeletePaymentTerm(request);
            return result;
        }
    }
    public class AddPaymentTermHandler : IRequestHandler<AddPaymentTerm, PaymentTerm>
    {
        private readonly IPaymentTermRepository _paymentTermRepository;
        public AddPaymentTermHandler(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }

        async Task<PaymentTerm> IRequestHandler<AddPaymentTerm, PaymentTerm>.Handle(AddPaymentTerm request, CancellationToken cancellationToken)
        {
            var result = await _paymentTermRepository.CreatePaymentTerm(request);
            return result;
        }
    }
    public class UpdatePaymentTermHandler : IRequestHandler<UpdatePaymentTerm, PaymentTerm>
    {
        private readonly IPaymentTermRepository _paymentTermRepository;
        public UpdatePaymentTermHandler(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }

        async Task<PaymentTerm> IRequestHandler<UpdatePaymentTerm, PaymentTerm>.Handle(UpdatePaymentTerm request, CancellationToken cancellationToken)
        {
            var result = await _paymentTermRepository.UpdatePaymentTerm(request);
            return result;
        }
    }
}
