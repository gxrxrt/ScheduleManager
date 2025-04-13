using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Schedule.Data;
using Schedule.Data.Interfaces;
using Schedule.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка строки подключения и DbContext с поддержкой Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Добавляем ASP.NET Core Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Пока не требуется подтверждение email
})
.AddEntityFrameworkStores<AppDbContext>();

// 3. Регистрация репозиториев в DI-контейнере
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ILessonGroupRepository, LessonGroupRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

// 4. Добавление MVC + Razor Pages (для Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Инициализация базы (если используешь начальные данные)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var env = services.GetRequiredService<IHostEnvironment>();
    DBObjects.Initial(services, env);
}

// 5. Конфигурация middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Добавлено: middleware для перенаправления на страницу авторизации, если пользователь не авторизован
app.Use(async (context, next) =>
{
    var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
    if (!signInManager.IsSignedIn(context.User) && context.Request.Path != "/Identity/Account/Login" && !context.Request.Path.StartsWithSegments("/Identity"))
    {
        context.Response.Redirect("/Identity/Account/Login");  // Перенаправление на страницу логина
    }
    else
    {
        await next();
    }
});

app.UseAuthorization();

// 6. Настройка маршрутизации
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Подключает Identity UI

app.Run();
