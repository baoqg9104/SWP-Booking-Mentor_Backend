using Microsoft.EntityFrameworkCore;
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

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenByUserIdAsync(string userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
