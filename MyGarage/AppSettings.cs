namespace MyGarage;

public class AppSettings
{
    public string JWTSecretKey { get; set; }
    public string AWSAccessKey { get; set; }
    public string AWSSecretKey { get; set; }
    public string AWSBucket { get; set; }
    public string[] AllowedCORSOrignis { get; set; }
}