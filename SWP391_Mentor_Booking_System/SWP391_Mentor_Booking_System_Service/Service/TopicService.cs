using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.Topic;
using SWP391_Mentor_Booking_System_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class TopicService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public TopicService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Create
        public async Task<bool> CreateTopicAsync(CreateTopicDTO createTopicDto)
        {
            var topic = new Topic
            {
                Name = createTopicDto.Name,
                Description = createTopicDto.Description,
                SemesterId = createTopicDto.SemesterId,
                Actors = createTopicDto.Actors,
                Status = createTopicDto.Status
            };

            _context.Topics.Add(topic);
            return await _context.SaveChangesAsync() > 0;
        }

        // Read by Id
        public async Task<TopicDTO> GetTopicByIdAsync(int topicId)
        {
            var topic = await _context.Topics
                .Include(t => t.Semester)
                .FirstOrDefaultAsync(t => t.TopicId == topicId);

            if (topic == null)
                return null;

            return new TopicDTO
            {
                TopicId = topic.TopicId,
                Name = topic.Name,
                Description = topic.Description,
                SemesterId = topic.SemesterId,
                SemesterName = topic.Semester.Name,
                Actors = topic.Actors,
                Status = topic.Status
            };
        }

        // Read all
        public async Task<List<TopicDTO>> GetAllTopicsAsync()
        {
            return await _context.Topics
                .Include(t => t.Semester)
                .Select(t => new TopicDTO
                {
                    TopicId = t.TopicId,
                    Name = t.Name,
                    Description = t.Description,
                    SemesterId = t.SemesterId,
                    SemesterName = t.Semester.Name,
                    Actors = t.Actors,
                    Status = t.Status
                })
                .ToListAsync();
        }

        // Update
        public async Task<bool> UpdateTopicAsync(UpdateTopicDTO updateTopicDto)
        {
            var existingTopic = await _context.Topics.FirstOrDefaultAsync(t => t.TopicId == updateTopicDto.TopicId);

            if (existingTopic == null)
                return false;

            existingTopic.Name = updateTopicDto.Name;
            existingTopic.Description = updateTopicDto.Description;
            existingTopic.SemesterId = updateTopicDto.SemesterId;
            existingTopic.Actors = updateTopicDto.Actors;
            existingTopic.Status = updateTopicDto.Status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete
        public async Task<bool> DeleteTopicAsync(int topicId)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(t => t.TopicId == topicId);

            if (topic == null)
                return false;

            _context.Topics.Remove(topic);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
