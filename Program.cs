using FluentValidation;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<GrobsJobsRazorPagesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("GrobsJobsRazorPagesContext") 
    ?? throw new InvalidOperationException("Connection string 'GrobsJobsRazorPagesContext' not found.")
    ), ServiceLifetime.Scoped);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("GrobsJobsRazorPagesContext") 
    ?? throw new InvalidOperationException("Connection string 'GrobsJobsRazorPagesContext' not found.")
    ), ServiceLifetime.Scoped);

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddSignInManager<CustomSignInManager<IdentityUser>>();

builder.Services.AddScoped<UserHandler>();

builder.Services.AddScoped<SearchHandler>();

builder.Services.AddScoped<MessageHandler>();

builder.Services.AddTransient<IValidator<Message>, MessageValidator>();

builder.Services.AddTransient<IValidator<Job>, JobValidator>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();