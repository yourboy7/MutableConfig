namespace MutableConfig.Sample.Models;

public class DatabaseConfig {
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string DatabaseName { get; set; }
    public int ConnectionTimeoutSeconds { get; set; }
}