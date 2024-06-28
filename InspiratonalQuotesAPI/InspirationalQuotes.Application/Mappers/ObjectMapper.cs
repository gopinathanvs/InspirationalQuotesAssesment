using AutoMapper;
using InspirationalQuotes.Domain.Entities;
using InspirationalQuotes.Application.DTOs;


namespace InspirationalQuotes.Application.Mappers
{
    public class ObjectMapper : Profile
    {
        public ObjectMapper()
        {

            CreateMap<Quote, QuoteDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuoteTags.Select(qt => qt.Tag.TagName).ToList()));

            CreateMap<QuoteDto, Quote>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore mapping Id from QuoteDto
                        .ForMember(dest => dest.QuoteTags, opt => opt.MapFrom(src =>
                        src.Tags.Select(tagName => new QuoteTag
                        {
                            Tag = new Tag { TagName = tagName }
                        }).ToList()));
            CreateMap<SearchQuotesRequestDto, SearchQuotesRequestDto>().ReverseMap();

            CreateMap<QuoteDto, CreatedQuoteResponse>()
                .ForMember(dest => dest.Quote, opt => opt.MapFrom(src => src));

        }
    }
}
