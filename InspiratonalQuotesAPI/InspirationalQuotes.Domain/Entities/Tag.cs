using System.ComponentModel.DataAnnotations;

namespace InspirationalQuotes.Domain.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }

        // Navigation property for many-to-many relationship with quotes
        public ICollection<QuoteTag> QuoteTags { get; set; }
    }
}
