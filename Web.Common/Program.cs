using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using Prometheus;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Web.Common.Atributes;
using Web.Common.Data.DataBase;
using Web.Common.Data.DataBase.Abstract;
using Web.Common.Data.Services;
using Web.Common.Data.Services.CrudServices;
using Web.Common.Entity.Entity;
using Web.Common.Handlers;
using Web.Common.ParameterProviders;
using Web.Common.Providers;
using WebUtilities.EventBus.Abstract;
using WebUtilities.Interfaces;
using WebUtilities.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetSection("ConnectionStrings")["DataContext"]));
builder.Services.AddSingleton(new ConnectionFactory()
{
    Uri = new Uri(builder.Configuration.GetSection("ConnectionStrings")["RabbitMQServerConnection"] ?? ""),
    DispatchConsumersAsync = true
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddSingleton<ITransactionContextFactory, TransactionContextFactory>();
builder.Services.AddSingleton(typeof(IRabbitMqProducer<>), typeof(ProducerBase<>));
builder.Services.AddSingleton(typeof(IQueryService<>), typeof(QueryService<>));
builder.Services.AddSingleton<IUserProvider, UserProvider>();
builder.Services.AddSingleton<ICrudService<DemoObject>, DemoObjectCrudService>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"] ?? string.Empty));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true
    };
});
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(new Uri(builder.Configuration.GetSection("ConnectionStrings")["ElasticSearchConnection"] ?? "http://localhost:9200"))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        }).WriteTo.Console().CreateLogger();

builder.Host.UseSerilog();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder.AddPrometheusExporter();

        builder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
        builder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
            });
    });
var mvcBuilder = builder.Services.AddMvc();
mvcBuilder.AddMvcOptions(o => o.Conventions.Add(new GenericRestControllerNameConvention()));
mvcBuilder.ConfigureApplicationPartManager(c =>
{
    c.FeatureProviders.Add(new GenericRestControllerFeatureProvider());
});

var app = builder.Build();
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin());
app.MigrateDatabase();
app.UseMetricServer();
app.UseHttpMetrics();


app.UseMiddleware<ErrorsHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();