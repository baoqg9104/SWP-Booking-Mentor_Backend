using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO
{
    public class UpdateUserDTO
    {
        public string role {  get; set; }
        public string Id { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MeetUrl { get; set; }
    }
}
