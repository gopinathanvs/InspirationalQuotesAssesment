using InspirationalQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirationalQuotes.Domain.Entities
{
    public class QuoteTag
    {
        public int QuoteId { get; set; }
        public Quote Quote { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
