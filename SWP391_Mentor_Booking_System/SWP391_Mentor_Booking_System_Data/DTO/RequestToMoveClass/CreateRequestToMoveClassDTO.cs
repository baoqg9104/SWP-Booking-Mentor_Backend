using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.RequestToMoveClass
{
    public class CreateRequestToMoveClassDTO
    {
        public string StudentId { get; set; }
        public int CurrentClassId { get; set; }
        public int ClassIdToMove { get; set; }
        public string? Reason { get; set; }
    }
}
