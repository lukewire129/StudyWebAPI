using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.database.entity;
using Microsoft.EntityFrameworkCore;

namespace IMS.database.context;

public class ImsContext : DbContext
{
    private ILogger<ImsContext> _logger;

    public DbSet<IdolGroup> IdolGroup { get; set; }
    public DbSet<Member> Member { get; set; }
    public ImsContext(DbContextOptions options) : base(options)
    {

    }

    protected ImsContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "ims.db");

        // SQLite 데이터베이스 연결을 설정합니다.
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdolGroup>()
                    .HasMany(ig => ig.Members)
                    .WithMany(m => m.IdolGroups)
                    .UsingEntity<IdolGroupLinkMember>(
                    builderLink => builderLink
                    .HasOne<Member>()
                    .WithMany()
                    .HasForeignKey(iglm => iglm.MemberId),
                    builderLink => builderLink
                    .HasOne<IdolGroup>()
                    .WithMany()
                    .HasForeignKey(iglm => iglm.IdolGroupId),
                builderLink =>
                {
                    builderLink.HasKey(iglm => new { iglm.IdolGroupId, iglm.MemberId }); // 복합 키 설정
                });
    }
}
