using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int TopicId { get; set; }
        public int ClassId { get; set; }
        public int WalletPoint { get; set; }
        public DateTime CreatedDate { get; set; }

        public Topic Topic { get; set; }
        public SwpClass SwpClass { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<BookingSlot> BookingSlots { get; set; }
    }

}
