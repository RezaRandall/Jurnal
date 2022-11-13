using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using TabpediaFin.Infrastructure.Data;
using MediatR;
using TabpediaFin.Infrastructure.Validation;
using TabpediaFin.Infrastructure;
using Microsoft.AspNetCore.Builder;
using TabpediaFin.Infrastructure.OpenApi;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using TabpediaFin.Infrastructure.Migrator;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddDbMigrator();

builder.Services.AddDbContext<FinContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterSwagger("Tabpedia Finance", "v1");

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddCors();

builder.Services.RegisterSettings(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();

builder.Services.AddJwt();

var app = builder.Build();

app.UseMiddlewares();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tabpedia Finance v1"));
// }

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(
    builder =>
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Token-Expired"));

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    TabpediaFin.Infrastructure.Migrator.Startup.UpdateDatabase(scope.ServiceProvider);
}

app.Run();
