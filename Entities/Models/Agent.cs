using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class Agent
{
    [Column("AgentId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "First name is required field")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name is required field")]
    public string LastName  { get; set; } = string.Empty;
    [Required(ErrorMessage = "SSN is required field")]
    [MaxLength(9)]
    [RegularExpression(@"^\d+$", ErrorMessage = "The property must contain only numbers.")]
    public string Ssn { get; set; } = string.Empty;

}
