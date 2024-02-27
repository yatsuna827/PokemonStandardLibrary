using System.Collections.Generic;
using static PokemonStandardLibrary.CommonFunctions;

namespace PokemonStandardLibrary.Gen3
{
    public partial class Pokemon
    {
        public class Individual
        {
            public Species Species { get; }

            public string Name { get; }
            public string Form { get; }
            public uint Lv { get; }

            public uint PID { get; }
            public Nature Nature { get; }
            public Gender Gender { get; }
            public string Ability { get; }

            public uint[] IVs { get; } = new uint[6];
            public uint[] EVs { get; } = new uint[6];
            public uint[] Stats { get; } = new uint[6];

            public uint HiddenPower { get; }
            public PokeType HiddenPowerType { get; }

            public ShinyType GetShinyType(uint TSV)
                => CommonFunctions.GetShinyType((PID >> 16) ^ (PID & 0xFFFF), TSV, 8);

            public Individual(Species species, uint pid, uint[] ivs, uint lv, uint[] evs = null)
            {
                Species = species;
                Name = species.Name;
                Lv = lv;
                Form = species.Form;
                PID = pid;
                IVs = ivs;
                EVs = evs ?? new uint[6];
                Nature = (Nature)(pid % 25);
                Stats = GetStats(species.BS, ivs, evs, Nature, lv);
                Ability = species.Ability[(int)(pid & 1)];
                Gender = GetGender(pid & 0xFF, species.GenderRatio);
                HiddenPower = CalcHiddenPower(ivs);
                HiddenPowerType = CalcHiddenPowerType(ivs);
            }

            public Individual(string name, uint pid, uint[] ivs, uint lv, uint[] evs = null)
            {
                var species = GetPokemon(name);

                Species = species;
                Name = species.Name;
                Lv = lv;
                Form = species.Form;
                PID = pid;
                IVs = ivs;
                EVs = evs ?? new uint[6];
                Nature = (Nature)(pid % 25);
                Stats = GetStats(species.BS, ivs, evs, Nature, lv);
                Ability = species.Ability[(int)(pid & 1)];
                Gender = GetGender(pid & 0xFF, species.GenderRatio);
                HiddenPower = CalcHiddenPower(ivs);
                HiddenPowerType = CalcHiddenPowerType(ivs);
            }
        }
    }
}
