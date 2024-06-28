using InspirationalQuotes.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirationalQuotes.Application.Services
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteDto>> GetQuotesAsync();
        Task<QuoteDto> GetQuoteByIdAsync(int id);
        Task<List<CreatedQuoteResponse>> AddQuotesAsync(IEnumerable<QuoteDto> quoteDtos);
        Task UpdateQuoteAsync(int id, QuoteDto quoteDto);
        Task DeleteQuoteAsync(int id);
        Task<IEnumerable<QuoteDto>> SearchQuotesAsync(SearchQuotesRequestDto request);
    }
}
