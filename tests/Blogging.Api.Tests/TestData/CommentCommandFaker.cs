using Blogging.Domain.Posts;
using Bogus;

namespace Blogging.Api.Tests.TestData;

/// <summary>
/// Builds deterministic comment commands for tests.
/// </summary>
public static class CommentCommandFaker
{
    /// <summary>
    /// Creates a seeded comment command faker.
    /// </summary>
    /// <returns>A deterministic comment command faker.</returns>
    public static Faker<CreateCommentCommand> Create()
    {
        return new Faker<CreateCommentCommand>()
            .UseSeed(702)
            .RuleFor(command => command.Content, faker => faker.Lorem.Sentence());
    }
}
