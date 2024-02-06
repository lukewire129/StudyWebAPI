using IMS.database.context;
using IMS.database.entity;
using IMS.Dto;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ImsContext>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

#region Member C/R/U/D 


app.MapGet("/member", async (ImsContext context) =>
    await context.Member.ToListAsync());

// 특정 ID 멤버 조회
app.MapGet("/member/{id}", async (int id, ImsContext context) =>
{
    return await context.Member.FindAsync(id)
        is Member member ? Results.Ok(member) : Results.NotFound();
});

// 멤버 추가
app.MapPost("/member", async (Member member, ImsContext context) =>
{
    context.Member.Add(member);
    await context.SaveChangesAsync();

    return Results.Created($"/member/{member.MemberId}", member);
});

// 멤버 업데이트
app.MapPut("/member/{id}", async (int id, Member updateMember, ImsContext context) =>
{
    var member = await context.Member.FindAsync(id);

    if (member is null) return Results.NotFound();

    member.MemberName = updateMember.MemberName;
    // 다른 필드 업데이트...

    await context.SaveChangesAsync();

    return Results.NoContent();
});

// 멤버 삭제
app.MapDelete("/member/{id}", async (int id, ImsContext context) =>
{
    if (await context.Member.FindAsync(id) is Member member)
    {
        context.Member.Remove(member);
        await context.SaveChangesAsync();
        return Results.Ok(member);
    }

    return Results.NotFound();
});
#endregion

#region IdolGroup C/R/U/D
// 모든 IdolGroup 조회
app.MapGet("/idolgroup", async (ImsContext context) =>
    await context.IdolGroup.ToListAsync());

// 특정 ID의 IdolGroup 조회
app.MapGet("/idolgroup/{id}", async (int id, ImsContext context) =>
{
    var idolGroup = await context.IdolGroup.FindAsync(id);
    return idolGroup != null ? Results.Ok(idolGroup) : Results.NotFound();
});

// IdolGroup 추가
app.MapPost("/idolgroup", async (IdolGroup idolGroup, ImsContext context) =>
{
    context.IdolGroup.Add(idolGroup);
    await context.SaveChangesAsync();
    return Results.Created($"/idolgroup/{idolGroup.IdolGroupId}", idolGroup);
});

// IdolGroup 업데이트
app.MapPut("/idolgroup/{id}", async (int id, IdolGroup updatedIdolGroup, ImsContext context) =>
{
    var idolGroup = await context.IdolGroup.FindAsync(id);
    if (idolGroup == null)
    {
        return Results.NotFound();
    }

    idolGroup.GroupName = updatedIdolGroup.GroupName;
    // 다른 필드들에 대한 업데이트도 여기에 추가...

    await context.SaveChangesAsync();
    return Results.NoContent();
});

// IdolGroup 삭제
app.MapDelete("/idolgroup/{id}", async (int id, ImsContext context) =>
{
    var idolGroup = await context.IdolGroup.FindAsync(id);
    if (idolGroup == null)
    {
        return Results.NotFound();
    }

    context.IdolGroup.Remove(idolGroup);
    await context.SaveChangesAsync();
    return Results.NoContent();
});


#endregion

#region Link C/R/D

// 모든 IdolGroupLinkMember 조회
app.MapGet("/idolgrouplinkmember", async (ImsContext context) =>
    await context.Set<IdolGroupLinkMember>().ToListAsync());

// IdolGroupLinkMember 추가
app.MapPost("/idolgrouplinkmember", async (IdolGroupLinkMember link, ImsContext context) =>
{
    context.Set<IdolGroupLinkMember>().Add(link);
    await context.SaveChangesAsync();
    return Results.Created($"/idolgrouplinkmember/{link.IdolGroupId}/{link.MemberId}", link);
});

// IdolGroupLinkMember 삭제
app.MapDelete("/idolgrouplinkmember/{idolGroupId}/{memberId}", async (int idolGroupId, int memberId, ImsContext context) =>
{
    var link = await context.Set<IdolGroupLinkMember>().FindAsync(idolGroupId, memberId);
    if (link == null)
    {
        return Results.NotFound();
    }

    context.Set<IdolGroupLinkMember>().Remove(link);
    await context.SaveChangesAsync();
    return Results.NoContent();
});





#endregion


app.MapGet("/groupMembers", async (int groupId, ImsContext context) =>
{
    var groupWithMembers = await context.IdolGroup
        .Where(g => g.IdolGroupId == groupId)
        .Include(g => g.Members)
        .Select(group => new IdolGroupDto
        {
            IdolGroupId = group.IdolGroupId,
            GroupName = group.GroupName,
            Members = group.Members.Select(member => new MemberDto
            {
                MemberId = member.MemberId,
                MemberName = member.MemberName
            }).ToList()
        })
        .FirstOrDefaultAsync();

    return groupWithMembers;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
