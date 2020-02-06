using System.Text.RegularExpressions;

namespace Application.Common.Extensions
{
    public static class PostExtensions
    {
        public static (int start, int end) IndexOfNonRepeat(this string value, char c, Regex exp)
        {
            for (var start = 0; start < value.Length; start++)
            {
                if (value[start] == c)
                {
                    var end = start + 1;
                    while (value.Length != end && exp.IsMatch(value[end].ToString()))
                        end++;
                    if (start + 1 != end)
                        return (start, end);
                }
            }
            return (-1, -1);
        }
    }
}
