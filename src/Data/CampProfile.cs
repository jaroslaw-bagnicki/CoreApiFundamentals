using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(d => d.Venue, o => o.MapFrom(m => m.Location.VenueName));
            CreateMap<Talk, TalkModel>();
            CreateMap<Speaker, SpeakerModel>();
        }
    }
}
