namespace DMCorp.Framework.Basics.Settings;

public sealed record EmailServiceSettings
{
    public string? OutAddress { get; set; }

    public string? OutAddressDisplayName { get; set; }

    public required string Host { get; set; }

    public int Port { get; set; } = 465;

    public string? Login { get; set; }

    public string? Password { get; set; }
}