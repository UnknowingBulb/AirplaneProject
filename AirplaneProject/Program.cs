using AiplaneProject.Models;
using AirplaneProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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
// добавление данных
using (ApplicationContext db = new ApplicationContext())
{
    // создаем два объекта User
    var user1 = new ClientUser { Name = "Tom", Id = Guid.NewGuid(), Login = "Tom", PhoneNumber = "+79999999999", Password = "123456"};
    var user2 = new ClientUser { Name = "Alice", Id = Guid.NewGuid(), Login = "Alice", PhoneNumber = "+79999999998", Password = "123456" };

    // добавляем их в бд
    db.Users.AddRange(user1, user2);
    db.SaveChanges();
}
// получение данных
using (ApplicationContext db = new ApplicationContext())
{
    // получаем объекты из бд и выводим на консоль
    var users = db.Users.ToList();
    Console.WriteLine("Users list:");
    foreach (ClientUser u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.PhoneNumber}");
    }
}
app.Run();
