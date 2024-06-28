using System.ComponentModel.DataAnnotations;

namespace InspirationalQuotes.Domain.Entities
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuoteText { get; set; }

        [Required]
        public string Author { get; set; }

        public ICollection<QuoteTag> QuoteTags { get; set; }
    }
}
