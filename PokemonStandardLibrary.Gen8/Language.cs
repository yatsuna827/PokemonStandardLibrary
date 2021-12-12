using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonStandardLibrary.Gen8.Properties;
using PokemonStandardLibrary.Language;

namespace PokemonStandardLibrary.Language.Gen8
{
    public static class Gen8LanguageSet
    {
        private static (string,string)[] Convert(this string source) => source.Replace("\r\n", "\n")
            .Split(new[] { '\n', '\r' })
            .Select(_ => {
                var pair = _.Split(',');
                return (pair[0], pair[1]);
            }).ToArray();

        private static readonly string[] resources = new string[]
        {
            Resources.Ability_eng,
            Resources.PokeName_eng,
        };

        public static ILanguage JPN = Language.JPN;
        public static ILanguage ENG = Language.ENG.Extends(resources.Select(_ => _.Convert()).SelectMany(_ => _).ToArray());
    }
}
