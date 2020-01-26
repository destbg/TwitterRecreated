using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Common.Validators
{
    public static class Validators
    {
        public static IRuleBuilderOptions<T, string[]> HasValidPoll<T>(this IRuleBuilder<T, string[]> ruleBuilder) =>
            ruleBuilder.Must(poll =>
            {
                if (poll == null)
                    return true;

                if (poll.Length > 0)
                {
                    foreach (var option in poll)
                    {
                        if (option == null || option.Length > 25)
                            return false;
                    }
                    return true;
                }
                return false;
            }).WithMessage("All options in poll must be less then 25 characters");

        public static IRuleBuilderInitial<T, List<IFormFile>> FilesValidator<T>(this IRuleBuilder<T, List<IFormFile>> ruleBuilder) =>
            ruleBuilder.Custom((files, context) =>
            {
                if (files == null)
                    return;

                if (files.Count > 4)
                    context.AddFailure("You can't upload more then 4 files");
                else if (files.Exists(f => f.ContentType.StartsWith("video")) && files.Count > 1)
                    context.AddFailure("You can't upload video and images");
                else if (files.Exists(f => f.ContentType == "image/gif") && files.Count > 1)
                    context.AddFailure("You can't upload gif and images");
                else if (!files.TrueForAll(f => f.ContentType.StartsWith("image")))
                    context.AddFailure("You can only upload video, images or gif");
            });
    }
}
