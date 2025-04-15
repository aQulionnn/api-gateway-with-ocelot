namespace Auth.Api.Contracts;

public class EncryptRequest
{
    public string Data { get; set; }
    public string MasterKey { get; set; }
}