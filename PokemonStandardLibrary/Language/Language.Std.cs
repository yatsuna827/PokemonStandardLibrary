using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    class JPNLang : ILanguage
    {
        private static readonly string[] natures = new string[]
        {
            "がんばりや", "さみしがり", "ゆうかん", "いじっぱり", "やんちゃ",
            "ずぶとい", "すなお", "のんき", "わんぱく", "のうてんき",
            "おくびょう", "せっかち", "まじめ", "ようき", "むじゃき",
            "ひかえめ", "おっとり", "れいせい", "てれや", "うっかりや",
            "おだやか", "おとなしい", "なまいき", "しんちょう", "きまぐれ",
            "---"
        };
        private static readonly string[] pokeTypes = new string[]
        {
            "ノーマル", "ほのお", "みず", "くさ", "でんき",
            "こおり", "かくとう", "どく", "じめん", "ひこう",
            "エスパー", "むし", "いわ", "ゴースト", "ドラゴン",
            "あく","はがね","フェアリー", "---"
        };

        public string ToJPN(string word) => word;

        public string ToJPN(Nature nature) => natures[(int)nature];

        public string ToJPN(PokeType t) => pokeTypes[(int)t];

        public string Translate(string word) => word;

        public string Translate(Nature nature) => natures[(int)nature];

        public string Translate(PokeType t) => pokeTypes[(int)t];
        public ILanguage Extends(params (string jpn, string word)[] wordMappings)
        {
            var newWords = new Dictionary<string, string>();
            var newToJPN = new Dictionary<string, string>();
            foreach (var (jpn, word) in wordMappings)
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

    public partial class Language
    {
        public static ILanguage JPN = new JPNLang();
        public static ILanguage ENG = new Language(
            new[]
            {
                "Hardy", "Lonely", "Brave", "Adamant", "Naughty",
                "Bold", "Docile", "Relaxed", "Impish", "Lax",
                "Timid", "Hasty", "Serious", "Jolly", "Naive",
                "Modest", "Mild", "Quiet", "Bashful", "Rash",
                "Calm", "Gentle", "Sassy", "Careful", "Quirky",
                "---"
            }, 
            new[]
            {
                "Normal", "Fire", "Water", "Grass", "Electric", "Ice", "Fighting", "Poison",
                "Ground", "Flying", "Psychic", "Bug", "Rock", "Ghost", "Dragon", "Dark", "Steel", "Fairy", "None"
            });
    }
}
