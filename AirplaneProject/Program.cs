using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Database;
using AirplaneProject.Database.DatabaseContextes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddDbContext<OrderDbContext>();
builder.Services.AddScoped<UserInteractor>();
//TODO: проверить что тут нужно
//builder.Services.AddDefaultIdentity<SignInManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtToken.TokenValidationParameters;
    });

var app = builder.Build();

// Верю, что это мб и плохо, но от разбора авторизации меня уже мутит
app.Use(async (context, next) =>
{
    var JWToken = context.Request.Cookies.FirstOrDefault(c => c.Key == "authToken").Value;
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();