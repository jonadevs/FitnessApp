using System.Reflection;
using System.Text;
using FitnessApp.API;
using FitnessApp.API.DbContexts;
using FitnessApp.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/fitnessapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

//builder.Services.AddControllers(options =>
//{
//    options.ReturnHttpNotAcceptable = true;
//}).AddXmlDataContractSerializerFormatters();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlCommentsFile));

    swaggerGenOptions.AddSecurityDefinition("FitnessAppApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference() {
                    Type=ReferenceType.SecurityScheme,
                    Id="FitnessAppApiBearerAuth" }
            }, new List<string>() }
    });
});
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<WorkoutsDataStore>();

// policy test
builder.Services.AddDbContext<FitnessAppContext>(
    dbContextOptionsBuilder => dbContextOptionsBuilder.UseSqlite(
        builder.Configuration["ConnectionStrings:FitnessAppDBConnectionString"]
    )
);

builder.Services.AddScoped<IFitnessAppRepository, FitnessAppRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer").AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
});

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("MustBeFromBerlin", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireAuthenticatedUser();
        authorizationPolicyBuilder.RequireClaim("city", "Berlin");
    });
});

builder.Services.AddApiVersioning(apiVersioningOptions =>
{
    apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
    apiVersioningOptions.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    apiVersioningOptions.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
