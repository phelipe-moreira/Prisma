using System.Text.Json;
using FluentAssertions;
using Prisma.Application.Extensions;
using Prisma.Domain.Entities;
using Prisma.Domain.Enums;

namespace Prisma.Tests.Application.Extensions;

public class PostExtensionsTests
{
    [Test]
    public void ToResponse_ShouldMapCommentsWithoutEntityNavigations()
    {
        var post = Post.Create(Guid.NewGuid(), "Post", "Conteúdo", PostStatus.Published);
        var comment = Comment.Create(post.Id, Guid.NewGuid(), null, "Comentário");
        comment.Post = post;
        post.Comments.Add(comment);

        var response = post.ToResponse();

        response.Comments.Should().ContainSingle().Which.Should().BeEquivalentTo(new
        {
            comment.Id,
            comment.PostId,
            comment.UserId,
            comment.ParentCommentId,
            comment.Content,
            comment.CreatedAt,
            comment.IsRemoved
        });
        JsonSerializer.Serialize(response).Should().NotBeNullOrEmpty();
    }
}
