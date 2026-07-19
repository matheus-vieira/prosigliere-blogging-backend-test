using Blogging.Domain.Posts;
using Bogus;

namespace Blogging.Api.Tests.TestData;

/// <summary>
/// Builds deterministic post commands for tests.
/// </summary>
public static class BlogPostCommandFaker
{
    /// <summary>
    /// Creates a seeded post command faker.
    /// </summary>
    /// <returns>A deterministic post command faker.</returns>
    public static Faker<CreateBlogPostCommand> Create()
    {
        return new Faker<CreateBlogPostCommand>()
            .UseSeed(701)
            .RuleFor(command => command.Title, faker => faker.Lorem.Sentence(3))
            .RuleFor(command => command.Content, faker => faker.Lorem.Paragraph());
    }
}
