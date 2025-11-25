using Library.Common;
using Library.Infrastructure;
using Library.Infrastructure.Identity;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Library.REST;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===== DbContext =====
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(@"Data Source=library.db"));

// ===== Identity =====
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LibraryContext>()
    .AddDefaultTokenProviders();

// ===== JWT Authentication =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("13579"))
    };
});

// ===== Controllers + JSON Options (обробка циклів) =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== DI для репозиторіїв та сервісів =====
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// ===== Seed ролей та користувачів =====
using (var scope = app.Services.CreateScope())
{
    await IdentityInitializer.SeedRolesAndUsersAsync(scope.ServiceProvider);
}

// ===== Middleware =====
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
