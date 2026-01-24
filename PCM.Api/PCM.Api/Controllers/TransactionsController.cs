using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .ToListAsync());
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var transactions = await _context.Transactions.ToListAsync();

            var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var totalExpense = Math.Abs(transactions.Where(t => t.Amount < 0).Sum(t => t.Amount));
            var balance = totalIncome - totalExpense;

            return Ok(new
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction t)
        {
            t.CreatedDate = DateTime.Now;

            _context.Transactions.Add(t);
            await _context.SaveChangesAsync();

            return Ok(t);
        }
    }
}
