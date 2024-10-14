using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await _studentRepository.GetStudentByIdAsync(id);
        }

        public async Task<bool> DeleteStudentAsync(string id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return false;
            }

            await _studentRepository.DeleteStudentAsync(student);
            return true;
        }
    }
}
