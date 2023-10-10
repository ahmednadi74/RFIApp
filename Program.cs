using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RFIApp.Helper;
using RFIApp.Models;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SupplierContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SupplierFormCon")));
var appSettingsAttachmentSection = builder.Configuration.GetSection("AttachmentSettings");
builder.Services.Configure<AttachmentSettings>(appSettingsAttachmentSection);
builder.Services.AddScoped<AttachmentSettings>();
builder.Services.AddDefaultIdentity<IdentityUser>
    (options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SupplierContext>();
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie(options => options.LogoutPath = "/Identity/Account/Logout");
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LogoutPath = "/Identity/Account/Logout";
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string role = "Administration";
    if (!(await roleManager.RoleExistsAsync(role)))
    {
        await roleManager.CreateAsync(new IdentityRole(role));
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection(); // Enable HTTPS redirection

    app.Use(async (context, next) => {
        if (context.Request.Path == "/") {
            var httpsUrl = "http://" + context.Request.Host + "/Admin/Login";
            context.Response.Redirect(httpsUrl);
        }
        else {
            await next();
        }
    });
}
else {
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Supplier}/{action=Edit}/{id?}");

app.Run();