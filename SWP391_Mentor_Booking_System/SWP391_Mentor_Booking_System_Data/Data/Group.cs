    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace SWP391_Mentor_Booking_System_Data.Data
    {
        public class Group
        {
            public string? GroupId { get; set; }
            public string Name { get; set; }

            // Foreign Keys
            public int TopicId { get; set; }
            public Topic Topic { get; set; }

            public string SwpClassId { get; set; }
            public SwpClass SwpClass { get; set; }

            public int WalletPoint { get; set; }
            public DateTime CreatedDate { get; set; }

            // Relationship with BookingSlots
            public ICollection<BookingSlot> BookingSlots { get; set; }

            // Relationship with Students
            public ICollection<Student> Students { get; set; }
        }


    }
