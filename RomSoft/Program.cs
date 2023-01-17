using Business.Repositories.Contracts;
using Business.Services;
using Business.Services.Contracts;
using DataAccess.Repositories;
using Models;
using RomSoft.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IArchiveService, ArchiveService>();
builder.Services.AddTransient<IArchivingLogsService, ArchivingLogsService>();
builder.Services.AddTransient<IArchivingLogsRepository, ArchivingLogsRepository>();
builder.Services.AddScoped<IGenericRepository<ArchivingLogs>, GenericRepository<ArchivingLogs>>();

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<FileUploadedHub>("/fileUploadedHub");
app.Run();
