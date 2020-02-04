using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class PollVm : IMapFrom<PollOption>
    {
        public long Id { get; set; }
        public string Option { get; set; }
        public int Votes { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<PollOption, PollVm>()
                .ForMember(f => f.Votes, f => f.MapFrom(s => s.Votes.Count));
    }

    public class PollVoteVm : IMapFrom<PollVote>
    {
        public long OptionId { get; set; }
        public long PostId { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<PollVote, PollVoteVm>()
                .ForMember(f => f.PostId, f => f.MapFrom(s => s.Option.PostId));
    }
}
