using Application.Common.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMainHubService
    {
        Task SendPost(PostVm post);
        Task SendLikedPost(LikeVm like);
        Task SendPollVote(PollVoteVm pollVote);
        Task SendDeletedPost(long postId);
        Task SendMessage(MessageVm message);
        Task AddUserToChat(string userId, ChatVm chat);
        Task AddUsersToChat(IReadOnlyList<string> userIds, ChatVm chat);
    }
}
