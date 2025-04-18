using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using R2ETien.MVC.

namespace R2ETien.MVC.Tests;

public class GlobalExceptionHandlingMiddlewareTests
{
private readonly Mock<ILogger<GlobalExceptionHandlingMiddleware>> _loggerMock;
        private readonly DefaultHttpContext _httpContext;

        public GlobalExceptionHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            _httpContext = new DefaultHttpContext();
        }

        private RequestDelegate CreateRequestDelegate()
        {
            return context => throw new Exception("Test exception");
        }

        [Fact]
        public async Task Invoke_ShouldHandleKeyNotFoundException()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlingMiddleware(CreateRequestDelegate(), _loggerMock.Object);
            _httpContext.Response.Body = new System.IO.MemoryStream(); // Use a memory stream to capture the response

            // Act
            var ex = new KeyNotFoundException("Not found item");
            var next = async (HttpContext context) => throw ex;
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);
            _loggerMock.Verify(l => l.LogWarning(It.IsAny<Exception>(), "Not found: {Message}", ex.Message), Times.Once);
        }

        [Fact]
        public async Task Invoke_ShouldHandleArgumentException()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlingMiddleware(CreateRequestDelegate(), _loggerMock.Object);
            _httpContext.Response.Body = new System.IO.MemoryStream();

            // Act
            var ex = new ArgumentException("Invalid argument");
            var next = async (HttpContext context) => throw ex;
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
            _loggerMock.Verify(l => l.LogWarning(It.IsAny<Exception>(), "Bad request: {Message}", ex.Message), Times.Once);
        }

        [Fact]
        public async Task Invoke_ShouldHandleUnauthorizedAccessException()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlingMiddleware(CreateRequestDelegate(), _loggerMock.Object);
            _httpContext.Response.Body = new System.IO.MemoryStream();

            // Act
            var ex = new UnauthorizedAccessException("Access denied");
            var next = async (HttpContext context) => throw ex;
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, _httpContext.Response.StatusCode);
            _loggerMock.Verify(l => l.LogWarning(It.IsAny<Exception>(), "Unauthorized: {Message}", ex.Message), Times.Once);
        }

        [Fact]
        public async Task Invoke_ShouldHandleGeneralException()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlingMiddleware(CreateRequestDelegate(), _loggerMock.Object);
            _httpContext.Response.Body = new System.IO.MemoryStream();

            // Act
            var ex = new Exception("Unhandled exception");
            var next = async (HttpContext context) => throw ex;
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
            _loggerMock.Verify(l => l.LogError(It.IsAny<Exception>(), "Unhandled exception"), Times.Once);
        }
    }
}
