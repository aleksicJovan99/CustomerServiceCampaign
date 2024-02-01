using System.Reflection.Metadata.Ecma335;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.Api;

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{  
    private readonly ICustomerService _service;
    private readonly IConfiguration _configuration;

    public CustomerController(ICustomerService service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
    }

    // Get Customers from source
    [HttpPost("source", Name = "ImportSourceCustomers")]
    public async Task<IActionResult> ImportSourceCustomers()
    { 
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        await _service.ImportSourceCustomers(connectionString);
            
        return Ok();
                
    }

    // Updates table of Customers with source data
    [HttpPost("update", Name = "UpdateCustomers")]
    public async Task<IActionResult> UpdateCustomers()
    { 
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        var isUpdated = await _service.UpdateCustomersTable(connectionString);

        if (isUpdated) return Ok("New data has been imported");

        return Ok("The data was already updated");
            
        
                
    }
}
