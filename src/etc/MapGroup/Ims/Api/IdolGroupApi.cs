using IMS.database.context;
using IMS.database.entity;
using Microsoft.EntityFrameworkCore;

namespace Ims.Api;

public static class IdolGroupApi
{
    public static WebApplication AddIdolGroupEndPoint(this WebApplication app)
    {
        var map = app.MapGroup("idolgroup");

        map.MapGet("/", async (ImsContext context) =>
            await context.IdolGroup.ToListAsync());

        // 특정 ID의 IdolGroup 조회
        map.MapGet("/{id}", async (int id, ImsContext context) =>
        {
            var idolGroup = await context.IdolGroup.FindAsync(id);
            return idolGroup != null ? Results.Ok(idolGroup) : Results.NotFound();
        });

        // IdolGroup 추가
        map.MapPost("/", async (IdolGroup idolGroup, ImsContext context) =>
        {
            context.IdolGroup.Add(idolGroup);
            await context.SaveChangesAsync();
            return Results.Created($"/idolgroup/{idolGroup.IdolGroupId}", idolGroup);
        });

        // IdolGroup 업데이트
        map.MapPut("/{id}", async (int id, IdolGroup updatedIdolGroup, ImsContext context) =>
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
        map.MapDelete("/{id}", async (int id, ImsContext context) =>
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

        return app;
    }
}
