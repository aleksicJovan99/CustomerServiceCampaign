﻿using Contracts;
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

    [HttpPost(Name = "ImportSourceCustomers")]
    public async Task<IActionResult> ImportSourceCustomers()
    { 
        var connectionString = _configuration.GetConnectionString("sqlConnection");
        await _service.GetSourceCustomers(connectionString);
            
        return Ok();
                
    }
}