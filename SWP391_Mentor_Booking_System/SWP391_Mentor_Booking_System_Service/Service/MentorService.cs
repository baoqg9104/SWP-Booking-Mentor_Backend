using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class MentorService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public MentorService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Method to change apply status
        public async Task<bool> ChangeMentorApplyStatusAsync(string mentorId)
        {
            var mentor = await _context.Mentors.FirstOrDefaultAsync(m => m.MentorId == mentorId);
            if (mentor == null)
                return false;

            mentor.ApplyStatus = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
