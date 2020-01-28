using System;
using System.Linq;
using FluentValidation;

namespace Application.Chats.Command.CreateGroupChat
{
    public class CreateGroupChatValidation : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatValidation()
        {
            RuleFor(f => f.Users)
                .Custom((users, context) =>
                {
                    if (users?.Any() != true)
                    {
                        context.AddFailure("List of users can't be empty");
                    }
                    else
                    {
                        foreach (var user in users)
                        {
                            if (users.Count(f => string.Equals(f, user, StringComparison.OrdinalIgnoreCase)) > 1)
                                context.AddFailure("List of users must be unique");
                        }
                    }
                });
        }
    }
}
