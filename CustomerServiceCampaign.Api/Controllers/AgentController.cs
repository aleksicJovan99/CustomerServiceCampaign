using Contracts;
using Entities;
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

    [HttpGet(Name = "Get Agents")]
    public async Task<IActionResult> GetAgents()
    {
        try 
        {
            var result = await _service.GetAgentsList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error($"Something went wrong in the {nameof(GetAgents)} action {ex}");
            return StatusCode(500, "Internal server error");
        }
        
    }

    [HttpPost(Name = "Create Agent")]
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
