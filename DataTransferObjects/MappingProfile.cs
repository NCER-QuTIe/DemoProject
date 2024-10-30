using Entities.Models;
using AutoMapper;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
namespace Service;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<QTITestCreationDTO, QTITest>().ForMember("Uploaded", opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<QTITest, QTITestDTO>();
    }
}
