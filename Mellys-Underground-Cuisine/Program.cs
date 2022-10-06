using DAL.Context;
using DAL.DbInitializer;
using DAL.Entities;
using Hangfire;
using Hangfire.SqlServer;
using Mellys_Underground_Cuisine.Profiles;
using Mellys_Underground_Cuisine.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// create a dbstring to add to sqlserver
var dbstring = builder.Configuration.GetConnectionString("DBInfo");

// adding the dbstring to the server
builder.Services.AddSqlServer<AppDbContext>(dbstring);

// add the services for identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//fix your forward to login page
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = "/Accounts/AccessDenied";
    opt.LoginPath = "/Accounts/Login";
});

// add initializer
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// add automapper 
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Add services to the container.
builder.Services.AddControllersWithViews();

// Using an interface for hangfire Services TimeServices
builder.Services.AddTransient<ITimeService, TimeService>();

// Using Hangfire
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(dbstring, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//make the dbinitializer work
var scope = app.Services.CreateScope();
var dbInit = scope.ServiceProvider.GetService<IDbInitializer>();
dbInit.InitializeAsync();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ITimeService>("print-time", service => service.DeleteOldMenus(),Cron.Daily);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
