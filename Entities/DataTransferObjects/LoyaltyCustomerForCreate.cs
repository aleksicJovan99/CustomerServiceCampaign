using System.ComponentModel.DataAnnotations;

namespace Entities;
public class LoyaltyCustomerForCreate
{

    [Required(ErrorMessage = "SSN is required field")]
    [MaxLength(9)]
    [MinLength(9)]
    [RegularExpression(@"^\d+$", ErrorMessage = "The property must contain only numbers.")]
    public string Ssn { get; set; }
}
