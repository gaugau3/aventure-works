using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Api.Dtos;

public class MonthlyReportDto : IValidatableObject
{
    public int? Year { get; set; }
    public int? Month { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Year < 2000 || Year > DateTime.Now.Year + 5)
        {
            yield return new ValidationResult("Year must be between 2000 and current year + 5.", [nameof(Year)]);
        }

        if (Month < 1 || Month > 12)
        {
            yield return new ValidationResult("Month must be between 1 and 12.", [nameof(Month)]);
        }

        // Optional: future month
        if (Year == DateTime.Now.Year && Month > DateTime.Now.Month)
        {
            yield return new ValidationResult("Cannot query a future month.", [nameof(Month), nameof(Year)]);
        }
    }
}
