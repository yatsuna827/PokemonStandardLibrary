using System.Collections.Generic;
using PokemonStandardLibrary.CommonExtension;
using static PokemonStandardLibrary.CommonFunctions;

namespace PokemonStandardLibrary.Gen8
{
    public partial class Pokemon
    {
        public sealed class Individual
        {
            public Species Species { get; }

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

            public byte HeightScale { get; }
            public byte WeightScale { get; }

            public Individual SetShinyType(ShinyType value) { Shiny = value; return this; }

            internal Individual(
                Species species, 
                uint lv, 
                uint ec, 
                uint pid,
                Nature nature,
                Gender gender, 
                uint abilityIndex,
                uint[] ivs,
                byte heightScale,
                byte weightScale
            ) {
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
                HeightScale = heightScale;
                WeightScale = weightScale;

                Species = species;
            }
        }

    }
}
