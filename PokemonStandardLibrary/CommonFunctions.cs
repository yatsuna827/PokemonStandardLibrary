using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary.CommonExtension;

namespace PokemonStandardLibrary
{
    public static class CommonFunctions
    {
        public static uint[] GetStats(IReadOnlyList<uint> bs, IList<uint> ivs, Nature nature, uint lv)
        {
            var mag = nature.ToMagnifications();

            return new uint[6] {
                CalcStat(bs[0], ivs[0], 0, lv),
                CalcStat(bs[1], ivs[1], 0, lv, mag[1]),
                CalcStat(bs[2], ivs[2], 0, lv, mag[2]),
                CalcStat(bs[3], ivs[3], 0, lv, mag[3]),
                CalcStat(bs[4], ivs[4], 0, lv, mag[4]),
                CalcStat(bs[5], ivs[5], 0, lv, mag[5]),
            };
        }
        public static uint[] GetStats(IReadOnlyList<uint> bs, IList<uint> ivs, IList<uint> evs, Nature nature, uint lv)
        {
            var mag = nature.ToMagnifications();
            evs = evs ?? new uint[6];

            return new uint[6] {
                CalcStat(bs[0], ivs[0], evs[0] / 4, lv),
                CalcStat(bs[1], ivs[1], evs[1] / 4, lv, mag[1]),
                CalcStat(bs[2], ivs[2], evs[2] / 4, lv, mag[2]),
                CalcStat(bs[3], ivs[3], evs[3] / 4, lv, mag[3]),
                CalcStat(bs[4], ivs[4], evs[4] / 4, lv, mag[4]),
                CalcStat(bs[5], ivs[5], evs[5] / 4, lv, mag[5]),
            };
        }

        public static uint CalcStat(uint bs, uint iv, uint ev, uint lv)
            => bs > 1 ? (iv + ev + bs * 2) * lv / 100 + 10 + lv : 1;
        public static uint CalcStat(uint bs, uint iv, uint ev, uint lv, double magnification)
            => (uint)(((iv + ev + bs * 2) * lv / 100 + 5) * magnification);

        public static uint CalcHiddenPower(IReadOnlyList<uint> ivs)
        {
            uint num = ((ivs[0] >> 1) & 1) + 2 * ((ivs[1] >> 1) & 1) + 4 * ((ivs[2] >> 1) & 1) + 8 * ((ivs[5] >> 1) & 1) + 16 * ((ivs[3] >> 1) & 1) + 32 * ((ivs[4] >> 1) & 1);

            return num * 40 / 63 + 30;
        }

        private static readonly PokeType[] hiddenPowerType = new PokeType[16]
        {
            PokeType.Fighting,
            PokeType.Flying,
            PokeType.Poison,
            PokeType.Ground,
            PokeType.Rock,
            PokeType.Bug,
            PokeType.Ghost,
            PokeType.Steel,
            PokeType.Fire,
            PokeType.Water,
            PokeType.Grass,
            PokeType.Electric,
            PokeType.Psychic,
            PokeType.Ice,
            PokeType.Dragon,
            PokeType.Dark
        };
        public static PokeType CalcHiddenPowerType(IReadOnlyList<uint> ivs)
        {
            uint num = (ivs[0] & 1) + 2 * (ivs[1] & 1) + 4 * (ivs[2] & 1) + 8 * (ivs[5] & 1) + 16 * (ivs[3] & 1) + 32 * (ivs[4] & 1);
            return hiddenPowerType[(num * 15 / 63)];
        }

        public static Gender GetGender(uint genderValue, GenderRatio ratio)
            => ratio == GenderRatio.Genderless ? Gender.Genderless : (genderValue < (uint)ratio ? Gender.Female : Gender.Male);

        public static ShinyType GetShinyType(uint psv, uint tsv, uint thresh)
            => (psv ^ tsv) < thresh ? (psv == tsv ? ShinyType.Square : ShinyType.Star) : ShinyType.NotShiny;
    }
}
