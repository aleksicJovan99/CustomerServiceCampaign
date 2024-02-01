using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CustomerServiceCampaign.Api;
[Route("api/agents")]
[ApiController]
public class AgentController : ControllerBase
{
    private readonly IAgentService _service;

    public AgentController(IAgentService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAgents"), Authorize(Roles = "sysadmin")]
    public async Task<IActionResult> GetAgents()
    { 
        var result = await _service.GetAgentsList();

        if (result == null) { return BadRequest(); }
            
        return Ok(result);
                
    }

    [HttpGet("id", Name = "GetAgentById"), Authorize(Roles = "sysadmin")]
    public async Task<IActionResult> GetAgentById(string id)
    {
        //Check if ID value is correct
        if (Guid.TryParse(id, out Guid guidValue))
        {
            var agent = await _service.GetAgentById(guidValue);

            if(agent == null) return BadRequest($"Agent with id({id}) doesn't exist");

            return Ok(agent);
        }
        else
        {
            return BadRequest("Invalid ID value");
        }
    }

    [HttpGet("ssn", Name = "GetAgentBySsn"), Authorize(Roles = "sysadmin")]
    public async Task<IActionResult> GetAgentBySsn(string ssn)
    {
        var agent = await _service.GetAgentBySsn(ssn);

        if(agent == null) return BadRequest($"Agent with ssn({ssn}) doesn't exist");

        return Ok(agent);
        
    }
    

    [HttpPost(Name = "Create Agent"), Authorize(Roles = "sysadmin")]
    public async Task<IActionResult> CreateAgent(AgentForCreateDto agent)
    {
        if (agent == null) 
        {
            Log.Information("Company object is null");
            return BadRequest("Company object is null");
        }
        

        var result = await _service.CreateAgent(agent);

        if (result == null) 
        {
            Log.Warning($"An agent with this SSN({agent.Ssn}) already exists");
            return BadRequest($"An agent with this SSN({agent.Ssn}) already exists");
        }

        return Ok();
    }



}
