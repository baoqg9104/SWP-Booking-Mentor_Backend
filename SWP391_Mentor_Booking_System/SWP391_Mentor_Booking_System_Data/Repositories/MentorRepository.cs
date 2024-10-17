using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class MentorRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public MentorRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mentor>> GetAllMentorsAsync()
        {
            return await _context.Mentors.ToListAsync();
        }

        public async Task<Mentor> GetMentorByIdAsync(string id)
        {
            return await _context.Mentors.FirstOrDefaultAsync(m => m.MentorId == id);
        }

        public async Task DeleteMentorAsync(Mentor mentor)
        {
            _context.Mentors.Remove(mentor);
            await _context.SaveChangesAsync();
        }
    }
}
