using AirplaneProject.Authorization;
using AirplaneProject.Database;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<UserInteractor>();
builder.Services.AddScoped<FlightInteractor>();
builder.Services.AddScoped<OrderInteractor>();
builder.Services.AddScoped<PassengerInteractor>();

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

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode >= 400)
    {
        response.Redirect($"/LoadErrorPage?statusCode={response.StatusCode}");
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();