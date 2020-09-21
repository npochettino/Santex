using AutoMapper;
using Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Mapping
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<JObject, Competition>()
            .ForMember("Code", cfg => { cfg.MapFrom(jo => jo["code"]); })
            .ForMember("Name", cfg => { cfg.MapFrom(jo => jo["name"]); });

            CreateMap<JObject, Team>()
                .ForMember("Name", cfg => { cfg.MapFrom(jo => jo["name"]); })
                .ForMember("ShortName", cfg => { cfg.MapFrom(jo => jo["shortName"]); });

        }
    }
}
