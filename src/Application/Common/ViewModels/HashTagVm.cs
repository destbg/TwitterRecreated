using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class HashTagVm : IMapFrom<HashTag>
    {
        public string Tag { get; set; }
        public int Posts { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<HashTag, HashTagVm>();
    }
}
