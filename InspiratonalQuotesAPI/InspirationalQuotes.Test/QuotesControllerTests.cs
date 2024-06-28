using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using InspirationalQuotes.Application.DTOs;
using InspirationalQuotes.Application.Services;
using InspirationalQuotes.API.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace InspirationalQuotes.Test
{
    public class QuotesControllerTests
    {
        private readonly Mock<IQuoteService> _mockQuoteService;
        private readonly QuotesController _controller;

        public QuotesControllerTests()
        {
            _mockQuoteService = new Mock<IQuoteService>();
            _controller = new QuotesController(_mockQuoteService.Object);
        }
    }
}
