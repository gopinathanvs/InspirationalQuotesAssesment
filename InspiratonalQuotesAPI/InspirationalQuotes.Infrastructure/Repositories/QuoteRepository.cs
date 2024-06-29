using Microsoft.EntityFrameworkCore;
using InspirationalQuotes.Application.DTOs;
using InspirationalQuotes.Application.Repositories;
using InspirationalQuotes.Domain.Entities;
using InspirationalQuotes.Infrastructure.Data;


namespace InspirationalQuotes.Infrastructure.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {

        private readonly QuoteContext _context;
        public QuoteRepository(QuoteContext context)
        {

            _context = context ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Quote>> GetQuotesAsync()
        {
            return await _context.Quotes
                .Include(q => q.QuoteTags)
                .ThenInclude(qt => qt.Tag)
                .ToListAsync();
        }

        public async Task<Quote> GetQuoteByIdAsync(int id)
        {
            return await _context.Quotes
                .Include(q => q.QuoteTags)
                .ThenInclude(qt => qt.Tag)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddQuoteAsync(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuoteAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuoteAsync(Quote quote)
        {
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Quote>> SearchQuotesAsync(SearchQuotesRequestDto request)
        {
            var query = _context.Quotes
                .Include(q => q.QuoteTags)
                .ThenInclude(qt => qt.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Author))
            {
                query = query.Where(q => q.Author.ToLower().Contains(request.Author.ToLower()));
            }

            if (request.Tags != null && request.Tags.Count > 0)
            {
                var filteredTags = request.Tags.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                if (filteredTags.Count > 0)
                {
                    var normalizedTags = request.Tags.Select(t => t.ToLower()).ToList();
                    query = query.Where(q => q.QuoteTags.Any(qt => normalizedTags.Contains(qt.Tag.TagName.ToLower())));
                }
            }

            if (!string.IsNullOrEmpty(request.Text))
            {
                query = query.Where(q => q.QuoteText.ToLower().Contains(request.Text.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> AddTagAsync(string tagName)
        {
            var existingTags = await _context.Tags.ToListAsync();

            var existingTag = existingTags.FirstOrDefault(t => string.Equals(t.TagName, tagName, StringComparison.OrdinalIgnoreCase));
            if (existingTag == null)
            {
                var newTag = new Tag { TagName = tagName };
                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();

                return newTag;
            }
            else
            {
                return existingTag;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
