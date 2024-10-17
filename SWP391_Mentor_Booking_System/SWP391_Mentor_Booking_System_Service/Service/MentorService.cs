using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.Repositories;
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
        private readonly MentorRepository _mentroRepository;

        public MentorService(SWP391_Mentor_Booking_System_DBContext context, MentorRepository mentorRepository)
        {
            _context = context;
            _mentroRepository = mentorRepository;
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

        public async Task<IEnumerable<Mentor>> GetAllMentorsAsync()
        {
            return await _mentroRepository.GetAllMentorsAsync();
        }

        public async Task<Mentor> GetMentorByIdAsync(string id)
        {
            return await _mentroRepository.GetMentorByIdAsync(id);
        }

        public async Task<bool> DeleteMentorAsync(string id)
        {
            var mentor = await _mentroRepository.GetMentorByIdAsync(id);
            if(mentor == null)
            {
                return false;
            }

            await _mentroRepository.DeleteMentorAsync(mentor);
            return true;
        }
    }

}
