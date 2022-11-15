using Microsoft.EntityFrameworkCore;


namespace TabpediaFin.Handler.UnitMeasures
{
    public class UnitMeasureList 
    {
        private readonly IUnitMeasureRepository _repository;
        public UnitMeasureList(IUnitMeasureRepository repository)
        {
            _repository = repository;
        }   

        public async Task<UnitMeasureDto> Handle(UnitMeasureDto request, CancellationToken cancellationToken)
        {
            var unit = await _repository.GetAllUnitMeasure();
            return unit;
        }
    }
}
