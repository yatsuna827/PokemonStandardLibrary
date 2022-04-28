using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    public class Translator : ITranslator
    {
        protected readonly Dictionary<string, string> words;
        protected readonly Dictionary<string, string> toJPN;
        public Translator((string jpn, string word)[] wordMappings)
        {
            words = new Dictionary<string, string>();
            toJPN = new Dictionary<string, string>();

            foreach (var (jpn, word) in wordMappings)
            {
                words.Add(jpn, word);
                toJPN.Add(word, jpn);
            }
        }
        public string Translate(string jpn)
        {
            if (!words.TryGetValue(jpn, out var res)) return jpn;

            return res;
        }
        public string ToJPN(string word)
        {
            if (!toJPN.TryGetValue(word, out var res)) return word;

            return res;
        }
    }
    public class IdentityTranslator : ITranslator
    {
        public string Translate(string word) => word;
        public string ToJPN(string word) => word;
    }
}
