using SWP391_Mentor_Booking_System_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class RefreshTokenRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public RefreshTokenRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Lưu refresh token
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
        }

        // Lấy refresh token từ database
        public RefreshToken GetByToken(string token)
        {
            return _context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        }

        // Xóa refresh token
        public void RemoveToken(string token)
        {
            var tokenEntity = GetByToken(token);
            if (tokenEntity != null)
            {
                _context.RefreshTokens.Remove(tokenEntity);
                _context.SaveChanges();
            }
        }
    }

}
