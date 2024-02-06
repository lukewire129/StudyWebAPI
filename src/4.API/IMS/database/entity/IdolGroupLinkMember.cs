using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.database.entity;
public class IdolGroupLinkMember
{
    [Key]
    public int IdolGroupId { get; set; }
    [Key]
    public int MemberId { get; set; }
}
