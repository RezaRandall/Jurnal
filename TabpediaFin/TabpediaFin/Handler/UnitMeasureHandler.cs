using TabpediaFin.Repository;
using static TabpediaFin.Dto.UnitMeasureDto;
using static TabpediaFin.Repository.UnitMeasureRepository;

namespace TabpediaFin.Handler;

public class UnitMeasureFetchHandler : IQueryByIdHandler<UnitMeasureDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public UnitMeasureFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<UnitMeasureDto>> Handle(QueryByIdDto<UnitMeasureDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<UnitMeasureDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<UnitMeasureDto>(request.Id, _currentUser);

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
    public class GetListUnitMeasureHandler : IRequestHandler<GetUnitMeasureListQuery, List<UnitMeasure>>
    {
        private readonly IUnitMeasureRepository _unitMeasureRepository;
        public GetListUnitMeasureHandler(IUnitMeasureRepository unitMeasureRepository)
        {
            _unitMeasureRepository = unitMeasureRepository;
        }

        public async Task<List<UnitMeasure>> Handle(GetUnitMeasureListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasureRepository.GetUnitMeasureList(request);
            return result;
        }
    }

    public class DeleteUnitMeasureHandler : IRequestHandler<DeleteUnitMeasure, bool>
    {
        private readonly IUnitMeasureRepository _unitMeasureRepository;
        public DeleteUnitMeasureHandler(IUnitMeasureRepository unitMeasureRepository)
        {
            _unitMeasureRepository = unitMeasureRepository;
        }

        public async Task<bool> Handle(DeleteUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasureRepository.DeleteUnitMeasure(request);
            return result;
        }
    }
    public class AddUnitMeasureHandler : IRequestHandler<AddUnitMeasure, UnitMeasure>
    {
        private readonly IUnitMeasureRepository _unitMeasureRepository;
        public AddUnitMeasureHandler(IUnitMeasureRepository unitMeasureRepository)
        {
            _unitMeasureRepository = unitMeasureRepository;
        }

        async Task<UnitMeasure> IRequestHandler<AddUnitMeasure, UnitMeasure>.Handle(AddUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasureRepository.CreateUnitMeasure(request);
            return result;
        }
    }
    public class UpdateUnitMeasureHandler : IRequestHandler<UpdateUnitMeasure, UnitMeasure>
    {
        private readonly IUnitMeasureRepository _unitMeasureRepository;
        public UpdateUnitMeasureHandler(IUnitMeasureRepository unitMeasureRepository)
        {
            _unitMeasureRepository = unitMeasureRepository;
        }

        async Task<UnitMeasure> IRequestHandler<UpdateUnitMeasure, UnitMeasure>.Handle(UpdateUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasureRepository.UpdateUnitMeasure(request);
            return result;
        }
    }




}
