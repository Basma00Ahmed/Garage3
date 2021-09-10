using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Members;

namespace Garage3.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
             CreateMap<Member, MembersVehiclesViewModel>()
            .ForMember(
                    mv => mv.NoOfVehicles,
                    from => from.MapFrom(v => v.Vehicles.Count));
        }
    }
}
