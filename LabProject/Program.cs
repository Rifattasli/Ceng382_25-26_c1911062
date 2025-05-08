using LabProject.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();

// 💾 DbContext servisi
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();

    if (!context.Classes.Any())
    {
        for (int i = 1; i <= 100; i++)
        {
            context.Classes.Add(new LabProject.Models.Class
            {
                Name = $"Class {i}",
                PersonCount = 20 + (i % 10),
                Description = $"This is a sample description for class {i}.",
                IsActive = i % 2 == 0
            });
        }

        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();
app.Run();