using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();

var dbSettingsSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(dbSettingsSection);
var dbSettings = dbSettingsSection.Get<DatabaseSettings>();

if (dbSettings == null)
{
    throw new Exception("Database settings are missing or invalid.");
}

var connectionString =
    $"Host={dbSettings?.Host};Port={dbSettings?.Port};Username={dbSettings?.Username};Password={dbSettings?.Password};Database={dbSettings?.DatabaseName}";

builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();

app.UseCors("AllowFrontendOrigin");

app.MapControllers();

app.Run();
