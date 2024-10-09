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
        public DbSet<RefreshToken> RefreshTokens { get; set; }  // Thêm RefreshToken

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.FullName);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mentor
            modelBuilder.Entity<Mentor>()
                .HasKey(m => m.MentorId);

            modelBuilder.Entity<Mentor>()
                .HasOne(m => m.User)
                .WithOne(u => u.Mentor)
                .HasForeignKey<Mentor>(m => m.MentorName)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
               .HasOne(s => s.SwpClass)  // Thiết lập quan hệ với SwpClass
               .WithMany(sc => sc.Students)  // Một lớp có nhiều học sinh
               .HasForeignKey(s => s.SwpClassId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.StudentName)
                .OnDelete(DeleteBehavior.Restrict);

            // MentorSkill
            modelBuilder.Entity<MentorSkill>()
                .HasKey(ms => ms.MentorSkillId);

            modelBuilder.Entity<MentorSkill>()
                .HasOne(ms => ms.Skill)
                .WithMany(s => s.MentorSkills)
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // MentorSlot
            modelBuilder.Entity<MentorSlot>()
                .HasKey(ms => ms.MentorSlotId);

            modelBuilder.Entity<MentorSlot>()
                .HasMany(ms => ms.BookingSlots)
                .WithOne(bs => bs.MentorSlot)
                .HasForeignKey(bs => bs.MentorSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookingSlot
            modelBuilder.Entity<BookingSlot>()
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.Group)
                .WithMany(g => g.BookingSlots)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.MentorSlot)
                .WithMany(ms => ms.BookingSlots)
                .HasForeignKey(b => b.MentorSlotId)
                .OnDelete(DeleteBehavior.Restrict);

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
                .WithMany(sc => sc.Groups)
                .HasForeignKey(g => g.SwpClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // SwpClass
            modelBuilder.Entity<SwpClass>()
                .HasKey(sc => sc.SwpClassId);

            modelBuilder.Entity<SwpClass>()
                .HasOne(sc => sc.Mentor)
                .WithMany(m => m.SwpClasses)
                .HasForeignKey(sc => sc.MentorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SwpClass>()
                .HasOne(sc => sc.Semester)
                .WithMany(s => s.SwpClasses)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // WalletTransaction
            modelBuilder.Entity<WalletTransaction>()
                .HasKey(wt => wt.WalletId);

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.BookingSlot)
                .WithMany()
                .HasForeignKey(wt => wt.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
         .HasKey(rt => rt.Id);  // Khóa chính

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()  // Nếu bạn không có thuộc tính collection trong User, thì để trống
                .HasForeignKey(rt => rt.UserName)  // Khóa ngoại
                .HasPrincipalKey(u => u.FullName)  // Khóa chính của User
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
