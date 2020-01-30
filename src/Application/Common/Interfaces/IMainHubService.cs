using Application.Common.ViewModels;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMainHubService
    {
        Task SendPost(PostVm post);
        Task SendLikedPost(LikeVm like);
        Task SendPollVote(PollVoteVm pollVote);
        Task SendDeletedPost(long id);
        Task SendMessage(MessageVm message);
    }
}
