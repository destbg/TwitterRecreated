using System.Collections.Generic;
using System.Linq;
using Common;

namespace Infrastructure.Common
{
    public class ConnectionMapping : IConnectionMapping
    {
        private readonly Dictionary<string, HashSet<string>> _connections;

        public int Count => _connections.Count;

        public ConnectionMapping()
        {
            _connections = new Dictionary<string, HashSet<string>>();
        }

        public void Add(string key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(string key) =>
            _connections.TryGetValue(key, out var connections) ? connections : Enumerable.Empty<string>();

        public void Remove(string key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                    return;

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                        _connections.Remove(key);
                }
            }
        }
    }
}
