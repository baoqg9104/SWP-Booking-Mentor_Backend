using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.Semester;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class SemesterService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public SemesterService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<bool> CreateSemesterAsync(CreateSemesterDTO createSemesterDto)
        {
            var semester = new Semester
            {
                SemesterId = Guid.NewGuid().ToString(), // Auto-generate SemesterId
                Code = createSemesterDto.Code,
                Name = createSemesterDto.Name,
                StartDate = createSemesterDto.StartDate,
                EndDate = createSemesterDto.EndDate,
                Status = createSemesterDto.Status
            };

            _context.Semesters.Add(semester);
            return await _context.SaveChangesAsync() > 0;
        }

        // Read by Id
        public async Task<SemesterDTO> GetSemesterByIdAsync(string semesterId)
        {
            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.SemesterId == semesterId);

            if (semester == null)
                return null;

            return new SemesterDTO
            {
                SemesterId = semester.SemesterId,
                Code = semester.Code,
                Name = semester.Name,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                Status = semester.Status
            };
        }

        // Read all
        public async Task<List<SemesterDTO>> GetAllSemestersAsync()
        {
            return await _context.Semesters
                .Select(s => new SemesterDTO
                {
                    SemesterId = s.SemesterId,
                    Code = s.Code,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status
                })
                .ToListAsync();
        }

        // Update
        public async Task<bool> UpdateSemesterAsync(UpdateSemesterDTO updateSemesterDto)
        {
            var existingSemester = await _context.Semesters.FirstOrDefaultAsync(s => s.SemesterId == updateSemesterDto.SemesterId);

            if (existingSemester == null)
                return false;

            existingSemester.Code = updateSemesterDto.Code;
            existingSemester.Name = updateSemesterDto.Name;
            existingSemester.StartDate = updateSemesterDto.StartDate;
            existingSemester.EndDate = updateSemesterDto.EndDate;
            existingSemester.Status = updateSemesterDto.Status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteSemesterAsync(string semesterId)
        {
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.SemesterId == semesterId);

            if (semester == null)
                return false;

            _context.Semesters.Remove(semester);
            return await _context.SaveChangesAsync() > 0;
        }
    }


}
