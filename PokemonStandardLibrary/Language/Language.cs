using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    public interface ILanguage : ITranslator, ITranslator<Nature>, ITranslator<PokeType>
    {
        ILanguage Extends(params (string jpn, string word)[] wordMappings);
    }

    public partial class Language : ILanguage
    {
        protected readonly Dictionary<string, string> words;
        protected readonly Dictionary<string, string> toJPN;
        protected readonly string[] natures;
        protected readonly string[] pokeTypes;

        public string Translate(string word) => words[word];
        public string Translate(Nature nature) => natures[(int)nature];
        public string Translate(PokeType pokeType) => pokeTypes[(int)pokeType];

        public string ToJPN(string word) => toJPN[word];
        public string ToJPN(Nature nature) => natures[(int)nature];
        public string ToJPN(PokeType pokeType) => pokeTypes[(int)pokeType];

        internal Language(string[] natures, string[] pokeTypes, Dictionary<string, string> words, Dictionary<string, string> toJpn)
        {
            this.words = words;
            this.toJPN = toJpn;
            this.natures = natures;
            this.pokeTypes = pokeTypes;
        }
        public Language(string[] natures, string[] pokeTypes, params (string jpn, string word)[] wordMappings)
        {
            words = new Dictionary<string, string>();
            toJPN = new Dictionary<string, string>();

            this.natures = natures;
            this.pokeTypes = pokeTypes;

            foreach(var (jpn, word) in wordMappings)
            {
                words.Add(jpn, word);
                toJPN.Add(word, jpn);
            }
        }
        public ILanguage Extends(params (string jpn, string word)[] wordMappings)
        {
            var newWords = new Dictionary<string, string>();
            foreach (var pair in words) newWords.Add(pair.Key, pair.Value);

            var newToJPN = new Dictionary<string, string>();
            foreach (var pair in toJPN) newToJPN.Add(pair.Key, pair.Value);

            foreach(var (jpn, word) in wordMappings)
            {
                if (newWords.ContainsKey(jpn))
                    newWords[jpn] = word;
                else 
                    newWords.Add(jpn, word);

                if (newWords.ContainsKey(jpn))
                    newToJPN[word] = jpn;
                else
                    newToJPN.Add(word, jpn);

            }

            return new Language(natures, pokeTypes, newWords, newToJPN);
        }
    }
}
