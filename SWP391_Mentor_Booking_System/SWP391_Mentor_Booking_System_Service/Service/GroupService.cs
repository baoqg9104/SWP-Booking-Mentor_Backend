using SWP391_Mentor_Booking_System_Data.DTO.Group;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class GroupService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public GroupService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<(bool Success, string Error)> CreateGroupAsync(CreateGroupDTO createGroupDto)
        {
            // Check if TopicId exists
            var topicExists = await _context.Topics.AnyAsync(t => t.TopicId == createGroupDto.TopicId);
            if (!topicExists)
                return (false, "TopicId does not exist");

            var group = new Group
            {
                Name = createGroupDto.Name,
                TopicId = createGroupDto.TopicId,
                SwpClassId = createGroupDto.SwpClassId,
                WalletPoint = 10,
                CreatedDate = DateTime.Now
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        // Read by Id
        public async Task<GroupDTO> GetGroupByIdAsync(string groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Topic)
                .Include(g => g.SwpClass)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
                return null;

            return new GroupDTO
            {
                GroupId = group.GroupId,
                Name = group.Name,
                TopicId = group.TopicId,
                TopicName = group.Topic.Name,
                SwpClassId = group.SwpClassId,
                SwpClassName = group.SwpClass.Name,
                WalletPoint = group.WalletPoint,
                CreatedDate = group.CreatedDate
            };
        }

        // Read all
        public async Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Topic)
                .Include(g => g.SwpClass)
                .Select(g => new GroupDTO
                {
                    GroupId = g.GroupId,
                    Name = g.Name,
                    TopicId = g.TopicId,
                    TopicName = g.Topic.Name,
                    SwpClassId = g.SwpClassId,
                    SwpClassName = g.SwpClass.Name,
                    WalletPoint = g.WalletPoint,
                    CreatedDate = g.CreatedDate
                })
                .ToListAsync();
        }

        // Update
        public async Task<(bool Success, string Error)> UpdateGroupAsync(UpdateGroupDTO updateGroupDto)
        {
            var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == updateGroupDto.GroupId);

            if (existingGroup == null)
                return (false, "Group not found");

            // Check if TopicId exists
            var topicExists = await _context.Topics.AnyAsync(t => t.TopicId == updateGroupDto.TopicId);
            if (!topicExists)
                return (false, "TopicId does not exist");

            existingGroup.Name = updateGroupDto.Name;
            existingGroup.TopicId = updateGroupDto.TopicId;
            existingGroup.SwpClassId = updateGroupDto.SwpClassId;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        // Delete
        public async Task<bool> DeleteGroupAsync(string groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
                return false;

            _context.Groups.Remove(group);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
