using System.Collections.Generic;

namespace Common
{
    public interface IConnectionMapping
    {
        public int Count { get; }
        public void Add(string key, string connectionId);
        public IEnumerable<string> GetConnections(string key);
        public void Remove(string key, string connectionId);
        public IEnumerable<string> UsersOnline(IEnumerable<string> usernames);
    }
}
