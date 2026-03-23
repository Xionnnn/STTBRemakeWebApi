using STTB.WebApiStandard.Commons.Behaviors;
using STTB.WebApiStandard.Commons.Authorizations;
using STTB.WebApiStandard.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using STTB.WebApiStandard.RequestHandlers.Web.News;
using STTB.WebApiStandard.Validators.Web.News;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Information()
      .WriteTo.File(
          path: "logs/Log-.txt",
          rollingInterval: RollingInterval.Day,
          shared: true);
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// MediatR (scan assembly Application)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAvailableNewsHandler>());

// FluentValidation (scan assembly Application)
builder.Services.AddValidatorsFromAssemblyContaining<GetAvailableNewsValidator>();

// FluentValidation via MediatR pipeline behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageAcademics", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageAcademics")));

    options.AddPolicy("CanManageNews", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageNews")));

    options.AddPolicy("CanManageEvents", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageEvents")));

    options.AddPolicy("CanManageMedia", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageMedia")));

    options.AddPolicy("CanManageUsers", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageUsers")));

    options.AddPolicy("CanManageLecturers", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageLecturers")));

    options.AddPolicy("CanManageAdmissionCost", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageAdmissionCost")));

    options.AddPolicy("CanManageAdministrator", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanManageAdministrator")));
});

// Authorization Handlers
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminBypassHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Atau pake policy spesifik untuk production:
    // options.AddPolicy("AllowSpecificOrigins", policy =>
    // {
    //     policy.WithOrigins("https://yourdomain.com", "http://localhost:3000")
    //           .AllowAnyMethod()
    //           .AllowAnyHeader()
    //           .AllowCredentials();
    // });
});

//Db Connection
var connectionString = builder.Configuration.GetConnectionString("PgSQLDB");

builder.Services.AddDbContext<SttbDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Local Storage / Static Files
app.UseStaticFiles();

// Error Compliant
app.UseExceptionHandler("/error");

// Enable CORS - harus sebelum UseAuthorization
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
