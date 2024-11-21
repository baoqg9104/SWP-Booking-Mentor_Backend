using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.SwpClass;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<bool> CreateClassAsync(CreateSwpClassDTO createSwpClassDto)
        {
            var newClass = new SwpClass
            {
                //SwpClassId = createSwpClassDto.SwpClassId,
                Name = createSwpClassDto.Name,
                SemesterId = createSwpClassDto.SemesterId,
                Status = createSwpClassDto.Status
            };

            _context.SwpClasses.Add(newClass);
            return await _context.SaveChangesAsync() > 0;
        }

        // Read by Id
        public async Task<SwpClassDTO> GetClassByIdAsync(int classId)
        {
            var swpClass = await _context.SwpClasses.FirstOrDefaultAsync(c => c.SwpClassId == classId);

            if (swpClass == null) return null;

            return new SwpClassDTO
            {
                SwpClassId = swpClass.SwpClassId,
                Name = swpClass.Name,
                SemesterId = swpClass.SemesterId,
                Status = swpClass.Status
            };
        }

        // Read all
        public async Task<List<SwpClassDTO>> GetAllClassesAsync()
        {
            return await _context.SwpClasses
                .Select(c => new SwpClassDTO
                {
                    SwpClassId = c.SwpClassId,
                    Name = c.Name,
                    SemesterId = c.SemesterId,
                    Status = c.Status
                })
                .ToListAsync();
        }
        public async Task<bool> UpdateClassByIdAsync(int swpClassId, UpdateSwpClassDTO updateSwpClassDto)
        {
            var existingClass = await _context.SwpClasses.FirstOrDefaultAsync(c => c.SwpClassId == swpClassId);

            if (existingClass == null)
                return false;

            existingClass.Name = updateSwpClassDto.Name;
            existingClass.SemesterId = updateSwpClassDto.SemesterId;
            existingClass.Status = updateSwpClassDto.Status;

            return await _context.SaveChangesAsync() > 0;
        }



        // Delete
        public async Task<bool> DeleteClassAsync(int classId)
        {
            var swpClass = await _context.SwpClasses.FirstOrDefaultAsync(c => c.SwpClassId == classId);

            if (swpClass == null)
                return false;

            swpClass.Status = false;
 
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
