    using Microsoft.EntityFrameworkCore;
    using SWP391_Mentor_Booking_System_Data.Data;
    using System.Linq;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class AuthRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public AuthRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            return _context.Students.Any(s => s.Email == email) || _context.Mentors.Any(m => m.Email == email);
        }
        public int GetTotalStudents()
        {
            return _context.Students.Count(); // Trả về số lượng sinh viên hiện tại
        }
        public int GetTotalMentors()
        {
            return _context.Mentors.Count();
        }
        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }
        public async Task<Mentor> GetMentorByEmailAsync(string email)
        {
            return await _context.Mentors.FirstOrDefaultAsync(m => m.Email == email);
        }
        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            // Tìm kiếm admin bằng email. Giả định có lớp Admin trong Data.
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
        }

        public void AddMentor(Mentor mentor)
        {
            _context.Mentors.Add(mentor);
        }
        public async Task<Student> GetStudentByIdAsync(string userId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == userId);
        }

        public async Task<Mentor> GetMentorByIdAsync(string userId)
        {
            return await _context.Mentors.FirstOrDefaultAsync(m => m.MentorId == userId);
        }
    }
}