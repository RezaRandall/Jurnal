using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using TabpediaFin.Infrastructure;
using TabpediaFin.Infrastructure.Migrator;
using TabpediaFin.Infrastructure.OpenApi;
using TabpediaFin.Infrastructure.Validation;
using TabpediaFin.Infrastructure.Worker;

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

builder.Services.AddCaching();

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
    })
    .AddFluentValidation(cfg => 
    { 
        cfg.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
        cfg.AutomaticValidationEnabled = true;
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllAllowPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Token-Expired")
        );
});

builder.Services.RegisterSettings(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();


builder.Services.AddScoped<IDbConnection>(db => new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddJwt();

builder.Services.AddWorker();

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

app.UseCors("AllAllowPolicy");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "UserUpload")),
    RequestPath = "/UserUpload",
    EnableDefaultFiles = true
});

using (var scope = app.Services.CreateScope())
{
    TabpediaFin.Infrastructure.Migrator.Startup.UpdateDatabase(scope.ServiceProvider);
}

app.Run();
