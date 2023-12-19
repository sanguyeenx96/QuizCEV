using Microsoft.AspNetCore.Authentication.Cookies;
using WebAPP.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Login/Login";
                   options.AccessDeniedPath = "/User/Forbidden/";
               });

builder.Services.AddAuthorization(options => {
    options.AddPolicy("userpolicy",
        builder => builder.RequireRole("user"));
    options.AddPolicy("adminpolicy",
        builder => builder.RequireRole("admin"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IQuestionApiClient, QuestionApiClient>();
builder.Services.AddTransient<ICauHoiTuLuanApiClient, CauHoiTuLuanApiClient>();
builder.Services.AddTransient<ICauHoiTrinhTuThaoTacApiClient, CauHoiTrinhTuThaoTacApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IDeptApiClient, DeptApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<IExamResultApiClient, ExamResultApiClient>();
builder.Services.AddTransient<ILogExamApiClient, LogExamApiClient>();
builder.Services.AddTransient<ILogExamTrinhtuthaotacApiClient, LogExamTrinhtuthaotacApiClient>();
builder.Services.AddScoped<ISettingApiClient, SettingApiClient>();

builder.Services.AddTransient<IDiemChuYApiClient, DiemChuYApiClient>();
builder.Services.AddTransient<ILoiTaiCongDoanApiClient, LoiTaiCongDoanApiClient>();
builder.Services.AddTransient<IDoiSachApiClient, DoiSachApiClient>();

builder.Services.AddTransient<ILogExamDCYApiClient, LogExamDCYApiClient>();
builder.Services.AddTransient<ILogExamLTCDApiClient, LogExamLTCDApiClient>();
builder.Services.AddTransient<ILogExamDSApiClient, LogExamDSApiClient>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");
app.Run();