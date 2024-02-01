using AutoMapper;
using Entities;

namespace CustomerServiceCampaign.Api;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AgentForCreateDto, Agent>();
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<Agent, AgentDto>();
        CreateMap<Customer, CustomerDto>();


    }
}
