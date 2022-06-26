using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PokemonStandardLibrary.Gen8
{
    internal class DataStore<T>
        where T : class
    {
        private readonly Dictionary<string, T> _store;

        public T GetData(string name)
            => _store.ContainsKey(name) ? _store[name] : null;

        public DataStore(string raw)
        {
            _store = JsonSerializer.Deserialize<Dictionary<string, T>>(raw, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
