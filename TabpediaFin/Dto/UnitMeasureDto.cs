namespace TabpediaFin.Dto
{
    public class UnitMeasureDto : BaseDto, IRequest<UnitMeasureDto>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedUid { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int UpdatedUid { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}

public class AuthenticateRequestValidator : AbstractValidator<UnitMeasureDto>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.TenantId).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
        RuleFor(x => x.CreatedUid).NotNull().NotEmpty();
        RuleFor(x => x.CreatedUtc).NotNull().NotEmpty();
        RuleFor(x => x.UpdatedUid).NotNull().NotEmpty();
        RuleFor(x => x.UpdatedUtc).NotNull().NotEmpty();
    }
}
