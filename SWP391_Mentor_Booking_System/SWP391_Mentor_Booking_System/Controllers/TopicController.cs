using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.Topic;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly TopicService _topicService;

        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        // Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicDTO createTopicDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _topicService.CreateTopicAsync(createTopicDto);
            if (!result)
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok();
        }

        // Read by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        // Read all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicService.GetAllTopicsAsync();
            return Ok(topics);
        }

        // Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopicDTO updateTopicDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _topicService.UpdateTopicAsync(updateTopicDto);
            if (!result)
                return NotFound();

            return Ok();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var result = await _topicService.DeleteTopicAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }


}
