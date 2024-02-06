using Ims;
using Ims.Api;
using IMS.database.context;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ImsContext>();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // 기본 로깅 프로바이더를 지웁니다.
    loggingBuilder.SetMinimumLevel(LogLevel.Trace); // 로그 레벨 설정
    loggingBuilder.AddNLog(); // NLog를 사용하도록 설정
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddMemberEndPoint()
   .AddIdolGroupEndPoint()
   .AddIdolGroupLinkEndPoint()
   .AddGroupMemberEndPoint()
   .Run();