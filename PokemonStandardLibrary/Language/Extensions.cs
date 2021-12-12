using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    public static class LanguageExtensions
    {
        public static string ToJPN(this string txt, ITranslator from)
            => from.ToJPN(txt);

        public static string Translate(this string wordJpn, ITranslator to)
            => to.Translate(wordJpn);

        /// <summary>
        /// 日本語を経由して言語を変換します
        /// </summary>
        /// <param name="wordJpn"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string Translate(this string wordJpn, ITranslator from, ITranslator to)
            => to.Translate(from.ToJPN(wordJpn));
    }
}
