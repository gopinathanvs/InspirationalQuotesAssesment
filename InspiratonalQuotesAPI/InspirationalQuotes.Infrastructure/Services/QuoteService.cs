using AutoMapper;
using InspirationalQuotes.Application.DTOs;
using InspirationalQuotes.Application.Repositories;
using InspirationalQuotes.Application.Services;
using InspirationalQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InspirationalQuotes.Infrastructure.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IMapper _mapper;

        public QuoteService(IQuoteRepository quoteRepository, IMapper mapper)
        {
            _quoteRepository = quoteRepository ?? throw new ArgumentNullException(nameof(quoteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<QuoteDto>> GetQuotesAsync()
        {
            var quotes = await _quoteRepository.GetQuotesAsync();
            return _mapper.Map<IEnumerable<QuoteDto>>(quotes);
        }

        public async Task<QuoteDto> GetQuoteByIdAsync(int id)
        {
            var quote = await _quoteRepository.GetQuoteByIdAsync(id);
            return _mapper.Map<QuoteDto>(quote);
        }

        public async Task<List<CreatedQuoteResponse>> AddQuotesAsync(IEnumerable<QuoteDto> quoteDTOs)
        {
            var createdQuotes = new List<CreatedQuoteResponse>();

            foreach (var quoteDto in quoteDTOs)
            {
                ValidateQuoteDto(quoteDto);

                var quote = _mapper.Map<Quote>(quoteDto);

                await ProcessTagsForQuoteAsync(quote, quoteDto.Tags);

                await _quoteRepository.AddQuoteAsync(quote);

                quoteDto.Id = quote.Id;
                quoteDto.Tags = quote.QuoteTags.Select(qt => qt.Tag.TagName).ToList();

                createdQuotes.Add(new CreatedQuoteResponse
                {
                    Quote = _mapper.Map<QuoteDto>(quote)
                });
            }

            return createdQuotes;
        }

        public async Task UpdateQuoteAsync(int id, QuoteDto quoteDto)
        {
            var quote = await _quoteRepository.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                throw new KeyNotFoundException("Quote not found");
            }

            ValidateQuoteDto(quoteDto);

            _mapper.Map(quoteDto, quote);

            quote.QuoteTags.Clear(); // Clear existing tags
            await ProcessTagsForQuoteAsync(quote, quoteDto.Tags);

            await _quoteRepository.UpdateQuoteAsync(quote);
        }

        public async Task DeleteQuoteAsync(int id)
        {
            var quote = await _quoteRepository.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                throw new KeyNotFoundException("Quote not found");
            }
            await _quoteRepository.DeleteQuoteAsync(quote);
        }

        public async Task<IEnumerable<QuoteDto>> SearchQuotesAsync(SearchQuotesRequestDto request)
        {
            var quotes = await _quoteRepository.SearchQuotesAsync(request);
            return _mapper.Map<IEnumerable<QuoteDto>>(quotes);
        }

        private async Task ProcessTagsForQuoteAsync(Quote quote, List<string> tagNames)
        {
            var distinctTags = tagNames.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var existingTags = await _quoteRepository.GetTagsAsync();

            quote.QuoteTags.Clear();

            foreach (var tagName in distinctTags)
            {
                var existingTag = existingTags.FirstOrDefault(t => string.Equals(t.TagName, tagName, StringComparison.OrdinalIgnoreCase));

                if (existingTag == null)
                {
                    // Add new tag if not found
                    var newTag = await _quoteRepository.AddTagAsync(tagName); // Assuming AddTagAsync adds a single tag
                    quote.QuoteTags.Add(new QuoteTag { TagId = newTag.Id });
                }
                else
                {
                    // Use existing tag
                    quote.QuoteTags.Add(new QuoteTag { TagId = existingTag.Id });
                }
            }
        }

        private void ValidateQuoteDto(QuoteDto quoteDto)
        {
            if (quoteDto == null || string.IsNullOrWhiteSpace(quoteDto.QuoteText) || string.IsNullOrWhiteSpace(quoteDto.Author) || quoteDto.Tags == null || !quoteDto.Tags.Any())
            {
                throw new ArgumentException("Author, QuoteText, and at least one tag must be provided.");
            }
        }
    }
}
