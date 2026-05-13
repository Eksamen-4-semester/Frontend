namespace FitLife.Models.User.ResultObjects;

public class ActivationResult
{
    public ActivationResultType ResultType { get; set; }
    public string? ErrorMessage { get; set; }
}

public enum ActivationResultType
{
    Success,
    NotFound,
    InternalError,
    NoChange
}