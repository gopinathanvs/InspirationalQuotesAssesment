using Microsoft.AspNetCore.Mvc;
using InspirationalQuotes.Application.DTOs;
using InspirationalQuotes.Application.Services;

namespace InspirationalQuotes.API.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;
        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        //api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotes()
        {
            var quotes = await _quoteService.GetQuotesAsync();
            return Ok(quotes);
        }

        // GET: api/Quotes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteDto>> GetQuote(int id)
        {
            var quote = await _quoteService.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<List<CreatedQuoteResponse>>> PostQuote(IEnumerable<QuoteDto> quoteDtos)
        {
            var createdQuotes = await _quoteService.AddQuotesAsync(quoteDtos);
            return CreatedAtAction(nameof(PostQuote), createdQuotes);
        }

        // PUT: api/Quotes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, QuoteDto quoteDto)
        {
            try
            {
                await _quoteService.UpdateQuoteAsync(id, quoteDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Quotes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                await _quoteService.DeleteQuoteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Quotes/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> SearchQuotes(SearchQuotesRequestDto request)
        {
            var quotes = await _quoteService.SearchQuotesAsync(request);
            return Ok(quotes);
        }
    }
}
