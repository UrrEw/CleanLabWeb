using Microsoft.Data.SqlClient;
using LabWeb.Service;
using LabWeb.Secruity;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// 設定 ConfigurationBuilder
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = configurationBuilder.Build();

var AllowSpecific = "_myAllowSpecific";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:AllowSpecific,
        policy =>
        {
            policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
        };
    });

// 設定連線字串 //註冊 IDbConnection
// var connectionString = configuration.GetConnectionString("DB");
// builder.Services.AddSingleton<IDbConnection>(new SqlConnection(connectionString));
//這裡註冊一個實現 IDbConnection 接口的 connectionString 類
//作用愈
var connectionString = configuration.GetConnectionString("DB");
builder.Services.AddSingleton<SqlConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddSingleton<ThesisService>();
builder.Services.AddSingleton<SeniorProjectService>();
builder.Services.AddSingleton<ContestAwardService>();
builder.Services.AddSingleton<AnnouncementService>();
builder.Services.AddSingleton<ProfessorService>();
builder.Services.AddSingleton<MessageBoardService>();
builder.Services.AddSingleton<ReplyService>();
builder.Services.AddSingleton<CarouselService>();
builder.Services.AddSingleton<ActivityService>();
builder.Services.AddSingleton<TestService>();
builder.Services.AddSingleton<ProctorService>();
builder.Services.AddSingleton<ReserveTimeService>();
builder.Services.AddSingleton<TesterService>();
builder.Services.AddSingleton<TestReserveService>();
builder.Services.AddSingleton<MailService>();
builder.Services.AddSingleton<MembersDBService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<GetImageService>();
builder.Services.AddSingleton<GetLoginClaimService>();
builder.Services.AddSingleton<HomeworkService>();
builder.Services.AddSingleton<CheckMemberService>();
builder.Services.AddSingleton<HomeworkCheckService>();
builder.Services.AddSingleton<GetFileService>();
builder.Services.AddSingleton<SeniorProject_MemberService>();
builder.Services.AddSingleton<PagingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors(AllowSpecific);

app.MapControllers();

app.Run();
