using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
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
        private readonly EmailService _emailService;
        public UserService(SWP391_Mentor_Booking_System_DBContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<(bool success, string error)> UpdateUserAsync(UpdateUserDTO updateUserDto)
        {
            if(updateUserDto.role == "Student")
            {
                var user = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == updateUserDto.Id);
                if (user == null)
                    return (false, "Account does not exist");

                var email = await _context.Students.FirstOrDefaultAsync(s => s.StudentId !=  updateUserDto.Id && s.Email == updateUserDto.Email);

                if (email != null)
                    return (false, "Email already exists");

                var phone = await _context.Students.FirstOrDefaultAsync(s => s.StudentId != updateUserDto.Id && s.Phone == updateUserDto.Phone);

                if (phone != null)
                    return (false, "Phone already exists");

                user.Email = updateUserDto.Email;
                user.StudentName = updateUserDto.Name;
                user.Phone = updateUserDto.Phone;
                user.Gender = updateUserDto.Gender;
                user.DateOfBirth = updateUserDto.DateOfBirth;

                _context.Students.Update(user);
                await _context.SaveChangesAsync();

                return (true, "");
            }

            if(updateUserDto.role == "Mentor")
            {
                var user = await _context.Mentors.FirstOrDefaultAsync(u => u.MentorId == updateUserDto.Id);
                if (user == null)
                    return (false, "Account does not exist");

                var email = await _context.Mentors.FirstOrDefaultAsync(s => s.MentorId != updateUserDto.Id && s.Email == updateUserDto.Email);

                if (email != null)
                    return (false, "Email already exists");

                user.Email = updateUserDto.Email;
                user.MentorName = updateUserDto.Name;
                user.Phone = updateUserDto.Phone;
                user.Gender = updateUserDto.Gender;
                user.DateOfBirth = updateUserDto.DateOfBirth;
                user.MeetUrl = updateUserDto.MeetUrl;

                _context.Mentors.Update(user);
                await _context.SaveChangesAsync();

                return (true, "");
            }

            return (false, "");
        }

        public async Task<bool> ChangePasswordAsync(ChangePassDTO changePassDto)
        {
            if (changePassDto.Role == "Student")
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == changePassDto.Id);
                if (student == null || !BCrypt.Net.BCrypt.Verify(changePassDto.OldPassword, student.Password))
                    return false;

                var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePassDto.NewPassword);
                student.Password = hashedNewPassword;
                await _context.SaveChangesAsync();

                return true;
            }

            if (changePassDto.Role == "Mentor")
            {
                var mentor = await _context.Mentors.FirstOrDefaultAsync(s => s.MentorId == changePassDto.Id);
                if (mentor == null || !BCrypt.Net.BCrypt.Verify(changePassDto.OldPassword, mentor.Password))
                    return false;

                var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePassDto.NewPassword);
                mentor.Password = hashedNewPassword;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<(bool Success, string Error)> GenerateAndSendOtpAsync(string email) 
        {
            var user = await _context.Admins
            .Where(x => x.Email == email)
            .Select(x => new { x.Email, UserType = "Admin" })
            .Union(_context.Mentors
                .Where(x => x.Email == email)
                .Select(x => new { x.Email, UserType = "Mentor" }))
            .Union(_context.Students
                .Where(x => x.Email == email)
                .Select(x => new { x.Email, UserType = "Student" }))
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return (false, "User not found");
            }


            var otp = new Random().Next(100000, 999999).ToString(); 
            var otpExpiry = DateTime.Now.AddMinutes(10); 
            var otpEntry = new OtpEntry 
            { 
                Id = Guid.NewGuid().ToString(), 
                Email = email, 
                Otp = otp, 
                OtpExpiry = otpExpiry 
            }; 
            _context.OtpEntries.Add(otpEntry); 
            await _context.SaveChangesAsync(); 
            await _emailService.SendOtpEmailAsync(email, otp); 
            return (true, null); 
        }

        public async Task<(bool Success, string Error)> ValidateOtpAsync(string email, string otp)
        {
            var otpEntry = await _context.OtpEntries
                .FirstOrDefaultAsync(o => o.Email == email && o.Otp == otp);

            if (otpEntry == null)
                return (false, "Invalid OTP");

            if (otpEntry.OtpExpiry < DateTime.Now)
                return (false, "OTP has expired");

            _context.OtpEntries.Remove(otpEntry);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, string Error)> SetNewPasswordAsync(SetNewPasswordDTO dto)
        {
            var user = await _context.Admins
            .Where(x => x.Email == dto.Email)
            .Select(x => new { x.Email, UserType = "Admin" })
            .Union(_context.Mentors
                .Where(x => x.Email == dto.Email)
                .Select(x => new { x.Email, UserType = "Mentor" }))
            .Union(_context.Students
                .Where(x => x.Email == dto.Email)
                .Select(x => new { x.Email, UserType = "Student" }))
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return (false, "User not found");
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            if (user.UserType == "Admin")
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == dto.Email);
                admin.Password = hashPassword;
            }
            else if (user.UserType == "Mentor")
            {
                var mentor = await _context.Mentors.FirstOrDefaultAsync(x => x.Email == dto.Email);
                mentor.Password = hashPassword;
            }
            else if (user.UserType == "Student")
            {
                var student = await _context.Students.FirstOrDefaultAsync(x => x.Email == dto.Email);
                student.Password = hashPassword;
            }

            await _context.SaveChangesAsync();
            return (true, "Password updated successfully");
        }
    }

}
