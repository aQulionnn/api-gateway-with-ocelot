namespace Auth.Api.Contracts;

public class DecryptRequest
{
    public string EncryptedData { get; set; }
    public string Key { get; set; }
    public string IV { get; set; }
}