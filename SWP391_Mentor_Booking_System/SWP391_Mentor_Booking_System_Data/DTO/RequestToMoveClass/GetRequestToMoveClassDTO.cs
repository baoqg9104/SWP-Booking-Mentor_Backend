using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.RequestToMoveClass
{
    public class GetRequestToMoveClassDTO
    {
        public int RequestId { get; set; }
        public string StudentId { get; set; }
        public string CurrentClassName { get; set; }
        public string ClassNameToMove { get; set; }
        public string? Reason { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; } = null;
        public string Status { get; set; }
    }
}
