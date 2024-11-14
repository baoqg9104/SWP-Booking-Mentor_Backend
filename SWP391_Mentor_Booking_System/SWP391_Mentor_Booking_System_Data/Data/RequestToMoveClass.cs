using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class RequestToMoveClass
    {
        public int RequestId { get; set; }
        public string StudentId { get; set; }
        public int CurrentClassId { get; set; }
        public int ClassIdToMove {  get; set; }
        public string? Reason { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; } = null;
        public string Status { get; set; }

        public Student Student { get; set; }
        public SwpClass CurrentClass { get; set; }
        public SwpClass ClassToMove { get; set; }


    }
}
