using MediatR;

namespace Application.Tags.Command.CheckForTags
{
    public class CheckForTagsCommand : IRequest
    {
        public string Content { get; set; }
    }
}
