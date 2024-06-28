using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirationalQuotes.Application.DTOs
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public string QuoteText { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
    }

    public class SearchQuotesRequestDto
    {
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public string Text { get; set; }
    }

    public class CreatedQuoteResponse
    {
        public QuoteDto Quote { get; set; }
    }
}
