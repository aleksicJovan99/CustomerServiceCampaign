using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class LoyaltyCustomer
{
    [ForeignKey("Agent")]
    public Guid AgentId { get; set; }

    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    public DateTime DateAdded { get; set; }


    public Agent Agent { get; set; }
    public Customer Customer { get; set; }
}
