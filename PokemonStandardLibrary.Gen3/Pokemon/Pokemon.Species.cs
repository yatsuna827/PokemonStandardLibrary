using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary.CommonExtension;

namespace PokemonStandardLibrary.Gen3
{
    public partial class Pokemon
    {
        public class Species
        {
            public int DexID { get; }
            public string Name { get; }
            public string Form { get; }
            public IReadOnlyList<uint> BS { get; }
            public IReadOnlyList<string> Ability { get; }
            public (PokeType Type1, PokeType Type2) Type { get; }
            public GenderRatio GenderRatio { get; }

            public virtual string GetDefaultName() => Name;

            public Individual GetIndividual(uint lv, uint[] ivs, uint pid) => new Individual(this, pid, ivs, lv);
            public Individual GetIndividual(uint lv, uint[] ivs, uint[] evs, uint pid) => new Individual(this, pid, ivs, lv, evs);

            internal Species(int dexID, string name, string formName, uint[] bs, (PokeType type1, PokeType type2) type, string[] ability, GenderRatio ratio)
            {
                DexID = dexID;
                Name = name;
                Form = formName;
                BS = bs;
                Ability = ability;
                Type = type;
                GenderRatio = ratio;
            }
        }
        class AnotherForm : Species
        {
            internal AnotherForm(int dexID, string name, string formName, uint[] bs, (PokeType type1, PokeType type2) type, string[] ability, GenderRatio ratio)
                : base(dexID, name, formName, bs, type, ability, ratio) { }

            public override string GetDefaultName() => $"{Name}#{Form}";
        }
    }

    public static class SpeciesExtensions
    {
        public static (uint[] Min, uint[] Max) CalcIVsRange(this Pokemon.Species species, uint[] stats, uint lv, Nature nature)
        {
            var minIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
            var maxIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };

            var mag = nature.ToMagnifications();
            var bs = species.BS;

            uint stat;
            for (minIVs[0] = 0; minIVs[0] < 32; minIVs[0]++)
            {
                stat = (minIVs[0] + bs[0] * 2) * lv / 100 + 10 + lv;
                if (stat == stats[0]) break;
            }
            if (minIVs[0] != 32)
            {
                for (maxIVs[0] = minIVs[0]; maxIVs[0] < 32; maxIVs[0]++)
                {
                    stat = (maxIVs[0] + 1 + bs[0] * 2) * lv / 100 + 10 + lv;
                    if (stat != stats[0]) break;
                }
                maxIVs[0] = Math.Min(maxIVs[0], 31);
            }

            for (int i = 1; i < 6; i++)
            {
                for (minIVs[i] = 0; minIVs[i] < 32; minIVs[i]++)
                {
                    stat = (uint)(((minIVs[i] + bs[i] * 2) * lv / 100 + 5) * mag[i]);
                    if (stat == stats[i]) break;
                }
                if (minIVs[i] != 32)
                {
                    for (maxIVs[i] = minIVs[i]; maxIVs[i] < 32; maxIVs[i]++)
                    {
                        stat = (uint)(((maxIVs[i] + 1 + bs[i] * 2) * lv / 100 + 5) * mag[i]);
                        if (stat != stats[i]) break;
                    }
                    maxIVs[i] = Math.Min(maxIVs[i], 31);
                }
            }

            return (minIVs, maxIVs);
        }
        public static (uint[] Min, uint[] Max) CalcIVsRange(this Pokemon.Species species, uint[] stats, uint[] evs, uint lv, Nature nature)
        {
            var minIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
            var maxIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
            evs = evs.Select(_ => _ / 4).ToArray();

            double[] mag = nature.ToMagnifications();
            var bs = species.BS;

            uint stat;
            for (minIVs[0] = 0; minIVs[0] < 32; minIVs[0]++)
            {
                stat = (minIVs[0] + evs[0] + bs[0] * 2) * lv / 100 + 10 + lv;
                if (stat == stats[0]) break;
            }
            if (minIVs[0] != 32)
            {
                for (maxIVs[0] = minIVs[0]; maxIVs[0] < 32; maxIVs[0]++)
                {
                    stat = (maxIVs[0] + evs[0] + 1 + evs[0] * 2) * lv / 100 + 10 + lv;
                    if (stat != stats[0]) break;
                }
                maxIVs[0] = Math.Min(maxIVs[0], 31);
            }

            for (int i = 1; i < 6; i++)
            {
                for (minIVs[i] = 0; minIVs[i] < 32; minIVs[i]++)
                {
                    stat = (uint)(((minIVs[i] + evs[i] + evs[i] * 2) * lv / 100 + 5) * mag[i]);
                    if (stat == stats[i]) break;
                }
                if (minIVs[i] != 32)
                {
                    for (maxIVs[i] = minIVs[i]; maxIVs[i] < 32; maxIVs[i]++)
                    {
                        stat = (uint)(((maxIVs[i] + evs[i] + 1 + evs[i] * 2) * lv / 100 + 5) * mag[i]);
                        if (stat != stats[i]) break;
                    }
                    maxIVs[i] = Math.Min(maxIVs[i], 31);
                }
            }

            return (minIVs, maxIVs);
        }

        public static (uint[] minStats, uint[] maxStats) CalcStatsRange(this Pokemon.Species species, uint lv)
        {
            var min = new uint[6];
            var max = new uint[6];

            min[0] = species.Name == "ヌケニン" ? 1 : (00 + species.BS[0] * 2) * lv / 100 + 10 + lv;
            max[0] = species.Name == "ヌケニン" ? 1 : (31 + species.BS[0] * 2) * lv / 100 + 10 + lv;

            for (int i = 1; i < 6; i++)
            {
                min[i] = (uint)(((00 + species.BS[i] * 2) * lv / 100 + 5) * 0.9); // 個体値0 下降補正
                max[i] = (uint)(((31 + species.BS[i] * 2) * lv / 100 + 5) * 1.1); // 個体値31 上昇補正
            }

            return (min, max);
        }
    }
}
