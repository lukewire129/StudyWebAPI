namespace IMS.Dto;
public class IdolGroupDto
{
    public int IdolGroupId { get; set; }
    public string GroupName { get; set; }
    public List<MemberDto> Members { get; set; }
}