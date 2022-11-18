namespace TabpediaFin.Dto.Common.Response;

public class BaseResponse
{
    public bool IsOk { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

}
