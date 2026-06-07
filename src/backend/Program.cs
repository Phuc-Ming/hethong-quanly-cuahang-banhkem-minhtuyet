using MinhTuyetBakery.DuLieu;
using MinhTuyetBakery.KhoXuLy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<KetNoiCSDL>();
builder.Services.AddScoped<XuLyCuaHang>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DangNhap}/{action=Index}/{id?}");

app.Run();
