using IMS.database.context;
using IMS.database.entity;
using Microsoft.EntityFrameworkCore;

namespace Ims.Api;

public static class IdolGroupLinkApi
{
    public static WebApplication AddIdolGroupLinkEndPoint(this WebApplication app)
    {
        var map = app.MapGroup("idolgrouplinkmember");

        // 모든 IdolGroupLinkMember 조회
        map.MapGet("/idolgrouplinkmember", async (ImsContext context) =>
            await context.Set<IdolGroupLinkMember>().ToListAsync());

        // IdolGroupLinkMember 추가
        map.MapPost("/idolgrouplinkmember", async (IdolGroupLinkMember link, ImsContext context) =>
        {
            context.Set<IdolGroupLinkMember>().Add(link);
            await context.SaveChangesAsync();
            return Results.Created($"/idolgrouplinkmember/{link.IdolGroupId}/{link.MemberId}", link);
        });

        // IdolGroupLinkMember 삭제
        map.MapDelete("/idolgrouplinkmember/{idolGroupId}/{memberId}", async (int idolGroupId, int memberId, ImsContext context) =>
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

        return app;
    }
}
