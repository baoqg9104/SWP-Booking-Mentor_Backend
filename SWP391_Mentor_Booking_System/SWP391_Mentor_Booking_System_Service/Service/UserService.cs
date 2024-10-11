using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class UserService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public UserService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDto)
        {
            if(updateUserDto.role == "Student")
            {
                var user = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == updateUserDto.Id);
                if (user == null)
                    return false;

                user.Email = updateUserDto.Email;
                user.StudentName = updateUserDto.Name;
                user.Phone = updateUserDto.Phone;
                user.Gender = updateUserDto.Gender;
                user.DateOfBirth = updateUserDto.DateOfBirth;

                _context.Students.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }

            if(updateUserDto.role == "Mentor")
            {
                var user = await _context.Mentors.FirstOrDefaultAsync(u => u.MentorId == updateUserDto.Id);
                if (user == null)
                    return false;

                user.Email = updateUserDto.Email;
                user.MentorName = updateUserDto.Name;
                user.Phone = updateUserDto.Phone;
                user.Gender = updateUserDto.Gender;
                user.DateOfBirth = updateUserDto.DateOfBirth;

                _context.Mentors.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }

}
