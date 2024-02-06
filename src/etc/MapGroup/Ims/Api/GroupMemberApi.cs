using IMS.database.context;
using IMS.Dto;
using Microsoft.EntityFrameworkCore;

namespace Ims;

public static class GroupMemberApi
{
    public static WebApplication AddGroupMemberEndPoint(this WebApplication app)
    {
        var map = app.MapGroup("groupMembers");

        map.MapGet("/groupMembers", async (int groupId, ImsContext context) =>
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

        return app;
    }
}
