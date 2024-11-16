using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Xunit;
using Moq;
using Backend.Services;
using Domain.DTOs;
using Backend.SecurityLogic;
using Microsoft.AspNetCore.Http;

namespace UnitTests
{
    public class SanitizerTests
    {
        [Fact]
        public void Sanitize_CommentWithScriptTag_ShouldRemoveScriptTag()
        {
            // Arrange
            var commentDto = new CommentDTO
            {
                Text = "<script>alert('XSS')</script> This is a comment."
            };

            // Act
            var sanitizedComment = Sanitizer.Sanitize(commentDto);

            // Assert
            Assert.DoesNotContain("<script>", sanitizedComment.Text);
            Assert.Contains("This is a comment.", sanitizedComment.Text);
        }

        [Fact]
        public void Sanitize_CommentWithHtmlTag_ShouldRemoveHtmlTags()
        {
            // Arrange
            var commentDto = new CommentDTO
            {
                Text = "<b>This is a comment.</b>"
            };

            // Act
            var sanitizedComment = Sanitizer.Sanitize(commentDto);

            // Assert
            Assert.DoesNotContain("<b>", sanitizedComment.Text);
            Assert.DoesNotContain("</b>", sanitizedComment.Text);
            Assert.Contains("This is a comment.", sanitizedComment.Text);
        }



        [Fact]
        public void PostComment_ValidComment_ShouldSanitizeAndAddComment()
        {
            // Arrange
            var mockDeckService = new Mock<IDeckService>();
            var deckId = 1;
            var commentDto = new CommentDTO
            {
                Text = "<script>alert('XSS')</script> Nice deck!"
            };

            // Act
            var sanitizedComment = Sanitizer.Sanitize(commentDto);
            mockDeckService.Object.AddComment(sanitizedComment);

            // Assert
            mockDeckService.Verify(s => s.AddComment(It.Is<CommentDTO>(c => !c.Text.Contains("<script>"))), Times.Once);
        }

        [Fact]
        public void GetComments_ShouldReturnSanitizedComments()
        {
            // Arrange
            var mockDeckService = new Mock<IDeckService>();
            var deckId = 1;
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Text = "<script>alert('XSS')</script> First comment." },
                new CommentDTO { Text = "<b>Second comment.</b>" }
            };

            mockDeckService.Setup(ds => ds.GetCommentsByDeckId(deckId)).Returns(comments);

            // Act
            var sanitizedComments = Sanitizer.Sanitize(mockDeckService.Object.GetCommentsByDeckId(deckId));

            // Assert
            Assert.All(sanitizedComments, comment =>
            {
                Assert.DoesNotContain("<script>", comment.Text);
                Assert.DoesNotContain("<b>", comment.Text);
                Assert.DoesNotContain("</b>", comment.Text);
            });
        }
    }
}
