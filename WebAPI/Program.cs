
using Application.Category;
using Application.ExamResult;
using Application.LogExam;
using Application.Question;
using Application.User;
using Data.EF;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TracNghiemCEVDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("tracnghiemDb")));

// Add services to the container.
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IExamResultService, ExamResultService >();
builder.Services.AddTransient<ILogExamService, LogExamService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



