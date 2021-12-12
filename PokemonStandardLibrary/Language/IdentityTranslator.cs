using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    public class IdentityTranslator : ITranslator
    {
        public string Translate(string word) => word;
        public string ToJPN(string word) => word;
    }
}
