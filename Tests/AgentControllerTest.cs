using Contracts;
using CustomerServiceCampaign.Api;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests;

[TestFixture]
public class AgentControllerTest
{
    private Mock<IAgentService> _agentServiceMock;
    private AgentController _agentController;

    [SetUp]
    public void Setup()
    {
        _agentServiceMock = new Mock<IAgentService>();
        _agentController = new AgentController(_agentServiceMock.Object);
    }

    [Test]
    public async Task GetAgents_ReturnsOkResult()
    {
        // Arrange
        _agentServiceMock.Setup(x => x.GetAgentsList()).ReturnsAsync(new List<AgentDto>());

        // Act
        var result = await _agentController.GetAgents();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task CreateAgent_WithValidAgent_ReturnsOkResult()
    {
        // Arrange
        var agentDto = new AgentForCreateDto 
            { 
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = "123456789" 
            };
            
        _agentServiceMock.Setup(x => x.CreateAgent(It.IsAny<AgentForCreateDto>()))
            .ReturnsAsync(new AgentForCreateDto());

        // Act
        var result = await _agentController.CreateAgent(agentDto);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public async Task CreateAgent_WithNullAgent_ReturnsBadRequestResult()
    {
        // Arrange
        AgentForCreateDto agentDto = null;

        // Act
        var result = await _agentController.CreateAgent(agentDto);
        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task GetAgentById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var agentId = Guid.NewGuid();
        var agentDto = new AgentDto 
            { 
                    Id = agentId,
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = "123456789" 
            };

         _agentServiceMock.Setup(x => x.GetAgentById(agentId)).ReturnsAsync(agentDto);

        // Act
        var result = await _agentController.GetAgentById(agentId.ToString());

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual(agentDto, (result as OkObjectResult).Value);
    }

    [Test]
    public async Task GetAgentById_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = "invalidId";

        // Act
        var result = await _agentController.GetAgentById(invalidId);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.AreEqual("Invalid ID value", (result as BadRequestObjectResult).Value);
    }

    [Test]
    public async Task GetAgentBySsn_WithValidSsn_ReturnsOkResult()
    {
        // Arrange
        var agentSsn = "123456789";
        var agentDto = new AgentDto 
            { 
                Id = Guid.NewGuid(),
                FirstName = "John", 
                LastName = "Doe", 
                Ssn = agentSsn 
            };
        
        _agentServiceMock.Setup(x => x.GetAgentBySsn(agentSsn)).ReturnsAsync(agentDto);

        // Act
        var result = await _agentController.GetAgentBySsn(agentSsn);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual(agentDto, (result as OkObjectResult).Value);
    }

    [Test]
    public async Task GetAgentBySsn_WithInvalidSsn_ReturnsBadRequest()
    {
        // Arrange
        var noExistSsn = "123456789";

        // Act
        var result = await _agentController.GetAgentBySsn(noExistSsn);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.AreEqual($"Agent with ssn({noExistSsn}) doesn't exist", (result as BadRequestObjectResult).Value);
    }
}
