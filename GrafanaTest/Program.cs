using GrafanaTest;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// add prometheus exporter
builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>

        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GrafanaTest"))
            .AddMeter(builder.Configuration.GetValue<string>("OpenRemoteManageMeterName"))
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                //otlpOptions.Endpoint = new Uri("http://otel-collector:4317");
                otlpOptions.Endpoint = new Uri("http://host.docker.internal:4317");
              
            })
    //.AddOtlpExporter(opts =>
    //{
    //    opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
    //})
    );

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecasts", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecastDetails
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecasts")
.WithOpenApi();

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecastDetails(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
