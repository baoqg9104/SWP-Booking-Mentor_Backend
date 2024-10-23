using SWP391_Mentor_Booking_System_Data;
using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWP391_Mentor_Booking_System_Data.DTO.SwpClass;
using SWP391_Mentor_Booking_System_Data.DTO.Semester;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class SkillService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public SkillService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SkillDTO>> GetAllSkillsAsync()
        {
            return await _context.Skills
                .Select(s => new SkillDTO
                {
                    SkillId = s.SkillId,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<SkillDTO> GetSkillByIdAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return null;

            return new SkillDTO
            {
                SkillId = skill.SkillId,
                Name = skill.Name
            };
        }

        public async Task<bool> AddSkillAsync(CreateSkillDTO cretaeskillDto)
        {
            var skill = new Skill
            {
                SkillId = cretaeskillDto.SkillId,
                Name = cretaeskillDto.Name
            };

            _context.Skills.Add(skill);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSkillByIdAsync( UpdateSkillDTO updateSkillDto)
        {
            var existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillId == updateSkillDto.SkillId);
            if (existingSkill == null)
                return false;

            existingSkill.SkillId = updateSkillDto.SkillId;
            existingSkill.Name = updateSkillDto.Name;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSkillAsync(int skillId)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillId == skillId);

            if (skill == null)
                return false;

            _context.Skills.Remove(skill);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddMentorSkillAsync(AddMentorSkillDTO dto)
        {
            // Kiểm tra Mentor và Skill có tồn tại không
            var mentor = await _context.Mentors.FindAsync(dto.MentorId);
            var skill = await _context.Skills.FindAsync(dto.SkillId);

            if (mentor == null || skill == null)
            {
                throw new Exception("Mentor hoặc Skill không tồn tại.");
            }

            var mentorSkill = new MentorSkill
            {
                MentorId = dto.MentorId,
                SkillId = dto.SkillId,
                Level = dto.Level
            };

            _context.MentorSkills.Add(mentorSkill);
            await _context.SaveChangesAsync();
        }
    }
}
