using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SecureNote.Application.DTOs;
using SecureNote.Application.Exceptions;
using SecureNote.Application.Interfaces;
using SecureNote.Application.Services;
using SecureNote.Domain.Entities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SecureNote.Application.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("BenimCokGizliVeUzunJwtAnahtarim123!");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("SecureNoteAPI");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("SecureNoteUsers");
            _mockConfig.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

            _authService = new AuthService(
                _mockUserRepo.Object,
                _mockPasswordHasher.Object,
                _mockConfig.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var request = new RegisterRequest { Username = "Emir", Email = "emir@test.com", Password = "Password123" };

            _mockUserRepo.Setup(repo => repo.GetByUsernameAsync(request.Username))
                         .ReturnsAsync(new User { Username = "Emir" });

            // Act
            Func<Task> act = async () => await _authService.RegisterAsync(request);

            
            await act.Should().ThrowAsync<ApplicationException>()
                     .WithMessage("Bu kullanıcı adı zaten kullanımda.");
        }
    }
}

