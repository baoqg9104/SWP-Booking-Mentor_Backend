using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }  // Khóa chính của bảng RefreshToken
        public string Token { get; set; }  // Refresh Token
        public DateTime ExpiryDate { get; set; }  // Thời gian hết hạn của token

        public string UserName { get; set; }  // Khóa ngoại liên kết với User

        public User User { get; set; }  // Navigation property để liên kết với User
    }

}
