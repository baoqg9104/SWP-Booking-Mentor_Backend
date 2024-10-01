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
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Mentor)
                .WithOne(m => m.User)
                .HasForeignKey<Mentor>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Chuyển từ Cascade sang Restrict

            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Chuyển từ Cascade sang Restrict

            // Mentor
            modelBuilder.Entity<Mentor>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Mentor>()
                .HasOne(m => m.User)
                .WithOne(u => u.Mentor)
                .HasForeignKey<Mentor>(m => m.UserId)  
                .OnDelete(DeleteBehavior.Restrict);  // Thay đổi hành vi xóa tùy theo yêu cầu

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
                .HasKey(ms => ms.Id);

            modelBuilder.Entity<MentorSkill>()
                .HasOne(ms => ms.Skill)
                .WithMany(s => s.MentorSkills)
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // MentorSlot
            modelBuilder.Entity<MentorSlot>()
                .HasKey(ms => ms.Id);

            modelBuilder.Entity<MentorSlot>()
                .HasMany(ms => ms.BookingSlots)
                .WithOne(bs => bs.MentorSlot)
                .HasForeignKey(bs => bs.MentorSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookingSlot
            modelBuilder.Entity<BookingSlot>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.Group)
                .WithMany(g => g.BookingSlots)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingSlot>()
                .HasOne(b => b.MentorSlot)
                .WithMany(ms => ms.BookingSlots)
                .HasForeignKey(b => b.MentorSlotId)
                .OnDelete(DeleteBehavior.Restrict); // Chuyển từ Cascade sang Restrict

            // Group
            modelBuilder.Entity<Group>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Topic)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TopicId)
                .OnDelete(DeleteBehavior.Restrict); // Chuyển từ Cascade sang Restrict

            modelBuilder.Entity<Group>()
                .HasOne(g => g.SwpClass)
                .WithMany(sc => sc.Groups)
                .HasForeignKey(g => g.SwpClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Semester
            modelBuilder.Entity<Semester>()
                .HasKey(s => s.Id);

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

            // SwpClass
            modelBuilder.Entity<SwpClass>()
                .HasKey(sc => sc.Id);

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
                .HasKey(t => t.Id);

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Semester)
                .WithMany(s => s.Topics)
                .HasForeignKey(t => t.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Skill
            modelBuilder.Entity<Skill>()
                .HasKey(s => s.Id);

            // Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Chuyển từ Cascade sang Restrict

            // WalletTransaction
            modelBuilder.Entity<WalletTransaction>()
                .HasKey(wt => wt.Id);

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.BookingSlot)
                .WithMany()
                .HasForeignKey(wt => wt.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
