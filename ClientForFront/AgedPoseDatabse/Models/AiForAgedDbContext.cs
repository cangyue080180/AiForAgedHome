using Microsoft.EntityFrameworkCore;

namespace AgedPoseDatabse.Models
{
    public class AiForAgedDbContext : DbContext
    {
        public AiForAgedDbContext(DbContextOptions<AiForAgedDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PoseInfo>().HasKey(c => new { c.AgesInfoId, c.Date });
           // modelBuilder.Entity<PoseInfo>().Property(x => x.IsAlarm).HasDefaultValue(false);
            modelBuilder.Entity<ServerInfo>().HasIndex(x => x.Ip).IsUnique();
            modelBuilder.Entity<ServerInfo>().HasIndex(x => x.Name).IsUnique();
        }

        public DbSet<RoomInfo> RoomInfos { get; set; }
        public DbSet<AgesInfo> AgesInfos { get; set; }
        public DbSet<PoseInfo> PoseInfos { get; set; }
        public DbSet<CameraInfo> CameraInfos { get; set; }
        public DbSet<ServerInfo> ServerInfos { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
    }
}
