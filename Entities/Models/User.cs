using Microsoft.AspNetCore.Identity;

namespace Entities;
public class User : IdentityUser
{
    public string SSN { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
}
