using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JobScout.Domain.Tenants.Contracts;

namespace JobScout.Infrastructure.Repository.Tenants;

public class SlugResolver : ISlugResolver
{
    public string ResolveFor(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be null or empty.", nameof(companyName));

        // Normalize and remove diacritics (e.g. Accents)
        var normalized = companyName.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicode = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicode != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        // Convert to lowercase
        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

        // Replace non-alphanumerics with underscores
        slug = Regex.Replace(slug, @"[^a-z0-9]+", "_");

        // Trim underscores from ends
        slug = slug.Trim('_');

        // Optional: enforce max length
        slug = slug.Length > 32 ? slug[..32] : slug;

        return slug;
    }
}
