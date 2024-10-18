using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.SwpClass;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class SwpClassService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public SwpClassService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<bool> CreateSwpClassAsync(CreateSwpClassDTO createSwpClassDto)
        {
            var swpClass = new SwpClass
            {
                Name = createSwpClassDto.Name,
                MentorId = createSwpClassDto.MentorId,
                SemesterId = createSwpClassDto.SemesterId,
                Status = createSwpClassDto.Status
            };

            _context.SwpClasses.Add(swpClass);
            return await _context.SaveChangesAsync() > 0;
        }

        // Read by Id
        public async Task<SwpClassDTO> GetSwpClassByIdAsync(int swpClassId)
        {
            var swpClass = await _context.SwpClasses
                .Include(s => s.Semester)
                .FirstOrDefaultAsync(s => s.SwpClassId == swpClassId);

            if (swpClass == null)
                return null;

            return new SwpClassDTO
            {
                SwpClassId = swpClass.SwpClassId,
                Name = swpClass.Name,
                MentorId = swpClass.MentorId,
                SemesterId = swpClass.SemesterId,
                SemesterName = swpClass.Semester.Name,
                Status = swpClass.Status
            };
        }

        // Read all
        public async Task<List<SwpClassDTO>> GetAllSwpClassesAsync()
        {
            return await _context.SwpClasses
                .Include(s => s.Semester)
                .Select(s => new SwpClassDTO
                {
                    SwpClassId = s.SwpClassId,
                    Name = s.Name,
                    MentorId = s.MentorId,
                    SemesterId = s.SemesterId,
                    SemesterName = s.Semester.Name,
                    Status = s.Status
                })
                .ToListAsync();
        }

        // Update
        public async Task<bool> UpdateSwpClassAsync(UpdateSwpClassDTO updateSwpClassDto)
        {
            var existingSwpClass = await _context.SwpClasses.FirstOrDefaultAsync(s => s.SwpClassId == updateSwpClassDto.SwpClassId);

            if (existingSwpClass == null)
                return false;

            existingSwpClass.Name = updateSwpClassDto.Name;
            existingSwpClass.MentorId = updateSwpClassDto.MentorId;
            existingSwpClass.SemesterId = updateSwpClassDto.SemesterId;
            existingSwpClass.Status = updateSwpClassDto.Status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteSwpClassAsync(int swpClassId)
        {
            var swpClass = await _context.SwpClasses.FirstOrDefaultAsync(s => s.SwpClassId == swpClassId);

            if (swpClass == null)
                return false;

            _context.SwpClasses.Remove(swpClass);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
