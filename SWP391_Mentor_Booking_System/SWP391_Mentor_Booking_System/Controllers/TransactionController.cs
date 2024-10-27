using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.Transaction;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("get-transactions-by-mentorId/{mentorId}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByMentorId(string mentorId)
        {
            var transactions = await _transactionService.GetTransactionsByMentorIdAsync(mentorId);
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        [HttpGet("get-transactions-by-groupId/{groupId}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByGroupIdAsync(string groupId)
        {
            var transactions = await _transactionService.GetTransactionsByGroupIdAsync(groupId);
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound();
            }
            return Ok(transactions);
        }
    }
}
