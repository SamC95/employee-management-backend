using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Calculators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IPayslipService, PayslipService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IPayslipRepository, PayslipRepository>();

builder.Services.AddSingleton<PayslipCalculator>();

var dbSettingsSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(dbSettingsSection);
var dbSettings = dbSettingsSection.Get<DatabaseSettings>();

if (dbSettings == null)
{
    throw new Exception("Database settings are missing or invalid.");
}

var connectionString =
    $"Host={dbSettings?.Host};Port={dbSettings?.Port};Username={dbSettings?.Username};Password={dbSettings?.Password};Database={dbSettings?.DatabaseName}";

builder.Services.AddDbContext<AnnouncementDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<ShiftDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<HolidayDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<PayslipDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
    
    options.AddPolicy("AllowLocalHosts", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();

app.UseCors("AllowLocalHosts");

app.MapControllers();

app.Run();
