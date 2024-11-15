using SWP391_Mentor_Booking_System_Data.DTO.Group;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391_Mentor_Booking_System_Data.Data;
using System.ComponentModel.DataAnnotations;

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

            var groupName = await _context.Groups.AnyAsync(g => g.Name == createGroupDto.Name);

            if (groupName)
            {
                return (false, "Duplicated name");
            }

            // Check if TopicId exists
            var topicExists = await _context.Topics.AnyAsync(t => t.TopicId == createGroupDto.TopicId && t.Status == true);
            if (!topicExists)
                return (false, "TopicId does not exist");

            // Check if SwpClassId exists
            var classExists = await _context.SwpClasses.AnyAsync(c => c.SwpClassId == createGroupDto.SwpClassId && c.Status == true);
            if (!classExists)
                return (false, "SwpClassId does not exist");

            // Check LeaderId
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == createGroupDto.LeaderId);
            if (student == null)
            {
                return (false, "Student does not exist");
            }

            if (student.GroupId != null)
            {
                return (false, "Student has joined the group");
            }


            // Check Group topic in a Class
            var groupTopic = await _context.Groups.AnyAsync(g => g.SwpClassId == createGroupDto.SwpClassId && g.TopicId == createGroupDto.TopicId);
            if (groupTopic)
            {
                return (false, "Dupplicate topic in the same class");
            }

            if (createGroupDto.MemberEmails == null || createGroupDto.MemberEmails.Count < 4)
            {
                return (false, "Minimum number of members is 4");
            }

            if (createGroupDto.MemberEmails.Count > 6) {
                return (false, "Maximum number of members is 6");
            }

            // Auto-generate GroupId
            var lastGroup = await _context.Groups.OrderByDescending(g => g.GroupId).FirstOrDefaultAsync();
            var nextIdNumber = lastGroup == null ? 1 : int.Parse(lastGroup.GroupId.Substring(2)) + 1;
            var groupId = $"GR{nextIdNumber:D3}";

            var group = new Group
            {
                GroupId = groupId,
                Name = createGroupDto.Name,
                TopicId = createGroupDto.TopicId,
                LeaderId = createGroupDto.LeaderId,
                Progress = 0,
                SwpClassId = createGroupDto.SwpClassId,
                WalletPoint = 0,
                CreatedDate = DateTime.Now,
                Status = false
            };

            _context.Groups.Add(group);

            //student.GroupId = group.GroupId;
            foreach (string email in createGroupDto.MemberEmails)
            {
                var stu = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
                if (stu == null) 
                {
                    return (false, "Member not found");
                }
                stu.GroupId = groupId;
            }

            await _context.SaveChangesAsync();
            return (true, group.GroupId);
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
                Progress = group.Progress,
                CreatedDate = group.CreatedDate,
                Status = group.Status,
                LeaderId = group.LeaderId
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
                    Progress = g.Progress,
                    CreatedDate = g.CreatedDate,
                    Status = g.Status,
                    LeaderId = g.LeaderId
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

        public async Task<(bool Success, string Error)> AddMemberAsync(AddMemberDTO addMemberDTO)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == addMemberDTO.GroupId);

            if (group == null)
            {
                return (false, "Group not found");
            }

            if (group.LeaderId != addMemberDTO.LeaderId)
            {
                return (false, "You are not leader");
            }

            var numOfMembers = await _context.Students
                .Where(s => s.GroupId == addMemberDTO.GroupId)
                .CountAsync();


            if (numOfMembers + addMemberDTO.Emails.Count > 6)
            {
                return (false, "Maximum number of members is 6");
            }

            foreach (string email in addMemberDTO.Emails)
            {
                var stu = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
                if (stu == null)
                {
                    return (false, "Member not found");
                }
                stu.GroupId = group.GroupId;
            }  

            await _context.SaveChangesAsync();
            return (true, "");
        }

        public async Task<List<MemberDTO>> GetMembersAsync(string groupId)
        {
            var members = await _context.Students
                .Where(s => s.GroupId == groupId)
                .Select(s => new MemberDTO {
                    StudentId = s.StudentId,
                    FullName = s.StudentName,
                    Email = s.Email,
                    Phone = s.Phone
                })
                .ToListAsync();

            if (members != null)
            {
                return members;
            }

            return new List<MemberDTO> ();
        }

        public async Task UpdateProgressAsync(UpdateProgressDTO updateProgressDTO)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == updateProgressDTO.GroupId);

            if (group == null)
            {
                return;
            }

            group.Progress = updateProgressDTO.Progress;
            await _context.SaveChangesAsync();
        }

        public async Task<(bool success, string error)> UpdateStatusGroupAsync(UpdateStatusGroupDTO dto)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == dto.GroupId);

            if (group == null)
            {
                return (false, "Group not found");
            }

            group.Status = dto.Status;
            group.WalletPoint = 10;
            await _context.SaveChangesAsync();
            return (true, "");
        }

        public async Task<(bool Success, string Error)> LeaveGroupAsync(string studentId)
        {
            // Kiểm tra xem sinh viên có tồn tại không
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
            {
                return (false, "Student not found");
            }

            // Kiểm tra xem sinh viên có thuộc nhóm không
            if (string.IsNullOrEmpty(student.GroupId))
            {
                return (false, "Student is not part of any group");
            }

            // Lấy thông tin nhóm của sinh viên
            var group = await _context.Groups.Include(g => g.Students)
                                              .FirstOrDefaultAsync(g => g.GroupId == student.GroupId);

            if (group == null)
            {
                return (false, "Group not found");
            }

            if (group.LeaderId == student.StudentId)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                return (true, "Deleted group successful");
            }

            student.GroupId = null; // Xoá quan hệ nhóm của sinh viên

            await _context.SaveChangesAsync();
            return (true, "Student has successfully left the group");
        }

        public async Task<(bool Success, string Error)> DeleteMemberAsync(string studentId)
        {
            // Kiểm tra xem sinh viên có tồn tại không
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
            {
                return (false, "Student not found");
            }

            // Kiểm tra xem sinh viên có thuộc nhóm không
            if (string.IsNullOrEmpty(student.GroupId))
            {
                return (false, "Student is not part of any group");
            }

            // Lấy thông tin nhóm của sinh viên
            var group = await _context.Groups.Include(g => g.Students)
                                              .FirstOrDefaultAsync(g => g.GroupId == student.GroupId);

            if (group == null)
            {
                return (false, "Group not found");
            }

            student.GroupId = null; // Xoá quan hệ nhóm của sinh viên

            await _context.SaveChangesAsync();
            return (true, "Deleted member successful");

        }


    }

}
