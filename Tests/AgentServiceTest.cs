using Moq;
using Contracts;
using AutoMapper;
using Service;
using Entities;

namespace Tests;

[TestFixture]
public class AgentServiceTests
{
    // Mock objects for repository and mapper
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private IAgentService _agentService;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
        _agentService = new AgentService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task CreateAgent_ValidAgent_ReturnsAgentDto()
    {
        // Arrange
        var agentDto = new AgentForCreateDto { Ssn = "123456789" };
        var encryptedSsn = EncryptHelper.EncryptDataBase64(agentDto.Ssn);

        // Mock repository method to return null since agent does not exist
        _repositoryMock.Setup(r => r.Agent.GetAgentBySsnAsync(encryptedSsn))
            .ReturnsAsync((Agent)null);

        // Mock mapper to return a new Agent object
        _mapperMock.Setup(m => m.Map<Agent>(agentDto))
            .Returns(new Agent());

        // Act
        var result = await _agentService.CreateAgent(agentDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(agentDto, result);
    }

    [Test]
    public async Task CreateAgent_AgentExist_ReturnsNull()
    {
        // Arrange
        var agentDto = new AgentForCreateDto { Ssn = "123456789" };
        var encryptedSsn = "MTIzNDU2Nzg5";

        // Mock repository method to return an existing Agent with the same SSN
        _repositoryMock.Setup(r => r.Agent.GetAgentBySsnAsync(encryptedSsn))
            .ReturnsAsync(new Agent { Ssn = "MTIzNDU2Nzg5" });

        // Act
        var result = await _agentService.CreateAgent(agentDto);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetAgentsList_ReturnsListOfAgentDtos()
    {
        // Arrange
        var agents = new List<Agent> { new Agent { Ssn = "MTIzNDU2Nzg5" } };
        var agentDto = new AgentDto { Ssn = "123456789" };

        // Mock repository method to return a list of agents
        _repositoryMock.Setup(r => r.Agent.GetAgentsAsync())
            .ReturnsAsync(agents);

        _mapperMock.Setup(m => m.Map<AgentDto>(It.IsAny<Agent>()))
            .Returns(agentDto);

        // Act
        var result = await _agentService.GetAgentsList();
        var count = result.ToList().Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, count);
        Assert.AreEqual(agentDto, result.Where(a => a.Ssn == agentDto.Ssn).SingleOrDefault());
    }

    [Test]
        public async Task GetAgentById_WithValidId_ReturnsAgentDto()
        {
            // Arrange
            var agentId = Guid.NewGuid();

            var agent = new Agent 
                { 
                    Id = agentId,
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = "MTIzNDU2Nzg5" 
                };

            var agentDto = new AgentDto 
            { 
                    Id = agent.Id,
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = "123456789" 
            };

            _repositoryMock.Setup(r => r.Agent.GetAgentByIdAsync(agentId)).ReturnsAsync(agent);

            _mapperMock.Setup(m => m.Map<AgentDto>(It.IsAny<Agent>()))
            .Returns(agentDto);

            // Act
            var result = await _agentService.GetAgentById(agentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<AgentDto>(result);
            Assert.AreEqual(agentId, result.Id);
        }

        [Test]
        public async Task GetAgentBySsn_WithValidSsn_ReturnsAgentDto()
        {
            // Arrange
            var agentSsn = "123456789";
            var encryptedSsn = EncryptHelper.EncryptDataBase64(agentSsn);

            var agent = new Agent 
                { 
                    Id = Guid.NewGuid(),
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = "MTIzNDU2Nzg5" 
                };
                var agentDto = new AgentDto 
                { 
                    Id = agent.Id,
                    FirstName = "John", 
                    LastName = "Doe", 
                    Ssn = agentSsn 
                };

            _repositoryMock.Setup(r => r.Agent.GetAgentBySsnAsync(encryptedSsn)).ReturnsAsync(agent);

            _mapperMock.Setup(m => m.Map<AgentDto>(It.IsAny<Agent>()))
            .Returns(agentDto);

            // Act
            var result = await _agentService.GetAgentBySsn(agentSsn);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<AgentDto>(result);
            Assert.AreEqual(agentSsn, result.Ssn);
        }
}
