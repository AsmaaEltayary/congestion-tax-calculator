using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using congestion_tax_calculator.Web.Middleware;
using Congestion_tax_calculator.AppDomain;
using Congestion_tax_calculator.Infrastructure;
using Congestion_tax_calculator.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddTransient<GlobalExceptionHandling>();

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true; 
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
    setupAction.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("X-Version"));
}).AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'V";
    o.SubstituteApiVersionInUrl = true;

});

var apiversiondescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

 builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlcommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlfilefullpath = Path.Combine(AppContext.BaseDirectory, xmlcommentsFile);
    setupAction.IncludeXmlComments(xmlfilefullpath);
    setupAction.EnableAnnotations();   
    foreach (var descrption in apiversiondescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc($"{descrption.GroupName}", new()
        {

            Title = "Toll Fee Calculator",
            Version = descrption.ApiVersion.ToString(),
            Description = descrption.ApiVersion.ToString() == "2.0" ? "Calculate Toll Fees For Vehicles Using External Data Store" : "Calculate Toll Fees for Vehicles Without using DB "

        });
    }

});

builder.Services.AddKeyedScoped<ICityTollRulesReader, GothenburgTollRulesReader>("NonDBTollFeeRulesReader"); 
builder.Services.AddKeyedScoped<ICityTollRulesReader, TollRulesDBReader>("DBTollFeeRuesReader");
builder.Services.AddScoped<CongestionTaxCalculator>();

builder.Services.AddDbContext<TollRulesContext>(optionsa => optionsa.UseSqlServer(builder.Configuration["ConnectionStrings:Tollfeecontext"]).EnableSensitiveDataLogging());

var app = builder.Build();

 app.UseSwagger();

 app.UseSwaggerUI(setupactions =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var desc in descriptions)
        { 
            setupactions.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }

    });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionHandling>();

app.Run();
