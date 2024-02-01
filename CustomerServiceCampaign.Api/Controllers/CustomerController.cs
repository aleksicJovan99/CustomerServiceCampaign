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

    [HttpGet("id", Name = "GetCustomersById"), Authorize]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        //Check if ID value is correct
        if (Guid.TryParse(id, out Guid guidValue))
        {
            var customer = await _service.GetCustomerById(guidValue);

            if(customer == null) return BadRequest($"Customer with id({id}) doesn't exist");

            return Ok(customer);
        }
        else
        {
            return BadRequest("Invalid ID value");
        }
    }

    [HttpGet("ssn", Name = "GetCUstomerBySsn"), Authorize]
    public async Task<IActionResult> GetCustomerBySsn(string ssn)
    {
        var customer = await _service.GetCustomerBySsn(ssn);

        if(customer == null) return BadRequest($"Customer with ssn({ssn}) doesn't exist");

        return Ok(customer);
        
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
    public async Task<IActionResult> UpdateCustomers(string updateFrom)
    { 
        if (updateFrom.Trim() != "csv" && updateFrom.Trim() != "source") return BadRequest("Invalid parameters. Choose from (source, csv)");

        var connectionString = _configuration.GetConnectionString("sqlConnection");

        var isUpdated = await _service.UpdateCustomersTable(connectionString, updateFrom);

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

    [HttpPost("csv"), Authorize]
    public async Task<IActionResult> ImportCsvCustomers([FromForm] IFormFileCollection file) 
    {
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        await _service.ImportCsvCustomers(file[0].OpenReadStream(), connectionString);

        return Ok();
    }
}
