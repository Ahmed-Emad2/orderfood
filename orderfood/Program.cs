using orderfood.Models; // åĞÇ ÇáÓØÑ íÍá ÇáãÔßáÉ İæÑÇğ æíÌÚá ÇáÓíÇŞ ãÑÆíÇğ áãáİ Program
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 1. ÑÈØ ŞÇÚÏÉ ÇáÈíÇäÇÊ ÈÇáÓíÇŞ
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 2. ÇáÃãÑ ÇáÓÍÑí ÇáÈÏíá ááÜ Migration áÊæáíÏ ŞÇÚÏÉ ÇáÈíÇäÇÊ ÊáŞÇÆíÇğ İæÑÇğ
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); // íŞæã ÈÅäÔÇÁ ŞÇÚÏÉ ÇáÈíÇäÇÊ æÇáÌÏÇæá ÊáŞÇÆíÇğ ÚäÏ ÊÔÛíá ÇáãæŞÚ!
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ÌÚá ÕİÍÉ ÇáæÌÈÇÊ (ÇáÊí ÓäŞæã ÈÚãá ÇáÜ CRUD áåÇ) åí ÇáÕİÍÉ ÇáÑÆíÓíÉ ááãæŞÚ
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FoodItems}/{action=Index}/{id?}");

app.Run();