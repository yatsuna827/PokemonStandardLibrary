using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary.CommonExtension;
using System.Text.Json.Serialization;

namespace PokemonStandardLibrary.Gen8
{
    public partial class Pokemon
    {
        public class Species
        {
            public int DexID { get; }
            public int GalarDexID { get; }
            public int ArmorDexID { get; }
            public int CrownDexID { get; }

            public string Name { get; }
            public IReadOnlyList<uint> BS { get; }
            public IReadOnlyList<string> Ability { get; }
            public (PokeType Type1, PokeType Type2) Type { get; }
            public GenderRatio GenderRatio { get; }
            public string Form { get; }

            public virtual string GetDefaultName() => Name;

            public Individual GetIndividual(uint lv, uint[] ivs, uint ec, uint pid, Nature nature, uint abilityIndex, Gender gender)
                => new Individual(this, lv, ec, pid, nature, gender, abilityIndex, ivs);

            internal Species(int dexID, int galarDexID, int armorDexID, int crownDexID, string name, string formName, uint[] bs, (PokeType type1, PokeType type2) type, string[] ability, GenderRatio ratio)
            {
                DexID = dexID;
                GalarDexID = galarDexID;
                ArmorDexID = armorDexID;
                CrownDexID = crownDexID;
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
            internal AnotherForm(int dexID, int galarDexID, int armorDexID, int crownDexID, string name, string formName, uint[] bs, (PokeType type1, PokeType type2) type, string[] ability, GenderRatio ratio)
                : base(dexID, galarDexID, armorDexID, crownDexID, name, formName, bs, type, ability, ratio) { }
            public override string GetDefaultName()
                => $"{Name}#{Form}";
        }
    }

    public class HoldingItem
    {
        [JsonInclude]
        public string Guaranteed { get; set; }
        [JsonInclude]
        public string Common { get; set; }
        [JsonInclude]
        public string Rare { get; set; }
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
        
        private static DataStore<HoldingItem> _holdingItemsStore;
        /// <summary>
        /// 戻り値はnullableなので注意してね！
        /// 持ち物を一切持っていない場合はnullが返ってきますよ！
        /// </summary>
        /// <param name="species"></param>
        /// <returns></returns>
        public static HoldingItem HoldingItems(this Pokemon.Species species)
        {
            if (_holdingItemsStore is null) _holdingItemsStore = new DataStore<HoldingItem>(Properties.Resources.holdingItem);

            return _holdingItemsStore.GetData(species.GetDefaultName());
        }

        private static DataStore<string[]> _eggMoves;
        /// <summary>
        /// 戻り値はNon-Nullable
        /// </summary>
        /// <param name="species"></param>
        /// <returns></returns>
        public static string[] EggMoves(this Pokemon.Species species)
        {
            if (_eggMoves is null) _eggMoves = new DataStore<string[]>(Properties.Resources.eggMoves);

            return _eggMoves.GetData(species.GetDefaultName()) ?? new string[0];
        }
    }
}
