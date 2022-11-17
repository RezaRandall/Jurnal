namespace TabpediaFin.Dto.Base;

public class BaseDto<TId>
{
    public TId Id { get; set; }
}

public class BaseDto : BaseDto<int>
{
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
