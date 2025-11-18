using IASServiceClientCore.Configuration.Host;
using IASServiceClientCore.Configuration.Host.Settings;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;

string myCors = "MyCors";
WebApplicationOptions options = new() {
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};
WebApplicationBuilder builder = WebApplication.CreateBuilder(options);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

if(WindowsServiceHelpers.IsWindowsService()) {
    builder.Host.UseWindowsService(options => {
        options.ServiceName = "Servicio IASServiceClientCore";
    });
}

#region Cors
builder.Services.AddCors(o => {
    o.AddPolicy(name: myCors, builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});
#endregion

#region Cargando Settings
builder.Services.Configure<HostSettings>(configuration.GetSection("Settings:Host"));
#endregion

#region Configure Host
IOptions<HostSettings> hostConfig = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<HostSettings>>();
new HostConfiguration(hostConfig).ConfigureWebHost(builder.WebHost, environment);
#endregion

builder.Services.AddControllers().AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
