using Application.Category;
using Application.CauHoiTrinhTuThaoTac;
using Application.CauHoiTrinhTuThaoTac.DiemChuY;
using Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using Application.CauHoiTuLuan;
using Application.Dept;
using Application.ExamResult;
using Application.LogExam;
using Application.LogExamDiemChuY;
using Application.LogExamDoiSach;
using Application.LogExamLoiTaiCongDoan;
using Application.LogExamTrinhtuthaotac;
using Application.Question;
using Application.Role;
using Application.Setting;
using Application.Users;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Utilities.Constants;

var builder = WebApplication.CreateBuilder(args);

//Cấu Hình Database và Identity:
//Thêm dịch vụ DbContext để sử dụng Entity Framework Core với cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<TracNghiemCEVDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

//Thêm dịch vụ Identity cho việc quản lý người dùng và vai trò.
builder.Services.AddIdentity<AppUser,AppRole>()
    .AddEntityFrameworkStores<TracNghiemCEVDbContext>()
    .AddDefaultTokenProviders();

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
});

//Dependency Injection:
//Đăng ký các dịch vụ ứng dụng (Category, Question, CauHoiTuLuan, CauHoiTrinhTuThaoTac, ExamResult, LogExam, Users).
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<ICauHoiTuLuanService, CauHoiTuLuanService>();
builder.Services.AddTransient<ICauHoiTrinhTuThaoTacService, CauHoiTrinhTuThaoTacService>();
builder.Services.AddTransient<IExamResultService, ExamResultService>();
builder.Services.AddTransient<ILogExamService, LogExamService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IDeptService, DeptService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ILogExamTrinhtuthaotacService, LogExamTrinhtuthaotacService>();
builder.Services.AddScoped<ISettingService, SettingSevice>();

builder.Services.AddTransient<IDiemChuYService, DiemChuYService>();
builder.Services.AddTransient<ILoiTaiCongDoanService, LoiTaiCongDoanService>();
builder.Services.AddTransient<IDoiSachService, DoiSachService>();

builder.Services.AddTransient<ILogExamDiemChuYService, LogExamDiemChuYService>();
builder.Services.AddTransient<ILogExamLoiTaiCongDoanService, LogExamLoiTaiCongDoanService>();
builder.Services.AddTransient<ILogExamDoiSachService, LogExamDoiSachService>();



builder.Services.AddControllers();

//Swagger
//Sử dụng Swagger để tạo và hiển thị tài liệu API.
builder.Services.AddSwaggerGen(c =>
{
    //Cấu hình thông tin API và định nghĩa chú thích
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eExam CEV", Version = "v1" });

    // Cấu hình xác thực JWT trong Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //chỉ định yêu cầu bảo mật cho các tài nguyên API.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme, //Xác định loại tham chiếu, ở đây là SecurityScheme.
                        Id = "Bearer" //Một chuỗi định danh cho Security Scheme, ở đây là "Bearer"
                    },
                    Scheme = "oauth2", //Chỉ định loại xác thực được sử dụng. Ở đây, bạn đang sử dụng oauth2.
                    Name = "Bearer", //Xác định tên của scheme xác thực. Trong trường hợp này, bạn đang sử dụng Bearer.
                    In = ParameterLocation.Header, //Chỉ định nơi mà thông tin xác thực được truyền. Ở đây, thông tin xác thực được truyền trong phần Header của HTTP request.
                },
                new List<string>()
        }
    });
});

//Cấu Hình Xác Thực JWT

//Sử dụng JWT để xác thực người dùng.
//Lấy Thông Tin Cấu Hình:
//Đoạn mã này sử dụng Configuration để đọc giá trị của "Issuer" và "Key" từ tệp cấu hình.
//Issuer thường là địa chỉ phát hành token và Key là khóa bí mật được sử dụng để ký và xác minh token.
string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
//Chuyển Đổi Khóa Ký Thành Mảng Byte:
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

//Thêm dịch vụ xác thực vào ứng dụng.
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => //Thêm xác thực JWT Bearer vào dịch vụ xác thực.
{
    options.RequireHttpsMetadata = false; //Xác định liệu cần yêu cầu sử dụng HTTPS hay không.
    options.SaveToken = true; //Xác định liệu token sẽ được lưu trong request chuyển tiếp hay không.
    options.TokenValidationParameters = new TokenValidationParameters() //Cấu hình các tham số xác thực của token.
    {
        //Xác định liệu issuer, audience, và thời gian sống của token có được kiểm tra không, Các giá trị hợp lệ của issuer và audience.
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //Xác định liệu khóa ký có được kiểm tra không.
        ClockSkew = System.TimeSpan.Zero, //Độ chệch đồng hồ cho phép khi kiểm tra thời gian hết hạn của token. Ở đây đặt là zero nghĩa là không chấp nhận độ chệch.
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes) //Khóa ký được sử dụng để xác minh chữ ký của token.
    };
});

//Cấu Hình Endpoint và Swagger UI:

///Cấu hình endpoint cho API và hiển thị Swagger UI.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();//Xây dựng ứng dụng từ WebApplication được khởi tạo.

// Configure the HTTP request pipeline.
// Khi ở môi trường phát triển, sử dụng Swagger và Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eExam V1");
});

app.UseHttpsRedirection(); //Middleware này chuyển hướng các yêu cầu HTTP sang HTTPS, đảm bảo rằng các yêu cầu được thực hiện qua kênh an toàn.

//Middleware xác thực (UseAuthentication()) và xác định quyền (UseAuthorization()).
//Điều này cần thiết khi bạn sử dụng xác thực JWT trong ứng dụng của mình.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();//Ánh xạ các controllers trong ứng dụng. Điều này giúp định tuyến các yêu cầu HTTP đến các action trong controllers tương ứng.

app.Run();//Kết thúc xử lý HTTP request pipeline.



