namespace Application.Chats.Queries.UserChatIds
{
    public class UserChatCheckResponse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public bool IsGroup { get; set; }
    }
}
