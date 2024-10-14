using SWP391_Mentor_Booking_System_Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class StudentRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public StudentRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task DeleteStudentAsync(Student student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
