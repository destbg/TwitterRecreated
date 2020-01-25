using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITwitterDbContext
    {
        DbSet<Bookmark> Bookmarks { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<ChatUser> ChatUsers { get; set; }
        DbSet<LikedPost> LikedPosts { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<PollVote> PollVotes { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<PostImage> PostImages { get; set; }
        DbSet<PollOption> PollOptions { get; set; }
        DbSet<Repost> Reposts { get; set; }
        DbSet<HashTag> HashTags { get; set; }
        DbSet<UserFollow> UserFollowers { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
