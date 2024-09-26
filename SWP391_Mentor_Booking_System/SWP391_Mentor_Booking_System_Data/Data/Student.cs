namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<BookingRequest> BookingRequests { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }

}
