namespace Auth.Api.Contracts;

public class DecryptRequest
{
    public string EncryptedData { get; set; }
    public string MasterKey { get; set; }
}