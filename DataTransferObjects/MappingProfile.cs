﻿using Entities.Models;
using AutoMapper;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
namespace Service;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<QTITestCreationDTO, QTITest>().ForMember("Uploaded", opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<QTITest, QTITestCreationDTO>();
        CreateMap<QTITest, QTITestDTO>();

        CreateMap<FeedbackCreationDTO, Feedback>().ForMember("Uploaded", opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<Feedback, FeedbackDTO>();

        CreateMap<ExternalTestCreationDTO, ExternalTest>().ForMember("Uploaded", opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<ExternalTest, ExternalTestCreationDTO>();
        CreateMap<ExternalTest, ExternalTestDTO>();
    }
}
