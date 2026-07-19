namespace Blogging.Domain.Validation;

/// <summary>
/// Defines and applies validation rules shared by post use cases.
/// </summary>
public static class PostValidationRules
{
    public const int TitleMaxLength = 200;
    public const int ContentMaxLength = 10000;
    public const int CommentContentMaxLength = 2000;

    /// <summary>
    /// Trims and validates a post title.
    /// </summary>
    /// <param name="value">The submitted title.</param>
    /// <returns>The normalized title.</returns>
    public static string NormalizeTitle(string? value)
    {
        return NormalizeRequired(value, "Title", TitleMaxLength);
    }

    /// <summary>
    /// Trims and validates post content.
    /// </summary>
    /// <param name="value">The submitted content.</param>
    /// <returns>The normalized content.</returns>
    public static string NormalizeContent(string? value)
    {
        return NormalizeRequired(value, "Content", ContentMaxLength);
    }

    /// <summary>
    /// Trims and validates comment content.
    /// </summary>
    /// <param name="value">The submitted comment content.</param>
    /// <returns>The normalized comment content.</returns>
    public static string NormalizeCommentContent(string? value)
    {
        return NormalizeRequired(value, "Comment content", CommentContentMaxLength);
    }

    private static string NormalizeRequired(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} is required.", fieldName);
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new ArgumentException(
                $"{fieldName} must be {maxLength} characters or fewer.",
                fieldName);
        }

        return normalized;
    }
}
