using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Bussiness.Concrete;
using ReportMicroService.ContactReports;
using ReportMicroService.DataAccess;
using ReportMicroService.DataAccess.Repositories;
using ReportMicroService.Entities.Repositories;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ReportServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddSwaggerDocument();

builder.Services.AddHostedService<ContactReportBackgroundWorker>();
Thread.Sleep(1000);
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();
app.UseOpenApi();
app.UseSwaggerUi3();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
