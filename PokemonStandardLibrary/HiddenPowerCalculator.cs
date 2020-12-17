using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonStandardLibrary
{
    static class HiddenPowerCalculator
    {

        internal static uint CalcHiddenPower(uint[] ivs)
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
        internal static PokeType CalcHiddenPowerType(uint[] ivs)
        {
            uint num = (ivs[0] & 1) + 2 * (ivs[1] & 1) + 4 * (ivs[2] & 1) + 8 * (ivs[5] & 1) + 16 * (ivs[3] & 1) + 32 * (ivs[4] & 1);
            return hiddenPowerType[(num * 15 / 63)];
        }
    }
}
