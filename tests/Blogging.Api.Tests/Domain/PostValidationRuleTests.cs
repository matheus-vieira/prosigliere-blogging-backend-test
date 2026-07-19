using Blogging.Domain.Validation;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies shared post and comment validation rules.
/// </summary>
[TestClass]
public sealed class PostValidationRuleTests
{
    /// <summary>
    /// Confirms that external whitespace is normalized.
    /// </summary>
    [TestMethod]
    public void NormalizeTitleTrimsExternalWhitespace()
    {
        var result = PostValidationRules.NormalizeTitle("  A title  ");

        Assert.AreEqual("A title", result);
    }

    /// <summary>
    /// Confirms that oversized values are rejected.
    /// </summary>
    [TestMethod]
    public void NormalizeContentRejectsOversizedValue()
    {
        var value = new string('x', PostValidationRules.ContentMaxLength + 1);

        Assert.ThrowsExactly<ArgumentException>(
            () => PostValidationRules.NormalizeContent(value));
    }

    /// <summary>
    /// Confirms that comment content is required.
    /// </summary>
    [TestMethod]
    public void NormalizeCommentContentRejectsWhitespace()
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => PostValidationRules.NormalizeCommentContent(" "));
    }
}
