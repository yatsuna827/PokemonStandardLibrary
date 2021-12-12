using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary.Language
{
    public interface ITranslator
    {
        string Translate(string word);
        string ToJPN(string word);
    }

    public interface ITranslator<T>
    {
        string Translate(T t);
        string ToJPN(T t);
    }
}
