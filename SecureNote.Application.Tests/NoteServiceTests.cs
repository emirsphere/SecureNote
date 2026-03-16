using Bogus;
using FluentAssertions;
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
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepo;
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly NoteService _noteService;

        public NoteServiceTests()
        {
            _mockNoteRepo = new Mock<INoteRepository>();
            _mockEncryptionService = new Mock<IEncryptionService>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();

            _noteService = new NoteService(
                _mockNoteRepo.Object,
                _mockEncryptionService.Object,
                _mockCategoryRepo.Object);
        }
        [Fact]
        public async Task GetNoteByIdAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            var noteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _mockNoteRepo.Setup(repo => repo.GetByIdAsync(noteId)).ReturnsAsync((Note?)null);

            Func<Task> act = async () => await _noteService.GetNoteByIdAsync(noteId, userId);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not bulunamadı.");
        }


        [Fact]
        public async Task GetNoteByIdAsync_ShouldThrowUnauthorizedException_WhenNoteBelongsToAnotherUser()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var requestingUserId = Guid.NewGuid(); 
            var ownerUserId = Guid.NewGuid();


            var fakeNote = new Note { Id = noteId, UserId = ownerUserId };


            _mockNoteRepo.Setup(repo => repo.GetByIdAsync(noteId)).ReturnsAsync(fakeNote);

            Func<Task> act = async () => await _noteService.GetNoteByIdAsync(noteId, requestingUserId);

            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage("Bu işlem için yetkiniz yok.");
        }


        [Fact]
        public async Task GetNoteByIdAsync_ShouldReturnNoteDto_WhenEverythingIsCorrect()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            // 1. Bogus Kütüphanesi ile Sahte Kategori
            var fakeCategory = new Faker<Category>()
                .RuleFor(c => c.Id, f => categoryId)
                .RuleFor(c => c.CategoryName, f => f.Commerce.Categories(1)[0]) // Rastgele bir e-ticaret kategori adı verir
                .RuleFor(c => c.UserId, f => userId)
                .Generate();

            // 2. Bogus ile Sahte Not
            var fakeNote = new Faker<Note>()
                .RuleFor(n => n.Id, f => noteId)
                .RuleFor(n => n.Title, f => f.Lorem.Sentence()) // Rastgele anlamlı bir başlık cümlesi
                .RuleFor(n => n.Content, f => "8f4d0a7d0caa...")
                .RuleFor(n => n.UserId, f => userId)
                .RuleFor(n => n.CategoryId, f => categoryId)
                .RuleFor(n => n.CreatedOn, f => f.Date.Past())
                .Generate();

            var expectedDecryptedContent = "Bu benim gizli notumun çözülmüş halidir.";


            _mockNoteRepo.Setup(repo => repo.GetByIdAsync(noteId)).ReturnsAsync(fakeNote);

            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(fakeCategory);

            
            _mockEncryptionService.Setup(enc => enc.Decrypt(fakeNote.Content)).Returns(expectedDecryptedContent);

            // Act (Aksiyon)
            var result = await _noteService.GetNoteByIdAsync(noteId, userId);

            // Assert (Doğrulama - FluentAssertions Gücü)
            result.Should().NotBeNull();
            result.Id.Should().Be(noteId);
            result.Title.Should().Be(fakeNote.Title); // Başlık düz metindi, aynı gelmeli
            result.Content.Should().Be(expectedDecryptedContent); // İçerik ÇÖZÜLMÜŞ gelmeli!
            result.CategoryName.Should().Be(fakeCategory.CategoryName);
        }

        [Fact]
        public async Task CreateNoteAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new NoteDto
            {
                Title = "Yeni Not",
                Content = "Gizli içerik",
                CategoryId = Guid.NewGuid() // Rastgele bir ID veriyoruz
            };


            _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(request.CategoryId.Value)).ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _noteService.CreateNoteAsync(request, userId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("Belirtilen kategori bulunamadı.");
        }
        [Fact]
        public async Task CreateNoteAsync_ShouldThrowUnauthorizedException_WhenCategoryBelongsToAnotherUser()
        {
            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var fakeNote = new Faker<Note>()
                .RuleFor(n => n.Id, f => noteId)
                .RuleFor(n => n.Title, f => f.Lorem.Sentence()) 
                .RuleFor(n => n.Content, f => "8f4d0a7d0caa...")
                .RuleFor(n => n.UserId, f => ownerId)
                .RuleFor(n => n.CategoryId, f => categoryId)
                .RuleFor(n => n.CreatedOn, f => f.Date.Past())
                .Generate();
            



        }


    }
}