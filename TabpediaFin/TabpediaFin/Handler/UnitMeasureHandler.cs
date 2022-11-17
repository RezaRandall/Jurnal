using TabpediaFin.Repository;
using static TabpediaFin.Dto.UnitMeasureDto;
using static TabpediaFin.Repository.UnitMeasureRepository;

namespace TabpediaFin.Handler;

//public class UnitMeasurehandler
//{
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

public class GetUnitMeasureHandler : IRequestHandler<GetUnitMeasureQuery, UnitMeasure>
{
    private readonly IUnitMeasureRepository _unitMeasureRepository;
    public GetUnitMeasureHandler(IUnitMeasureRepository unitMeasureRepository)
    {
        _unitMeasureRepository = unitMeasureRepository;
    }

    public async Task<UnitMeasure> Handle(GetUnitMeasureQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitMeasureRepository.GetUnitMeasureById(request);
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




//}
