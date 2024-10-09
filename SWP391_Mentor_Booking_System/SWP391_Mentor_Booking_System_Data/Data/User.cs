using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class User
    {
        
        public string FullName { get; set; }  
        public string Password { get; set; }  
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }  

        public Role Role { get; set; }  
        public Mentor Mentor { get; set; }  
        public Student Student { get; set; }


    }






}
