using AutoMapper;
using Entities;

namespace CustomerServiceCampaign.Api;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AgentForCreateDto, Agent>();
    }
}
