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
    public class MentorSlotService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public MentorSlotService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<bool> CreateMentorSlotAsync(CreateMentorSlotDTO createMentorSlotDto)
        {
            var mentor = await _context.Mentors.FirstOrDefaultAsync(m => m.MentorId == createMentorSlotDto.MentorId);
            if (mentor == null)
                return false; // MentorId does not exist

            var mentorSlot = new MentorSlot
            {
                MentorId = createMentorSlotDto.MentorId,
                StartTime = createMentorSlotDto.StartTime,
                EndTime = createMentorSlotDto.EndTime,
                BookingPoint = createMentorSlotDto.BookingPoint,
                isOnline = createMentorSlotDto.IsOnline,
                room = createMentorSlotDto.Room
            };

            _context.MentorSlots.Add(mentorSlot);
            return await _context.SaveChangesAsync() > 0;
        }

        // Read by MentorSlotId
        public async Task<MentorSlotDTO> GetMentorSlotByIdAsync(int mentorSlotId)
        {
            var mentorSlot = await _context.MentorSlots
                .Include(ms => ms.Mentor)
                .FirstOrDefaultAsync(ms => ms.MentorSlotId == mentorSlotId);

            if (mentorSlot == null)
                return null;

            return new MentorSlotDTO
            {
                MentorSlotId = mentorSlot.MentorSlotId,
                MentorId = mentorSlot.MentorId,
                StartTime = mentorSlot.StartTime,
                EndTime = mentorSlot.EndTime,
                BookingPoint = mentorSlot.BookingPoint,
                isOnline = mentorSlot.isOnline,
                room = mentorSlot.room
            };
        }

        // Read all
        public async Task<List<MentorSlotDTO>> GetAllMentorSlotsAsync()
        {
            return await _context.MentorSlots
                .Select(ms => new MentorSlotDTO
                {
                    MentorSlotId = ms.MentorSlotId,
                    MentorId = ms.MentorId,
                    StartTime = ms.StartTime,
                    EndTime = ms.EndTime,
                    BookingPoint = ms.BookingPoint,
                    isOnline = ms.isOnline,
                    room = ms.room
                })
                .ToListAsync();
        }

        // Read by MentorId
        public async Task<List<MentorSlotDTO>> GetMentorSlotsByMentorIdAsync(string mentorId)
        {
            return await _context.MentorSlots
                .Where(ms => ms.MentorId == mentorId)
                .Select(ms => new MentorSlotDTO
                {
                    MentorSlotId = ms.MentorSlotId,
                    MentorId = ms.MentorId,
                    StartTime = ms.StartTime,
                    EndTime = ms.EndTime,
                    BookingPoint = ms.BookingPoint,
                    isOnline = ms.isOnline,
                    room = ms.room
                })
                .ToListAsync();
        }

        // Update
        public async Task<bool> UpdateMentorSlotAsync(UpdateMentorSlotDTO updateMentorSlotDto)
        {
            var existingMentorSlot = await _context.MentorSlots
                .FirstOrDefaultAsync(ms => ms.MentorSlotId == updateMentorSlotDto.MentorSlotId);

            if (existingMentorSlot == null)
                return false;

            existingMentorSlot.StartTime = updateMentorSlotDto.StartTime;
            existingMentorSlot.EndTime = updateMentorSlotDto.EndTime;
            existingMentorSlot.BookingPoint = updateMentorSlotDto.BookingPoint;
            existingMentorSlot.isOnline = updateMentorSlotDto.IsOnline;
            existingMentorSlot.room = updateMentorSlotDto.Room;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteMentorSlotAsync(int mentorSlotId)
        {
            var mentorSlot = await _context.MentorSlots
                .FirstOrDefaultAsync(ms => ms.MentorSlotId == mentorSlotId);

            if (mentorSlot == null)
                return false;

            _context.MentorSlots.Remove(mentorSlot);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
