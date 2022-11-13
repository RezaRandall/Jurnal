namespace TabpediaFin.Dto.Base;

public class BaseDto
{
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
