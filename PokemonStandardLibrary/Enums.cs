using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary.Language;

namespace PokemonStandardLibrary
{
    public enum Gender { Male, Female, Genderless }
    public enum GenderRatio : uint
    {
        MaleOnly = 0,
        M7F1 = 0x1F,
        M3F1 = 0x3F,
        M1F1 = 0x7F,
        M1F3 = 0xBF,
        M1F7 = 0xE1,
        FemaleOnly = 0x100,
        Genderless = 0x12C
    }
    public enum Nature
    {
        Hardy, Lonely, Brave, Adamant, Naughty,
        Bold, Docile, Relaxed, Impish, Lax,
        Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash,
        Calm, Gentle, Sassy, Careful, Quirky, other
    }
    public enum PokeType
    {
        None = 0,
        Normal = (1<<0), 
        Fire = (1<<1), 
        Water = (1<<2), 
        Grass = (1<<3), 
        Electric = (1<<4), 
        Ice = (1<<5), 
        Fighting = (1<<6), 
        Poison = (1<<7),
        Ground = (1<<8), 
        Flying = (1<<9), 
        Psychic = (1<<10),
        Bug = (1<<11), 
        Rock = (1<<12), 
        Ghost = (1<<13), 
        Dragon = (1<<14), 
        Dark = (1<<15), 
        Steel = (1<<16), 
        Fairy = (1<<17)
    }
    public enum ShinyType { NotShiny, Star, Square }
}

namespace PokemonStandardLibrary.CommonExtension
{
    public static class CommonExtension
    {
        static private readonly double[][] Magnifications =
            {
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1.1, 0.9, 1, 1, 1 },
                new double[] { 1, 1.1, 1, 1, 1, 0.9 },
                new double[] { 1, 1.1, 1, 0.9, 1, 1 },
                new double[] { 1, 1.1, 1, 1, 0.9, 1 },
                new double[] { 1, 0.9, 1.1, 1, 1, 1 },
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1, 1.1, 1, 1, 0.9 },
                new double[] { 1, 1, 1.1, 0.9, 1, 1 },
                new double[] { 1, 1, 1.1, 1, 0.9, 1 },
                new double[] { 1, 0.9, 1,1, 1, 1.1 },
                new double[] { 1, 1, 0.9, 1,1, 1.1 },
                new double[] { 1, 1,1, 1, 1, 1 },
                new double[] { 1, 1,1, 0.9, 1, 1.1 },
                new double[] { 1, 1,1, 1, 0.9, 1.1 },
                new double[] { 1, 0.9, 1, 1.1, 1,1 },
                new double[] { 1, 1, 0.9, 1.1, 1, 1 },
                new double[] { 1, 1, 1, 1.1, 1, 0.9 },
                new double[] { 1, 1, 1, 1, 1, 1 },
                new double[] { 1, 1, 1, 1.1, 0.9, 1 },
                new double[] { 1, 0.9, 1,1, 1.1, 1 },
                new double[] { 1, 1, 0.9, 1, 1.1, 1},
                new double[] { 1, 1, 1, 1, 1.1, 0.9 },
                new double[] { 1, 1, 1, 0.9, 1.1, 1 },
                new double[] { 1, 1, 1, 1, 1, 1}
            };
        static private readonly string[] Nature_JP =
        {
            "がんばりや", "さみしがり", "ゆうかん", "いじっぱり",
            "やんちゃ", "ずぶとい", "すなお", "のんき", "わんぱく",
            "のうてんき", "おくびょう", "せっかち", "まじめ", "ようき",
            "むじゃき", "ひかえめ", "おっとり", "れいせい", "てれや",
            "うっかりや", "おだやか", "おとなしい",
            "なまいき", "しんちょう", "きまぐれ", "---"
        };
        static private readonly Dictionary<int, string> PokeType_Kanji = new Dictionary<int, string>()
        {
            { (int) PokeType.None,      "---" },
            { (int) PokeType.Normal,   "普" },
            { (int) PokeType.Fire,     "炎" },
            { (int) PokeType.Water,    "水" },
            { (int) PokeType.Grass,    "草" },
            { (int) PokeType.Electric, "電" },
            { (int) PokeType.Ice,      "氷" },
            { (int) PokeType.Fighting, "闘" },
            { (int) PokeType.Poison,   "毒" },
            { (int) PokeType.Ground,   "地" },
            { (int) PokeType.Flying,   "飛" },
            { (int) PokeType.Psychic,  "超" },
            { (int) PokeType.Bug,      "虫" },
            { (int) PokeType.Rock,     "岩" },
            { (int) PokeType.Ghost,    "霊" },
            { (int) PokeType.Dragon,   "龍" },
            { (int) PokeType.Dark,     "悪" },
            { (int) PokeType.Steel,    "鋼" },
            { (int) PokeType.Fairy,    "妖" }
        };
        static private readonly Dictionary<int, string> PokeType_JP = new Dictionary<int, string>()
        {
            { (int)PokeType.None,      "---" },
            { (int)PokeType.Normal,   "ノーマル" },
            { (int)PokeType.Fire,     "ほのお" },
            { (int)PokeType.Water,    "みず" },
            { (int)PokeType.Grass,    "くさ" },
            { (int)PokeType.Electric, "でんき" },
            { (int)PokeType.Ice,      "こおり" },
            { (int)PokeType.Fighting, "かくとう" },
            { (int)PokeType.Poison,   "どく" },
            { (int)PokeType.Ground,   "じめん" },
            { (int)PokeType.Flying,   "ひこう" },
            { (int)PokeType.Psychic,  "エスパー" },
            { (int)PokeType.Bug,      "むし" },
            { (int)PokeType.Rock,     "いわ" },
            { (int)PokeType.Ghost,    "ゴースト" },
            { (int)PokeType.Dragon,   "ドラゴン" },
            { (int)PokeType.Dark,     "あく" },
            { (int)PokeType.Steel,    "はがね" },
            { (int)PokeType.Fairy,    "フェアリー" }
        };

        static private readonly string[] genderSymbol = { "♂", "♀", "-" };
        static private readonly string[] shinySymbol = { "-", "☆", "◇" };
        static public bool IsFixed(this GenderRatio ratio) => ratio == GenderRatio.FemaleOnly || ratio == GenderRatio.MaleOnly || ratio == GenderRatio.Genderless;
        static public Gender Reverse(this Gender gender)
        {
            if (gender == Gender.Male) return Gender.Female;
            if (gender == Gender.Female) return Gender.Male;
            return Gender.Genderless;
        }
        public static string ToSymbol(this Gender gender) => genderSymbol[(int)gender];

        public static string ToKanji(this PokeType pokeType) { return PokeType_Kanji[(int)pokeType]; }
        public static string ToJapanese(this PokeType pokeType) { return PokeType_JP[(int)pokeType]; }
        public static string ToString(this PokeType pokeType, ITranslator<PokeType> lang)
            => lang.Translate(pokeType);

        public static string ToJapanese(this Nature nature) => Nature_JP[(int)nature];
        public static string ToString(this Nature nature, ITranslator<Nature> lang)
            => lang.Translate(nature);
        public static double[] ToMagnifications(this Nature nature)=> Magnifications[(int)nature].ToArray();
        public static bool IsUncorrected(this Nature nature) => (uint)nature / 5 == (uint)nature % 5;

        public static string ToSymbol(this ShinyType shinyType) => shinySymbol[(int)shinyType];
        public static bool IsShiny(this ShinyType shinyType) => !(shinyType == ShinyType.NotShiny);

        public static Gender ConvertToGender(this string symbol)
        {
            if (symbol == "♂") return Gender.Male;
            if (symbol == "♀") return Gender.Female;
            return Gender.Genderless;
        }
        public static Nature ConvertToNature(this string nature)
        {
            for (int i = 0; i < 25; i++)
                if (Nature_JP[i] == nature) return (Nature)i;

            return Nature.other;
        }
    }
}
