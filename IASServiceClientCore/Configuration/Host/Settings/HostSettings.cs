namespace IASServiceClientCore.Configuration.Host.Settings;
public class HostSettings {
    public string Url { get; set; } = string.Empty;
    public HostPorts Ports { get; set; } = new();
}

public class HostPorts {
    public int Development { get; set; }
    public int Production { get; set; }
}
