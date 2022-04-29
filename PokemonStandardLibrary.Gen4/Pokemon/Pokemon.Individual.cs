using System.Collections.Generic;
using static PokemonStandardLibrary.CommonFunctions;

namespace PokemonStandardLibrary.Gen4
{
    public sealed partial class Pokemon
    {
        public class Individual
        {
            public string Name { get; }
            public string Form { get; }
            public uint Lv { get; }
            public uint PID { get; }
            public Nature Nature { get; }
            public Gender Gender { get; }
            public string Ability { get; }
            public IReadOnlyList<uint> IVs { get; }
            public IReadOnlyList<uint> Stats { get; }
            public ShinyType Shiny { get; private set; }
            public uint HiddenPower { get; }
            public PokeType HiddenPowerType { get; }
            public string HoldItem { get; set; }

            public ShinyType GetShinyType(uint TSV)
                => CommonFunctions.GetShinyType((PID >> 16) ^ (PID & 0xFFFF), TSV, 8);
            public Individual SetShinyType(ShinyType value) { Shiny = value; return this; }
            public Individual SetShinyType(uint TSV) { Shiny = GetShinyType(TSV); return this; }

            internal protected Individual(Species species, uint pid, uint[] ivs, uint lv)
            {
                Name = species.Name;
                Form = species.Form;
                Lv = lv;
                PID = pid;
                IVs = ivs;
                Nature = (Nature)(pid % 25);
                Stats = GetStats(species.BS, ivs, Nature, lv);
                Ability = species.Ability[(int)(pid & 1)];
                Gender = GetGender(pid & 0xFF, species.GenderRatio);
                HiddenPower = CalcHiddenPower(ivs);
                HiddenPowerType = CalcHiddenPowerType(ivs);
            }

            public static Individual Empty = GetPokemon("Dummy").GetIndividual(0, new uint[6], 0);
        }

    }
}
