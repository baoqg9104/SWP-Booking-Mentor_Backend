using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;
using System.Linq;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class UserRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public UserRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public int GetTotalStudents()
        {
            return _context.Students.Count(); // Trả về số lượng sinh viên hiện tại
        }
    }

}
