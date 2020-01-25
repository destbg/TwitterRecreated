using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors) : this(succeeded)
        {
            Errors = errors.ToArray();
        }

        internal Result(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public static Result Success() =>
            new Result(true);

        public static Result Failure(IEnumerable<string> errors) =>
            new Result(false, errors);

        public static Result Failure() =>
            new Result(false);
    }
}
