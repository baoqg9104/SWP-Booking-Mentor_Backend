using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;

namespace SWP391_Mentor_Booking_System_Data
{
    public class SWP391_Mentor_Booking_System_DBContext : DbContext
    {
        public SWP391_Mentor_Booking_System_DBContext(DbContextOptions<SWP391_Mentor_Booking_System_DBContext> options)
            : base(options)
        {
        }

        public DbSet<BookingSlot> BookingSlots { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MentorSkill> MentorSkills { get; set; }
        public DbSet<MentorSlot> MentorSlots { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SwpClass> SwpClasses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BookingSlot
            modelBuilder.Entity<BookingSlot>()
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.Group)
                .WithMany(g => g.BookingSlots)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade delete for Group

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.MentorSlot)
                .WithMany()
                .HasForeignKey(b => b.SlotId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction for MentorSlot

            // Group
            modelBuilder.Entity<Group>()
                .HasKey(g => g.GroupId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Topic)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.SwpClass)
                .WithMany(s => s.Groups)
                .HasForeignKey(g => g.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mentor
            modelBuilder.Entity<Mentor>()
                .HasKey(m => m.UserId);

            modelBuilder.Entity<Mentor>()
                .HasMany(m => m.MentorSkills)
                .WithOne(ms => ms.Mentor)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mentor>()
                .HasMany(m => m.MentorSlots)
                .WithOne(ms => ms.Mentor)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            // MentorSkill
            modelBuilder.Entity<MentorSkill>()
                .HasKey(ms => ms.MentorSkillId);

            modelBuilder.Entity<MentorSkill>()
                .HasOne(ms => ms.Skill)
                .WithMany()
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // MentorSlot
            modelBuilder.Entity<MentorSlot>()
                .HasKey(ms => ms.SlotId);

            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Semester
            modelBuilder.Entity<Semester>()
                .HasKey(s => s.SemesterId);

            modelBuilder.Entity<Semester>()
                .HasMany(s => s.SwpClasses)
                .WithOne(sc => sc.Semester)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Semester>()
                .HasMany(s => s.Topics)
                .WithOne(t => t.Semester)
                .HasForeignKey(t => t.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Skill
            modelBuilder.Entity<Skill>()
                .HasKey(s => s.SkillId);

            // Student
            modelBuilder.Entity<Student>()
                .HasKey(s => new { s.UserId, s.GroupId });

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // SwpClass
            modelBuilder.Entity<SwpClass>()
                .HasKey(sc => sc.ClassId);

            modelBuilder.Entity<SwpClass>()
                .HasOne(sc => sc.Mentor)
                .WithMany(m => m.SwpClasses)
                .HasForeignKey(sc => sc.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SwpClass>()
                .HasOne(sc => sc.Semester)
                .WithMany(s => s.SwpClasses)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Topic
            modelBuilder.Entity<Topic>()
                .HasKey(t => t.TopicId);

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Semester)
                .WithMany(s => s.Topics)
                .HasForeignKey(t => t.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // WalletTransaction
            modelBuilder.Entity<WalletTransaction>()
                .HasKey(wt => new { wt.UserId, wt.BookingId });

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.BookingSlot)
                .WithMany(bs => bs.WalletTransactions)
                .HasForeignKey(wt => wt.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
