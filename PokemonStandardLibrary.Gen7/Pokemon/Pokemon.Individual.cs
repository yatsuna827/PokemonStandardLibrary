using System.Collections.Generic;
using static PokemonStandardLibrary.CommonFunctions;

namespace PokemonStandardLibrary.Gen7
{
    public sealed partial class Pokemon
    {
        public class Individual
        {
            public string Name { get; }
            public string Form { get; }
            public uint Lv { get; }
            public uint EC { get; }
            public uint PID { get; }
            public Nature Nature { get; }
            public Gender Gender { get; }
            public string Ability { get; }
            public IReadOnlyList<uint> IVs { get; }
            public IReadOnlyList<uint> Stats { get; }
            public ShinyType Shiny { get; private set; }
            public Individual SetShinyType(ShinyType value) { Shiny = value; return this; }

            internal Individual(Species species, uint lv, uint ec, uint pid, Nature nature, Gender gender, uint abilityIndex, uint[] ivs)
            {
                Name = species.Name;
                Form = species.Form;
                Lv = lv;
                EC = ec;
                PID = pid;
                Nature = nature;
                Gender = gender;
                Ability = species.Ability[(int)abilityIndex];
                IVs = ivs;
                Stats = GetStats(species.BS, ivs, nature, lv);
            }

            public static Individual Empty = GetPokemon("Dummy").GetIndividual(0, new uint[6], 0, 0, Nature.Hardy, 0, Gender.Genderless);
        }

    }
}
