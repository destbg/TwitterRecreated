namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string Ip { get; }
        string UserId { get; }
        bool IsAuthenticated { get; }
    }
}
