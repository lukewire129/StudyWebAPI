using IMS.database.context;
using IMS.database.entity;
using Microsoft.EntityFrameworkCore;

namespace Ims.Api;

public static class MemberApi
{
    public static WebApplication AddMemberEndPoint(this WebApplication app)
    {
        var map = app.MapGroup("member");

        map.MapGet("/", async (ImsContext context) =>
            await context.Member.ToListAsync());

        // 특정 ID 멤버 조회
        map.MapGet("/{id}", async (int id, ImsContext context) =>
        {
            return await context.Member.FindAsync(id)
                is Member member ? Results.Ok(member) : Results.NotFound();
        });

        // 멤버 추가
        map.MapPost("/", async (Member member, ImsContext context) =>
        {
            context.Member.Add(member);
            await context.SaveChangesAsync();

            return Results.Created($"/member/{member.MemberId}", member);
        });

        // 멤버 업데이트
        map.MapPut("/{id}", async (int id, Member updateMember, ImsContext context) =>
        {
            var member = await context.Member.FindAsync(id);

            if (member is null) return Results.NotFound();

            member.MemberName = updateMember.MemberName;
            // 다른 필드 업데이트...

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        // 멤버 삭제
        map.MapDelete("/{id}", async (int id, ImsContext context) =>
        {
            if (await context.Member.FindAsync(id) is Member member)
            {
                context.Member.Remove(member);
                await context.SaveChangesAsync();
                return Results.Ok(member);
            }

            return Results.NotFound();
        });

        return app;
    }
}
