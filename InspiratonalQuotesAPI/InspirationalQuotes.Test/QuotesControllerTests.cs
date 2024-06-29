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
        [Fact]
        public async Task GetQuotes_ReturnsListOfQuotes()
        {
            // Arrange
            var quotes = new List<QuoteDto>
            {
                new QuoteDto { Id = 1, Author = "Author 1", QuoteText = "Quote 1", Tags = new List<string> { "Tag1", "Tag2" } },
                new QuoteDto { Id = 2, Author = "Author 2", QuoteText = "Quote 2", Tags = new List<string> { "Tag3", "Tag4" } },
                new QuoteDto { Id = 3, Author = "Author 3", QuoteText = "Quote 3", Tags = new List<string> { "Tag5", "Tag6" } }
            };
            _mockQuoteService.Setup(service => service.GetQuotesAsync()).ReturnsAsync(quotes);

            // Act
            var result = await _controller.GetQuotes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<QuoteDto>>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnQuotes = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(returnValue.Value);
            Assert.Equal(3, returnQuotes.Count());
        }

        [Fact]
        public async Task GetQuote_ReturnsSingleQuote()
        {
            // Arrange
            var quote = new QuoteDto { Id = 1, Author = "Author 1", QuoteText = "Quote 1", Tags = new List<string> { "Tag1", "Tag2" } };
            _mockQuoteService.Setup(service => service.GetQuoteByIdAsync(1)).ReturnsAsync(quote);

            // Act
            var result = await _controller.GetQuote(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuoteDto>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnQuote = Assert.IsType<QuoteDto>(returnValue.Value);
            Assert.Equal(1, returnQuote.Id);
        }

        [Fact]
        public async Task GetQuote_ReturnsNotFound_WhenQuoteDoesNotExist()
        {
            // Arrange
            _mockQuoteService.Setup(service => service.GetQuoteByIdAsync(1)).ReturnsAsync((QuoteDto)null);

            // Act
            var result = await _controller.GetQuote(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuoteDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostQuote_CreatesNewQuote()
        {
            // Arrange
            var newQuotes = new List<QuoteDto>
            {
                new QuoteDto { Author = "New Author", QuoteText = "New Quote", Tags = new List<string> { "Tag1", "Tag2" } },
                new QuoteDto { Author = "New Author1", QuoteText = "New Quote1", Tags = new List<string> { "Tag3", "Tag4" }}
            };
            var createdQuotes = new List<CreatedQuoteResponse>
            {
                new CreatedQuoteResponse { Quote = newQuotes[0] }
            };

            _mockQuoteService.Setup(service => service.AddQuotesAsync(newQuotes)).ReturnsAsync(createdQuotes);

            // Act
            var result = await _controller.PostQuote(newQuotes);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<CreatedQuoteResponse>>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnQuotes = Assert.IsType<List<CreatedQuoteResponse>>(createdAtActionResult.Value);
            Assert.Equal(createdQuotes.Count, returnQuotes.Count);

            for (int i = 0; i < createdQuotes.Count; i++)
            {
                Assert.Equal(createdQuotes[i].Quote.Author, returnQuotes[i].Quote.Author);
                Assert.Equal(createdQuotes[i].Quote.QuoteText, returnQuotes[i].Quote.QuoteText);
                Assert.Equal(createdQuotes[i].Quote.Tags.Count, returnQuotes[i].Quote.Tags.Count);
            }
        }

        [Fact]
        public async Task PostQuote_CreatesNewQuote_InvalidDTO()
        {
            // Arrange
            var newQuotes = new List<QuoteDto>();

            _mockQuoteService.Setup(service => service.AddQuotesAsync(newQuotes))
                             .ThrowsAsync(new ArgumentException("Invalid DTOs")); // Simulate throwing ArgumentException

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var result = await _controller.PostQuote(newQuotes);
            });

            Assert.Equal("Invalid DTOs", exception.Message);
        }

        [Fact]
        public async Task PutQuote_UpdatesExistingQuote()
        {
            // Arrange
            var updatedQuote = new QuoteDto { Id = 1, Author = "Updated Author", QuoteText = "Updated Quote", Tags = new List<string> { "Tag1", "Tag2" } };
            _mockQuoteService.Setup(service => service.UpdateQuoteAsync(1, updatedQuote)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutQuote(1, updatedQuote);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutQuote_ReturnsNotFound_WhenQuoteDoesNotExist()
        {
            // Arrange
            var updatedQuote = new QuoteDto { Id = 1, Author = "Updated Author", QuoteText = "Updated Quote", Tags = new List<string> { "Tag1", "Tag2" } };
            _mockQuoteService.Setup(service => service.UpdateQuoteAsync(1, updatedQuote)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.PutQuote(1, updatedQuote);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteQuote_RemovesQuote()
        {
            // Arrange
            _mockQuoteService.Setup(service => service.DeleteQuoteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteQuote(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteQuote_ReturnsNotFound_WhenQuoteDoesNotExist()
        {
            // Arrange
            _mockQuoteService.Setup(service => service.DeleteQuoteAsync(1)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteQuote(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SearchQuotes_ReturnsMatchingQuotes()
        {
            // Arrange
            var searchRequest = new SearchQuotesRequestDto { Author = "Author 1", Tags = new List<string>(), Text = "" };
            var quotes = new List<QuoteDto>
            {
                new QuoteDto { Id = 1, Author = "Author 1", QuoteText = "Quote 1", Tags = new List<string> { "Tag1", "Tag2" } }
            };
            _mockQuoteService.Setup(service => service.SearchQuotesAsync(searchRequest)).ReturnsAsync(quotes);

            // Act
            var result = await _controller.SearchQuotes(searchRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<QuoteDto>>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnQuotes = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(returnValue.Value);
            Assert.Single(returnQuotes);
        }
    }
}
