using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.BookingSlot;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class BookingService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public BookingService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create Booking
        public async Task<(bool Success, string Error)> CreateBookingAsync(CreateBookingDTO createBookingDto)
        {
            // Check if the group exists and has enough wallet points
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == createBookingDto.GroupId);
            if (group == null)
                return (false, "Group does not exist");

            var mentorSlot = await _context.MentorSlots.FirstOrDefaultAsync(ms => ms.MentorSlotId == createBookingDto.MentorSlotId);
            if (mentorSlot == null)
                return (false, "Mentor slot does not exist");

            if (group.WalletPoint < mentorSlot.BookingPoint)
                return (false, "Not enough wallet points");

            // Check if the slot is in the past
            if (mentorSlot.StartTime < DateTime.Now)
                return (false, "Cannot book a slot in the past");

            // Check for overlapping bookings for the group
            var overlappingBooking = await _context.BookingSlots
                .Include(b => b.MentorSlot) 
                .FirstOrDefaultAsync(b => b.GroupId == createBookingDto.GroupId
                && b.MentorSlot.StartTime < mentorSlot.EndTime
                && b.MentorSlot.EndTime > mentorSlot.StartTime);


            if (overlappingBooking != null)
                return (false, "The group already has a booking that overlaps with the selected slot");



            // Create the booking
            var booking = new BookingSlot
            {
                GroupId = createBookingDto.GroupId,
                MentorSlotId = createBookingDto.MentorSlotId,
                SkillId = createBookingDto.SkillId,
                BookingTime = DateTime.Now,
                Status = "Pending"
            };

            _context.BookingSlots.Add(booking);

            // Deduct wallet points from the group
            group.WalletPoint -= mentorSlot.BookingPoint;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        // Update Booking Status
        public async Task<bool> UpdateBookingStatusAsync(UpdateBookingStatusDTO updateBookingStatusDto)
        {
            var existingBooking = await _context.BookingSlots.FirstOrDefaultAsync(b => b.BookingId == updateBookingStatusDto.BookingId);

            if (existingBooking == null)
                return false;

            existingBooking.Status = updateBookingStatusDto.Status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Get BookingSlots by MentorSlotId
        public async Task<List<BookingDTO>> GetBookingsByMentorSlotIdAsync(int mentorSlotId)
        {
            var bookings = await _context.BookingSlots
                .Where(bs => bs.MentorSlotId == mentorSlotId)
                .ToListAsync();

            return bookings.Select(bs => new BookingDTO
            {
                BookingId = bs.BookingId,
                GroupId = bs.GroupId,
                GroupName = _context.Groups.FirstOrDefault(g => g.GroupId == bs.GroupId)?.Name ?? "Unknown",
                MentorSlotId = bs.MentorSlotId,
                SkillName = _context.Skills.FirstOrDefault(s => s.SkillId == bs.SkillId).Name,
                BookingTime = bs.BookingTime,
                Status = bs.Status
            }).ToList();
        }

        public async Task<List<BookingDTO>> GetBookingByGroupIdAsync(string groupId)
        {
            var bookings = await _context.BookingSlots
                .Where(bs => bs.GroupId == groupId)
                .ToListAsync();

            return bookings.Select(bs => new BookingDTO
            {
                BookingId = bs.BookingId,
                GroupId = bs.GroupId,
                GroupName = _context.Groups.FirstOrDefault(g => g.GroupId == bs.GroupId)?.Name ?? "Unknown",
                MentorSlotId = bs.MentorSlotId,
                SkillName = _context.Skills.FirstOrDefault(s => s.SkillId == bs.SkillId).Name,
                BookingTime = bs.BookingTime,
                Status = bs.Status
            }).ToList();
        }
    }

}
