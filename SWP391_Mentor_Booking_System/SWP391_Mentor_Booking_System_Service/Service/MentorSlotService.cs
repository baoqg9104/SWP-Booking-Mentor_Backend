using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.MentorSlot;

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
        public async Task<(bool Success, string Error)> CreateMentorSlotAsync(
            CreateMentorSlotDTO createMentorSlotDto
        )
        {
            // Check if the slot is in the past
            if (createMentorSlotDto.StartTime < DateTime.Now)
                return (false, "The start time of the mentor slot cannot be in the past");

            // Check for overlapping slots for the mentor
            var overlappingMentorSlots = await _context
                .MentorSlots.Where(ms =>
                    ms.MentorId == createMentorSlotDto.MentorId
                    && ms.StartTime < createMentorSlotDto.EndTime
                    && createMentorSlotDto.StartTime < ms.EndTime
                )
                .ToListAsync();

            if (overlappingMentorSlots.Any())
                return (false, "Overlapping mentor slots");

            // Check for overlapping slots in the room
            if (!string.IsNullOrEmpty(createMentorSlotDto.Room))
            {
                var overlappingRoomSlots = await _context
                    .MentorSlots.Where(ms =>
                        ms.room == createMentorSlotDto.Room
                        && ms.StartTime < createMentorSlotDto.EndTime
                        && createMentorSlotDto.StartTime < ms.EndTime
                    )
                    .ToListAsync();

                if (overlappingRoomSlots.Any())
                    return (false, "Overlapping room slots");
            }

            var mentor = await _context.Mentors.SingleOrDefaultAsync(m =>
                m.MentorId == createMentorSlotDto.MentorId
            );

            if (mentor.NumOfSlot == 0)
            {
                return (false, null);
            }

            var mentorSlot = new MentorSlot
            {
                MentorId = createMentorSlotDto.MentorId,
                StartTime = createMentorSlotDto.StartTime,
                EndTime = createMentorSlotDto.EndTime,
                BookingPoint = createMentorSlotDto.BookingPoint,
                isOnline = createMentorSlotDto.IsOnline,
                room = createMentorSlotDto.Room,
                Status = "Pending"
            };

            _context.MentorSlots.Add(mentorSlot);

            mentor.NumOfSlot -= 1;

            await _context.SaveChangesAsync();



            return (true, null);
        }

        // Read by MentorSlotId
        public async Task<MentorSlotDTO> GetMentorSlotByIdAsync(int mentorSlotId)
        {
            var mentorSlot = await _context
                .MentorSlots.Include(ms => ms.Mentor)
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
                room = mentorSlot.room,
            };
        }

        // Read all
        public async Task<List<MentorSlotDTO>> GetAllMentorSlotsAsync()
        {
            return await _context
                .MentorSlots.Select(ms => new MentorSlotDTO
                {
                    MentorSlotId = ms.MentorSlotId,
                    MentorId = ms.MentorId,
                    StartTime = ms.StartTime,
                    EndTime = ms.EndTime,
                    BookingPoint = ms.BookingPoint,
                    isOnline = ms.isOnline,
                    room = ms.room,
                })
                .ToListAsync();
        }

        // Read by MentorId
        public async Task<List<MentorSlotDTO>> GetMentorSlotsByMentorIdAsync(string mentorId)
        {
            return await _context
                .MentorSlots.Where(ms => ms.MentorId == mentorId)
                .Select(ms => new MentorSlotDTO
                {
                    MentorSlotId = ms.MentorSlotId,
                    MentorId = ms.MentorId,
                    StartTime = ms.StartTime,
                    EndTime = ms.EndTime,
                    BookingPoint = ms.BookingPoint,
                    isOnline = ms.isOnline,
                    room = ms.room,
                    Status = ms.Status
                })
                .ToListAsync();
        }

        // Update
        public async Task<(bool Success, string Error)> UpdateMentorSlotAsync(
            UpdateMentorSlotDTO updateMentorSlotDto
        )
        {
            var existingMentorSlot = await _context.MentorSlots.FirstOrDefaultAsync(ms =>
                ms.MentorSlotId == updateMentorSlotDto.MentorSlotId
            );

            if (existingMentorSlot == null)
                return (false, "Mentor slot not found");

            // Check if the slot is in the past
            if (updateMentorSlotDto.StartTime < DateTime.Now)
                return (false, "The start time of the mentor slot cannot be in the past");

            // Check for overlapping slots for the mentor
            var overlappingMentorSlots = await _context
                .MentorSlots.Where(ms =>
                    ms.MentorSlotId != updateMentorSlotDto.MentorSlotId
                    && ms.StartTime < updateMentorSlotDto.EndTime
                    && updateMentorSlotDto.StartTime < ms.EndTime
                )
                .ToListAsync();

            if (overlappingMentorSlots.Any())
                return (false, "Overlapping mentor slots");

            // Check for overlapping slots in the room
            if (!string.IsNullOrEmpty(updateMentorSlotDto.Room))
            {
                var overlappingRoomSlots = await _context
                    .MentorSlots.Where(ms =>
                        ms.room == updateMentorSlotDto.Room
                        && ms.MentorSlotId != updateMentorSlotDto.MentorSlotId
                        && ms.StartTime < updateMentorSlotDto.EndTime
                        && updateMentorSlotDto.StartTime < ms.EndTime
                    )
                    .ToListAsync();

                if (overlappingRoomSlots.Any())
                    return (false, "Overlapping room slots");
            }

            existingMentorSlot.StartTime = updateMentorSlotDto.StartTime;
            existingMentorSlot.EndTime = updateMentorSlotDto.EndTime;
            existingMentorSlot.BookingPoint = updateMentorSlotDto.BookingPoint;
            existingMentorSlot.isOnline = updateMentorSlotDto.IsOnline;
            existingMentorSlot.room = updateMentorSlotDto.Room;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        // Delete
        public async Task<bool> DeleteMentorSlotAsync(int mentorSlotId)
        {
            var mentorSlot = await _context.MentorSlots.FirstOrDefaultAsync(ms =>
                ms.MentorSlotId == mentorSlotId
            );

            if (mentorSlot == null)
                return false;

            _context.MentorSlots.Remove(mentorSlot);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MentorAppointmentDTO>> GetMentorAppointmentsByMentorIdAsync(
            string mentorId
        )
        {
            var mentorAppointments = await _context
                .MentorSlots.Where(ms => ms.MentorId == mentorId)
                .Select(ms => new MentorAppointmentDTO
                {
                    MentorSlotId = ms.MentorSlotId,
                    MentorId = ms.MentorId,
                    StartTime = ms.StartTime,
                    EndTime = ms.EndTime,
                    BookingPoint = ms.BookingPoint,
                    isOnline = ms.isOnline,
                    room = ms.room,
                    Status = ms.Status,
                })
                .ToListAsync();

            foreach (var slot in mentorAppointments)
            {
                var numOfPending = await _context
                    .BookingSlots.Where(bs =>
                        bs.MentorSlotId == slot.MentorSlotId && bs.Status == "Pending"
                    )
                    .CountAsync();

                //slot.Bookings = numOfPending;

                var approvedBooking = await _context.BookingSlots.SingleOrDefaultAsync(bs =>
                    bs.MentorSlotId == slot.MentorSlotId && bs.Status == "Approved"
                );

                var completedBooking = await _context.BookingSlots.SingleOrDefaultAsync(bs =>
                    bs.MentorSlotId == slot.MentorSlotId && bs.Status == "Completed"
                );

                var groupId = await _context.BookingSlots
                    .SingleOrDefaultAsync(bs => bs.MentorSlotId == slot.MentorSlotId && bs.Status == "Completed");

                if (numOfPending > 0)
                {
                    slot.Status = "Pending";
                    slot.Bookings = numOfPending;
                }
                else if (approvedBooking != null)
                {
                    slot.Status = "Approved";
                    var group = await _context.Groups.SingleOrDefaultAsync(g =>
                        g.GroupId == approvedBooking.GroupId
                    );
                    slot.Group = group.Name;

                    var swpClass = await _context.SwpClasses.SingleOrDefaultAsync(c => c.SwpClassId == group.SwpClassId);
                    slot.Class = swpClass.Name;
                }
                else if (completedBooking != null)
                {
                    slot.Status = "Completed";
                    var group = await _context.Groups.SingleOrDefaultAsync(g =>
                        g.GroupId == completedBooking.GroupId
                    );
                    slot.Group = group.Name;

                    var swpClass = await _context.SwpClasses.SingleOrDefaultAsync(c => c.SwpClassId == group.SwpClassId);
                    slot.Class = swpClass.Name;
                }
            }

            return mentorAppointments;
        }
    }
}
