using IASServiceClientCore.Configuration.Host.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace IASServiceClientCore.Configuration.Host;
public class HostConfiguration {
    private readonly HostSettings hostSettings;

    public HostConfiguration(IOptions<HostSettings> settings) {
        hostSettings = settings.Value;
    }

    public void ConfigureWebHost(IWebHostBuilder webHostBuilder, IHostEnvironment environment) {
        int port = environment.IsDevelopment() ? hostSettings.Ports.Development : hostSettings.Ports.Production;
        string finalUrl = $"{hostSettings.Url}:{port}";

        webHostBuilder.UseUrls(finalUrl);

        if(environment.IsDevelopment()) {
            OpenSwagger(finalUrl);
        }
    }

    private static void OpenSwagger(string url) {
        try {
            ProcessStartInfo processInfo = new() {
                FileName = $"{url}/swagger/index.html",
                UseShellExecute = true
            };
            Process.Start(processInfo);
        } catch(Exception ex) {
            Console.WriteLine($"Failed to open Swagger: {ex.Message}");
        }
    }
}