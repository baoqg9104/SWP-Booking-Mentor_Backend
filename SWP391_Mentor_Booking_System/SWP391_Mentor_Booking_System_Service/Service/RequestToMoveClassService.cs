using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.RequestToMoveClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class RequestToMoveClassService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public RequestToMoveClassService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string error)> CreateRequestToMoveClassAsync(CreateRequestToMoveClassDTO dto)
        {
            if (_context.RequestToMoveClasses.Any(x => x.StudentId == dto.StudentId && x.Status == "Pending"))
            {
                return (false, "You have an unresolved request");
            }

            var request = new RequestToMoveClass()
            {
                StudentId = dto.StudentId,
                CurrentClassId = dto.CurrentClassId,
                ClassIdToMove = dto.ClassIdToMove,
                Reason = dto.Reason,
                RequestDate = DateTime.Now,
                Status = "Pending",
            };

            _context.RequestToMoveClasses.Add(request);

            await _context.SaveChangesAsync();

            return (true, "");
        }

        public async Task<List<GetRequestToMoveClassDTO>> GetRequestToMoveClassesAsync()
        {
            var list = await _context.RequestToMoveClasses
                .Select(x => new GetRequestToMoveClassDTO() {
                    RequestId = x.RequestId,
                    StudentId = x.StudentId,
                    CurrentClassName = x.CurrentClass.Name,
                    ClassNameToMove = x.ClassToMove.Name,
                    Reason = x.Reason,
                    RequestDate = x.RequestDate,
                    ApprovalDate = x.ApprovalDate,
                    Status = x.Status,
                })
                .ToListAsync();

            return list;
        }

        public async Task<(bool success, string error)> UpdateRequestToMoveClassAsync(UpdateRequestToMoveClassDTO dto) 
        {
            var request = await _context.RequestToMoveClasses.FirstOrDefaultAsync(x => x.RequestId == dto.RequestId);

            if (request == null)
            {
                return (false, "Request not found");
            }

            request.ApprovalDate = DateTime.Now;
            request.Status = dto.Status;

            var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == request.StudentId);

            if (student != null)
            {
                student.SwpClassId = request.ClassIdToMove;
            }

            await _context.SaveChangesAsync();

            return (true, "Request resolved successfully");

        }
    }
}
