using Microsoft.AspNetCore.Identity;

namespace FitQuest.Api.Exceptions;

/// <summary>
/// Exception thrown when Identity operations fail with specific error results
/// </summary>
public class IdentityResultException : Exception
{
    public IEnumerable<string> Errors { get; }

    public IdentityResultException(IdentityResult result) 
        : base($"Identity operation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}")
    {
        Errors = result.Errors.Select(e => e.Description);
    }

    public IdentityResultException(IEnumerable<string> errors) 
        : base($"Identity operation failed: {string.Join(", ", errors)}")
    {
        Errors = errors;
    }

    public IdentityResultException(string message, IEnumerable<string> errors) 
        : base(message)
    {
        Errors = errors;
    }
}