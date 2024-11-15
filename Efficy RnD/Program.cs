using Microsoft.EntityFrameworkCore;
using EfficyRnD.Models;
using EfficyRnD.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();

builder.Services.AddControllers(); 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{ 
	c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger(); 
	app.UseSwaggerUI(c => {
							c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); 
							c.RoutePrefix = string.Empty; // Ustawienie Swagger UI na stronie g³ównej
						}); 
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();
//app.MapRazorPages()
//   .WithStaticAssets();

app.Run();
