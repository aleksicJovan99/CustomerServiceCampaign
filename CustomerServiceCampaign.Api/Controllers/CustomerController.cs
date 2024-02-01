using System.Reflection.Metadata.Ecma335;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet(Name = "GetCustomers"), Authorize]
    public async Task<IActionResult> GetCustomers()
    { 
        var result = await _service.GetCustomersList();

        if (result == null) { return BadRequest(); }
            
        return Ok(result);
                
    }

        // Imports Customers from source
    [HttpPost("source", Name = "ImportSourceCustomers"), Authorize]
    public async Task<IActionResult> ImportSourceCustomers()
    { 
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        await _service.ImportSourceCustomers(connectionString);
            
        return Ok();
                
    }


    // Updates table of Customers with source data
    [HttpPost("update", Name = "UpdateCustomers"), Authorize]
    public async Task<IActionResult> UpdateCustomers()
    { 
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        var isUpdated = await _service.UpdateCustomersTable(connectionString);

        if (isUpdated) return Ok("New data has been imported");

        return Ok("The data was already updated");
                    
    }

    [HttpPost("loyalty"), Authorize(Roles = "agent")]
    public async Task<IActionResult> CreateLoyaltyCustomer([FromBody] LoyaltyCustomerForCreate loyaltyCustomer)
    {
        string authorizationHeader = HttpContext.Request.Headers["Authorization"]; // Use HttpContext to access the Request object

            // Check if the Authorization header is present and in the correct format
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return BadRequest("Invalid Authorization header");
            }

            // Extract the token from the Authorization header
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var customer = await _service.CreateLoyaltyCustomer(loyaltyCustomer, token);

            if (customer == null) return BadRequest("Customer can't be added to the Loyalty club ");

            return Ok();
    }
}
