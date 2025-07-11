namespace Infrastructure.Redis;

public class RedisSettings
{
    public string Host { get; set; } = "localhost";
    public string Port { get; set; } = "6379";
    public int DbIndex { get; set; } = 0;
    public int DefaultExpirationSeconds { get; set; } = 300;
}
