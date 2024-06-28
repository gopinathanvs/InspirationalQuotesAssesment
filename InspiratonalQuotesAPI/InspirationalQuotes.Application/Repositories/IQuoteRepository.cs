using InspirationalQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using InspirationalQuotes.Application.DTOs;

namespace InspirationalQuotes.Application.Repositories
{
    public interface IQuoteRepository
    {
        Task<IEnumerable<Quote>> GetQuotesAsync();
        Task<Quote> GetQuoteByIdAsync(int id);
        Task AddQuoteAsync(Quote quote);
        Task UpdateQuoteAsync(Quote quote);
        Task DeleteQuoteAsync(Quote quote);
        Task<IEnumerable<Quote>> SearchQuotesAsync(SearchQuotesRequestDto request);
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task<Tag> AddTagAsync(string tagName);
        Task SaveChangesAsync();
    }
}
