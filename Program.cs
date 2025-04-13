using Microsoft.EntityFrameworkCore;
using Schedule.Data;
using Schedule.Data.Interfaces;
using Schedule.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. ��������� ������ ����������� � DbContext.
builder.Services.AddDbContext<ScheduleDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// 2. ����������� ������������ � DI-����������.
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ILessonGroupRepository, LessonGroupRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();


// 3. ���������� ��������� ������������ � ������������� (MVC).
builder.Services.AddControllersWithViews();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var env = services.GetRequiredService<IHostEnvironment>();
    DBObjects.Initial(services, env);
}

// 4. ������������ middleware.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 5. ��������� ������������� MVC.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
