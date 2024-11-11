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
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SwpClass> SwpClasses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<BookingSkill> BookingSkills { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Mentor
            modelBuilder.Entity<Mentor>()
                .HasKey(m => m.MentorId);



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




            // MentorSkill
            modelBuilder.Entity<MentorSkill>()
                .HasKey(ms => ms.MentorSkillId);

            modelBuilder.Entity<MentorSkill>()
                .HasOne(ms => ms.Skill)
                .WithMany(s => s.MentorSkills)
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MentorSkill>()
                .HasMany(ms => ms.BookingSkills)
                .WithOne(bs => bs.MentorSkill)
                .HasForeignKey(bs => bs.MentorSkillId)
                .OnDelete(DeleteBehavior.Cascade);

            //Booking Skill

            modelBuilder.Entity<BookingSkill>()
                .HasKey(msl => msl.BookingSkillId);

            modelBuilder.Entity<BookingSkill>()
                .HasOne(bsl => bsl.BookingSlot)
                .WithMany(bs => bs.BookingSkills)
                .HasForeignKey(bsl => bsl.BookingSlotId);

            modelBuilder.Entity<BookingSkill>()
                .HasOne(bsl => bsl.MentorSkill)
                .WithMany(msl => msl.BookingSkills)
                .HasForeignKey(bsl => bsl.MentorSkillId);

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

            //modelBuilder.Entity<BookingSlot>()
            //    .HasMany(bs => bs.BookingSkills)      
            //    .WithOne(bsl => bsl.BookingSlot)  
            //    .HasForeignKey(bs => bs.MentorSkillId);

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

            modelBuilder.Entity<Group>()
               .HasMany(g => g.Students)
               .WithOne(s => s.Group)
               .HasForeignKey(s => s.GroupId)
               .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<Group>()
            //    .HasOne(g => g.Leader)
            //    .WithMany()
            //    .HasForeignKey(g => g.LeaderId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // SwpClass
            modelBuilder.Entity<SwpClass>()
                .HasKey(sc => sc.SwpClassId);

            modelBuilder.Entity<SwpClass>()
                .HasOne(sc => sc.Semester)
                .WithMany(s => s.SwpClasses)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // WalletTransaction
            modelBuilder.Entity<WalletTransaction>()
                .HasKey(wt => wt.TransactionId);

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.BookingSlot)
                .WithMany()
                .HasForeignKey(wt => wt.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.BookingSlot)
                .WithOne(b => b.Feedback)
                .HasForeignKey<Feedback>(f => f.BookingId);





        }
    }
}
