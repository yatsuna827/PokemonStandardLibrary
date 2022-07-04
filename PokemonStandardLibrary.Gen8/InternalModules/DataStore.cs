using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            _store = JsonConvert.DeserializeObject<Dictionary<string, T>>(raw);
        }
    }
}
