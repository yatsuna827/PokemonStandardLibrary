using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonStandardLibrary.Gen3
{
    public sealed partial class Pokemon
    {
        private static readonly IReadOnlyList<Species> uniqueList;
        private static readonly List<Species> dexData;
        private static readonly ILookup<string, Species> formDex;
        private static readonly Dictionary<string, Species> uniqueDex;
        private static readonly Dictionary<string, Species> dexDictionary;

        private Pokemon() { }
        public static Species GetPokemon(string name)
        {
            if (name.Contains('#'))
            {
                var sp = name.Split('#');
                return GetPokemon(sp[0], sp[1]);
            }

            if (!uniqueDex.TryGetValue(name, out var pokemon))
                throw new Exception($"{name}は登録されていません");

            return pokemon;
        }
        public static Species GetPokemon(string name, string form)
        {
            var key = $"{name}#{form}";

            if (!dexDictionary.TryGetValue(key, out var pokemon))
                throw new Exception($"{name}#{form}は登録されていません");

            return pokemon;
        }

        public static bool TryGetPokemon(string name, out Species pokemon)
        {
            if (name.Contains('#'))
            {
                var sp = name.Split('#');
                return TryGetPokemon(sp[0], sp[1], out pokemon);
            }

            return uniqueDex.TryGetValue(name, out pokemon);
        }
        public static bool TryGetPokemon(string name, string form, out Species pokemon)
            => dexDictionary.TryGetValue($"{name}#{form}", out pokemon);

        public static IReadOnlyList<Species> GetAllForms(string name) 
            => formDex[name].ToArray();
        public static IReadOnlyList<Species> GetUniquePokemonList() 
            => uniqueList.Where(_ => _.Name != "Dummy").ToArray();
        public static IReadOnlyList<Species> GetAllPokemonList() 
            => dexData.Where(_ => _.Name != "Dummy").ToArray();

        static Pokemon()
        {
            dexData = new List<Species>();

            dexData.Add(new Species(-1, "Dummy", "Genderless", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.Genderless));
            dexData.Add(new Species(-1, "Dummy", "MaleOnly", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(-1, "Dummy", "M7F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M7F1));
            dexData.Add(new Species(-1, "Dummy", "M3F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M3F1));
            dexData.Add(new Species(-1, "Dummy", "M1F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M1F1));
            dexData.Add(new Species(-1, "Dummy", "M1F3", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M1F3));
            dexData.Add(new Species(-1, "Dummy", "FemaleOnly", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.None), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.FemaleOnly));

            dexData.Add(new Species(1, "フシギダネ", "", new uint[] { 45, 49, 49, 65, 65, 45 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(2, "フシギソウ", "", new uint[] { 60, 62, 63, 80, 80, 60 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(3, "フシギバナ", "", new uint[] { 80, 82, 83, 100, 100, 80 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(4, "ヒトカゲ", "", new uint[] { 39, 52, 43, 60, 50, 65 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(5, "リザード", "", new uint[] { 58, 64, 58, 80, 65, 80 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(6, "リザードン", "", new uint[] { 78, 84, 78, 109, 85, 100 }, (PokeType.Fire, PokeType.Flying), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(7, "ゼニガメ", "", new uint[] { 44, 48, 65, 50, 64, 43 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(8, "カメール", "", new uint[] { 59, 63, 80, 65, 80, 58 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(9, "カメックス", "", new uint[] { 79, 83, 100, 85, 105, 78 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(10, "キャタピー", "", new uint[] { 45, 30, 35, 20, 20, 45 }, (PokeType.Bug, PokeType.None), new string[] { "りんぷん", "りんぷん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(11, "トランセル", "", new uint[] { 50, 20, 55, 25, 25, 30 }, (PokeType.Bug, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(12, "バタフリー", "", new uint[] { 60, 45, 50, 80, 80, 70 }, (PokeType.Bug, PokeType.Flying), new string[] { "ふくがん", "ふくがん" }, GenderRatio.M1F1));
            dexData.Add(new Species(13, "ビードル", "", new uint[] { 40, 35, 30, 20, 20, 50 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "りんぷん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(14, "コクーン", "", new uint[] { 45, 25, 50, 25, 25, 35 }, (PokeType.Bug, PokeType.Poison), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(15, "スピアー", "", new uint[] { 65, 80, 40, 45, 80, 75 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(16, "ポッポ", "", new uint[] { 40, 45, 40, 35, 35, 56 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(17, "ピジョン", "", new uint[] { 63, 60, 55, 50, 50, 71 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(18, "ピジョット", "", new uint[] { 83, 80, 75, 70, 70, 91 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(19, "コラッタ", "", new uint[] { 30, 56, 35, 25, 35, 72 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "こんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(20, "ラッタ", "", new uint[] { 55, 81, 60, 50, 70, 97 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(21, "オニスズメ", "", new uint[] { 40, 60, 30, 31, 31, 70 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(22, "オニドリル", "", new uint[] { 65, 90, 65, 61, 61, 100 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(23, "アーボ", "", new uint[] { 35, 60, 44, 40, 54, 55 }, (PokeType.Poison, PokeType.None), new string[] { "いかく", "だっぴ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(24, "アーボック", "", new uint[] { 60, 85, 69, 65, 79, 80 }, (PokeType.Poison, PokeType.None), new string[] { "いかく", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(25, "ピカチュウ", "", new uint[] { 35, 55, 30, 50, 40, 90 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(26, "ライチュウ", "", new uint[] { 60, 90, 55, 90, 80, 100 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(27, "サンド", "", new uint[] { 50, 75, 85, 20, 30, 40 }, (PokeType.Ground, PokeType.None), new string[] { "すながくれ", "すながくれ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(28, "サンドパン", "", new uint[] { 75, 100, 110, 45, 55, 65 }, (PokeType.Ground, PokeType.None), new string[] { "すながくれ", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(29, "ニドラン♀", "", new uint[] { 55, 47, 52, 40, 40, 41 }, (PokeType.Poison, PokeType.None), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(30, "ニドリーナ", "", new uint[] { 70, 62, 67, 55, 55, 56 }, (PokeType.Poison, PokeType.None), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(31, "ニドクイン", "", new uint[] { 90, 82, 87, 75, 85, 76 }, (PokeType.Poison, PokeType.Ground), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(32, "ニドラン♂", "", new uint[] { 46, 57, 40, 40, 40, 50 }, (PokeType.Poison, PokeType.None), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.MaleOnly, true));
            dexData.Add(new Species(33, "ニドリーノ", "", new uint[] { 61, 72, 57, 55, 55, 65 }, (PokeType.Poison, PokeType.None), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(34, "ニドキング", "", new uint[] { 81, 92, 77, 85, 75, 85 }, (PokeType.Poison, PokeType.Ground), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(35, "ピッピ", "", new uint[] { 70, 45, 48, 60, 65, 35 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(36, "ピクシー", "", new uint[] { 95, 70, 73, 85, 90, 60 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(37, "ロコン", "", new uint[] { 38, 41, 40, 50, 65, 65 }, (PokeType.Fire, PokeType.None), new string[] { "もらいび", "もらいび" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(38, "キュウコン", "", new uint[] { 73, 76, 75, 81, 100, 100 }, (PokeType.Fire, PokeType.None), new string[] { "もらいび", "もらいび" }, GenderRatio.M1F3));
            dexData.Add(new Species(39, "プリン", "", new uint[] { 115, 45, 20, 45, 25, 20 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(40, "プクリン", "", new uint[] { 140, 70, 45, 75, 50, 45 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(41, "ズバット", "", new uint[] { 40, 45, 35, 30, 40, 55 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(42, "ゴルバット", "", new uint[] { 75, 80, 70, 65, 75, 90 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(43, "ナゾノクサ", "", new uint[] { 45, 50, 55, 75, 65, 30 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(44, "クサイハナ", "", new uint[] { 60, 65, 70, 85, 75, 40 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(45, "ラフレシア", "", new uint[] { 75, 80, 85, 100, 90, 50 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(46, "パラス", "", new uint[] { 35, 70, 55, 45, 55, 25 }, (PokeType.Bug, PokeType.Grass), new string[] { "ほうし", "ほうし" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(47, "パラセクト", "", new uint[] { 60, 95, 80, 60, 80, 30 }, (PokeType.Bug, PokeType.Grass), new string[] { "ほうし", "ほうし" }, GenderRatio.M1F1));
            dexData.Add(new Species(48, "コンパン", "", new uint[] { 60, 55, 50, 40, 55, 45 }, (PokeType.Bug, PokeType.Poison), new string[] { "ふくがん", "ふくがん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(49, "モルフォン", "", new uint[] { 70, 65, 60, 90, 75, 90 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "りんぷん" }, GenderRatio.M1F1));
            dexData.Add(new Species(50, "ディグダ", "", new uint[] { 10, 55, 25, 35, 45, 95 }, (PokeType.Ground, PokeType.None), new string[] { "すながくれ", "ありじごく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(51, "ダグトリオ", "", new uint[] { 35, 80, 50, 50, 70, 120 }, (PokeType.Ground, PokeType.None), new string[] { "すながくれ", "ありじごく" }, GenderRatio.M1F1));
            dexData.Add(new Species(52, "ニャース", "", new uint[] { 40, 45, 35, 40, 40, 90 }, (PokeType.Normal, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(53, "ペルシアン", "", new uint[] { 65, 70, 60, 65, 65, 115 }, (PokeType.Normal, PokeType.None), new string[] { "じゅうなん", "じゅうなん" }, GenderRatio.M1F1));
            dexData.Add(new Species(54, "コダック", "", new uint[] { 50, 52, 48, 65, 50, 55 }, (PokeType.Water, PokeType.None), new string[] { "しめりけ", "ノーてんき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(55, "ゴルダック", "", new uint[] { 80, 82, 78, 95, 80, 85 }, (PokeType.Water, PokeType.None), new string[] { "しめりけ", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(56, "マンキー", "", new uint[] { 40, 80, 35, 35, 45, 70 }, (PokeType.Fighting, PokeType.None), new string[] { "やるき", "やるき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(57, "オコリザル", "", new uint[] { 65, 105, 60, 60, 70, 95 }, (PokeType.Fighting, PokeType.None), new string[] { "やるき", "やるき" }, GenderRatio.M1F1));
            dexData.Add(new Species(58, "ガーディ", "", new uint[] { 55, 70, 45, 70, 50, 60 }, (PokeType.Fire, PokeType.None), new string[] { "いかく", "もらいび" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(59, "ウインディ", "", new uint[] { 90, 110, 80, 100, 80, 95 }, (PokeType.Fire, PokeType.None), new string[] { "いかく", "もらいび" }, GenderRatio.M3F1));
            dexData.Add(new Species(60, "ニョロモ", "", new uint[] { 40, 50, 40, 40, 40, 90 }, (PokeType.Water, PokeType.None), new string[] { "ちょすい", "しめりけ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(61, "ニョロゾ", "", new uint[] { 65, 65, 65, 50, 50, 90 }, (PokeType.Water, PokeType.None), new string[] { "ちょすい", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(62, "ニョロボン", "", new uint[] { 90, 95, 95, 70, 90, 70 }, (PokeType.Water, PokeType.Fighting), new string[] { "ちょすい", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(63, "ケーシィ", "", new uint[] { 25, 20, 15, 105, 55, 90 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "せいしんりょく" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(64, "ユンゲラー", "", new uint[] { 40, 35, 30, 120, 70, 105 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "せいしんりょく" }, GenderRatio.M3F1));
            dexData.Add(new Species(65, "フーディン", "", new uint[] { 55, 50, 45, 135, 85, 120 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "せいしんりょく" }, GenderRatio.M3F1));
            dexData.Add(new Species(66, "ワンリキー", "", new uint[] { 70, 80, 50, 35, 35, 35 }, (PokeType.Fighting, PokeType.None), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(67, "ゴーリキー", "", new uint[] { 80, 100, 70, 50, 60, 45 }, (PokeType.Fighting, PokeType.None), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M3F1));
            dexData.Add(new Species(68, "カイリキー", "", new uint[] { 90, 130, 80, 65, 85, 55 }, (PokeType.Fighting, PokeType.None), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M3F1));
            dexData.Add(new Species(69, "マダツボミ", "", new uint[] { 50, 75, 35, 70, 30, 40 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(70, "ウツドン", "", new uint[] { 65, 90, 50, 85, 45, 55 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(71, "ウツボット", "", new uint[] { 80, 105, 65, 100, 60, 70 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(72, "メノクラゲ", "", new uint[] { 40, 40, 35, 50, 100, 70 }, (PokeType.Water, PokeType.Poison), new string[] { "クリアボディ", "ヘドロえき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(73, "ドククラゲ", "", new uint[] { 80, 70, 65, 80, 120, 100 }, (PokeType.Water, PokeType.Poison), new string[] { "クリアボディ", "ヘドロえき" }, GenderRatio.M1F1));
            dexData.Add(new Species(74, "イシツブテ", "", new uint[] { 40, 80, 100, 30, 30, 20 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(75, "ゴローン", "", new uint[] { 55, 95, 115, 45, 45, 35 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(76, "ゴローニャ", "", new uint[] { 80, 110, 130, 55, 65, 45 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(77, "ポニータ", "", new uint[] { 50, 85, 55, 65, 65, 90 }, (PokeType.Fire, PokeType.None), new string[] { "にげあし", "もらいび" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(78, "ギャロップ", "", new uint[] { 65, 100, 70, 80, 80, 105 }, (PokeType.Fire, PokeType.None), new string[] { "にげあし", "もらいび" }, GenderRatio.M1F1));
            dexData.Add(new Species(79, "ヤドン", "", new uint[] { 90, 65, 65, 40, 40, 15 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(80, "ヤドラン", "", new uint[] { 95, 75, 110, 100, 80, 30 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(81, "コイル", "", new uint[] { 25, 35, 70, 95, 55, 45 }, (PokeType.Electric, PokeType.Steel), new string[] { "じりょく", "がんじょう" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(82, "レアコイル", "", new uint[] { 50, 60, 95, 120, 70, 70 }, (PokeType.Electric, PokeType.Steel), new string[] { "じりょく", "がんじょう" }, GenderRatio.Genderless));
            dexData.Add(new Species(83, "カモネギ", "", new uint[] { 52, 65, 55, 58, 62, 60 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "せいしんりょく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(84, "ドードー", "", new uint[] { 35, 85, 45, 35, 35, 75 }, (PokeType.Normal, PokeType.Flying), new string[] { "にげあし", "はやおき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(85, "ドードリオ", "", new uint[] { 60, 110, 70, 60, 60, 100 }, (PokeType.Normal, PokeType.Flying), new string[] { "にげあし", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(86, "パウワウ", "", new uint[] { 65, 45, 55, 45, 70, 45 }, (PokeType.Water, PokeType.None), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(87, "ジュゴン", "", new uint[] { 90, 70, 80, 70, 95, 70 }, (PokeType.Water, PokeType.Ice), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(88, "ベトベター", "", new uint[] { 80, 80, 50, 40, 50, 25 }, (PokeType.Poison, PokeType.None), new string[] { "あくしゅう", "ねんちゃく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(89, "ベトベトン", "", new uint[] { 105, 105, 75, 65, 100, 50 }, (PokeType.Poison, PokeType.None), new string[] { "あくしゅう", "ねんちゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(90, "シェルダー", "", new uint[] { 30, 65, 100, 45, 25, 40 }, (PokeType.Water, PokeType.None), new string[] { "シェルアーマー", "シェルアーマー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(91, "パルシェン", "", new uint[] { 50, 95, 180, 85, 45, 70 }, (PokeType.Water, PokeType.Ice), new string[] { "シェルアーマー", "シェルアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(92, "ゴース", "", new uint[] { 30, 35, 30, 100, 35, 80 }, (PokeType.Ghost, PokeType.Poison), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(93, "ゴースト", "", new uint[] { 45, 50, 45, 115, 55, 95 }, (PokeType.Ghost, PokeType.Poison), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(94, "ゲンガー", "", new uint[] { 60, 65, 60, 130, 75, 110 }, (PokeType.Ghost, PokeType.Poison), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(95, "イワーク", "", new uint[] { 35, 45, 160, 30, 45, 70 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(96, "スリープ", "", new uint[] { 60, 48, 45, 43, 90, 42 }, (PokeType.Psychic, PokeType.None), new string[] { "ふみん", "ふみん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(97, "スリーパー", "", new uint[] { 85, 73, 70, 73, 115, 67 }, (PokeType.Psychic, PokeType.None), new string[] { "ふみん", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new Species(98, "クラブ", "", new uint[] { 30, 105, 90, 25, 25, 50 }, (PokeType.Water, PokeType.None), new string[] { "かいりきバサミ", "シェルアーマー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(99, "キングラー", "", new uint[] { 55, 130, 115, 50, 50, 75 }, (PokeType.Water, PokeType.None), new string[] { "かいりきバサミ", "シェルアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(100, "ビリリダマ", "", new uint[] { 40, 30, 50, 55, 55, 100 }, (PokeType.Electric, PokeType.None), new string[] { "ぼうおん", "せいでんき" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(101, "マルマイン", "", new uint[] { 60, 50, 70, 80, 80, 140 }, (PokeType.Electric, PokeType.None), new string[] { "ぼうおん", "せいでんき" }, GenderRatio.Genderless));
            dexData.Add(new Species(102, "タマタマ", "", new uint[] { 60, 40, 80, 60, 45, 40 }, (PokeType.Grass, PokeType.Psychic), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(103, "ナッシー", "", new uint[] { 95, 95, 85, 125, 65, 55 }, (PokeType.Grass, PokeType.Psychic), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(104, "カラカラ", "", new uint[] { 50, 50, 95, 40, 50, 35 }, (PokeType.Ground, PokeType.None), new string[] { "いしあたま", "ひらいしん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(105, "ガラガラ", "", new uint[] { 60, 80, 110, 50, 80, 45 }, (PokeType.Ground, PokeType.None), new string[] { "いしあたま", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(106, "サワムラー", "", new uint[] { 50, 120, 53, 35, 110, 87 }, (PokeType.Fighting, PokeType.None), new string[] { "じゅうなん", "じゅうなん" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(107, "エビワラー", "", new uint[] { 50, 105, 79, 35, 110, 76 }, (PokeType.Fighting, PokeType.None), new string[] { "するどいめ", "するどいめ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(108, "ベロリンガ", "", new uint[] { 90, 55, 75, 60, 75, 30 }, (PokeType.Normal, PokeType.None), new string[] { "マイペース", "どんかん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(109, "ドガース", "", new uint[] { 40, 65, 95, 60, 45, 35 }, (PokeType.Poison, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(110, "マタドガス", "", new uint[] { 65, 90, 120, 85, 70, 60 }, (PokeType.Poison, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(111, "サイホーン", "", new uint[] { 80, 85, 95, 30, 30, 25 }, (PokeType.Ground, PokeType.Rock), new string[] { "ひらいしん", "いしあたま" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(112, "サイドン", "", new uint[] { 105, 130, 120, 45, 45, 40 }, (PokeType.Ground, PokeType.Rock), new string[] { "ひらいしん", "いしあたま" }, GenderRatio.M1F1));
            dexData.Add(new Species(113, "ラッキー", "", new uint[] { 250, 5, 5, 35, 105, 50 }, (PokeType.Normal, PokeType.None), new string[] { "しぜんかいふく", "てんのめぐみ" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(114, "モンジャラ", "", new uint[] { 65, 55, 115, 100, 40, 60 }, (PokeType.Grass, PokeType.None), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(115, "ガルーラ", "", new uint[] { 105, 95, 80, 40, 80, 90 }, (PokeType.Normal, PokeType.None), new string[] { "はやおき", "はやおき" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(116, "タッツー", "", new uint[] { 30, 40, 70, 70, 25, 60 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(117, "シードラ", "", new uint[] { 55, 65, 95, 95, 45, 85 }, (PokeType.Water, PokeType.None), new string[] { "どくのトゲ", "どくのトゲ" }, GenderRatio.M1F1));
            dexData.Add(new Species(118, "トサキント", "", new uint[] { 45, 67, 60, 35, 50, 63 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "みずのベール" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(119, "アズマオウ", "", new uint[] { 80, 92, 65, 65, 80, 68 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(120, "ヒトデマン", "", new uint[] { 30, 45, 55, 70, 55, 85 }, (PokeType.Water, PokeType.None), new string[] { "はっこう", "しぜんかいふく" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(121, "スターミー", "", new uint[] { 60, 75, 85, 100, 85, 115 }, (PokeType.Water, PokeType.Psychic), new string[] { "はっこう", "しぜんかいふく" }, GenderRatio.Genderless));
            dexData.Add(new Species(122, "バリヤード", "", new uint[] { 40, 45, 65, 100, 120, 90 }, (PokeType.Psychic, PokeType.None), new string[] { "ぼうおん", "ぼうおん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(123, "ストライク", "", new uint[] { 70, 110, 80, 55, 80, 105 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "むしのしらせ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(124, "ルージュラ", "", new uint[] { 65, 50, 35, 115, 95, 95 }, (PokeType.Ice, PokeType.Psychic), new string[] { "どんかん", "どんかん" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(125, "エレブー", "", new uint[] { 65, 83, 57, 95, 85, 105 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M3F1));
            dexData.Add(new Species(126, "ブーバー", "", new uint[] { 65, 95, 57, 100, 85, 93 }, (PokeType.Fire, PokeType.None), new string[] { "ほのおのからだ", "ほのおのからだ" }, GenderRatio.M3F1));
            dexData.Add(new Species(127, "カイロス", "", new uint[] { 65, 125, 100, 55, 70, 85 }, (PokeType.Bug, PokeType.None), new string[] { "かいりきバサミ", "かいりきバサミ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(128, "ケンタロス", "", new uint[] { 75, 100, 95, 40, 70, 110 }, (PokeType.Normal, PokeType.None), new string[] { "いかく", "いかく" }, GenderRatio.MaleOnly, true));
            dexData.Add(new Species(129, "コイキング", "", new uint[] { 20, 10, 55, 15, 20, 80 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(130, "ギャラドス", "", new uint[] { 95, 125, 79, 60, 100, 81 }, (PokeType.Water, PokeType.Flying), new string[] { "いかく", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(131, "ラプラス", "", new uint[] { 130, 85, 80, 85, 95, 60 }, (PokeType.Water, PokeType.Ice), new string[] { "ちょすい", "シェルアーマー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(132, "メタモン", "", new uint[] { 48, 48, 48, 48, 48, 48 }, (PokeType.Normal, PokeType.None), new string[] { "じゅうなん", "じゅうなん" }, GenderRatio.Genderless));
            dexData.Add(new Species(133, "イーブイ", "", new uint[] { 55, 55, 50, 45, 65, 55 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "にげあし" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(134, "シャワーズ", "", new uint[] { 130, 65, 60, 110, 95, 65 }, (PokeType.Water, PokeType.None), new string[] { "ちょすい", "ちょすい" }, GenderRatio.M7F1));
            dexData.Add(new Species(135, "サンダース", "", new uint[] { 65, 65, 60, 110, 95, 130 }, (PokeType.Electric, PokeType.None), new string[] { "ちくでん", "ちくでん" }, GenderRatio.M7F1));
            dexData.Add(new Species(136, "ブースター", "", new uint[] { 65, 130, 60, 95, 110, 65 }, (PokeType.Fire, PokeType.None), new string[] { "もらいび", "もらいび" }, GenderRatio.M7F1));
            dexData.Add(new Species(137, "ポリゴン", "", new uint[] { 65, 60, 70, 85, 75, 40 }, (PokeType.Normal, PokeType.None), new string[] { "トレース", "トレース" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(138, "オムナイト", "", new uint[] { 35, 40, 100, 90, 55, 35 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "シェルアーマー" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(139, "オムスター", "", new uint[] { 70, 60, 125, 115, 70, 55 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(140, "カブト", "", new uint[] { 30, 80, 90, 55, 45, 55 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "カブトアーマー" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(141, "カブトプス", "", new uint[] { 60, 115, 105, 65, 70, 80 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "カブトアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(142, "プテラ", "", new uint[] { 80, 105, 65, 60, 75, 130 }, (PokeType.Rock, PokeType.Flying), new string[] { "いしあたま", "プレッシャー" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(143, "カビゴン", "", new uint[] { 160, 110, 65, 65, 110, 30 }, (PokeType.Normal, PokeType.None), new string[] { "めんえき", "あついしぼう" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(144, "フリーザー", "", new uint[] { 90, 85, 100, 95, 125, 85 }, (PokeType.Ice, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(145, "サンダー", "", new uint[] { 90, 90, 85, 125, 90, 100 }, (PokeType.Electric, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(146, "ファイヤー", "", new uint[] { 90, 100, 90, 125, 85, 90 }, (PokeType.Fire, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(147, "ミニリュウ", "", new uint[] { 41, 64, 45, 50, 50, 50 }, (PokeType.Dragon, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(148, "ハクリュー", "", new uint[] { 61, 84, 65, 70, 70, 70 }, (PokeType.Dragon, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(149, "カイリュー", "", new uint[] { 91, 134, 95, 100, 100, 80 }, (PokeType.Dragon, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(150, "ミュウツー", "", new uint[] { 106, 110, 90, 154, 90, 130 }, (PokeType.Psychic, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(151, "ミュウ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "シンクロ" }, GenderRatio.Genderless));
            dexData.Add(new Species(152, "チコリータ", "", new uint[] { 45, 49, 65, 49, 65, 45 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(153, "ベイリーフ", "", new uint[] { 60, 62, 80, 63, 80, 60 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(154, "メガニウム", "", new uint[] { 80, 82, 100, 83, 100, 80 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(155, "ヒノアラシ", "", new uint[] { 39, 52, 43, 60, 50, 65 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(156, "マグマラシ", "", new uint[] { 58, 64, 58, 80, 65, 80 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(157, "バクフーン", "", new uint[] { 78, 84, 78, 109, 85, 100 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(158, "ワニノコ", "", new uint[] { 50, 65, 64, 44, 48, 43 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(159, "アリゲイツ", "", new uint[] { 65, 80, 80, 59, 63, 58 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(160, "オーダイル", "", new uint[] { 85, 105, 100, 79, 83, 78 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(161, "オタチ", "", new uint[] { 35, 46, 34, 35, 45, 20 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(162, "オオタチ", "", new uint[] { 85, 76, 64, 45, 55, 90 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(163, "ホーホー", "", new uint[] { 60, 30, 30, 36, 56, 50 }, (PokeType.Normal, PokeType.Flying), new string[] { "ふみん", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(164, "ヨルノズク", "", new uint[] { 100, 50, 50, 76, 96, 70 }, (PokeType.Normal, PokeType.Flying), new string[] { "ふみん", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(165, "レディバ", "", new uint[] { 40, 20, 30, 40, 80, 55 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "はやおき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(166, "レディアン", "", new uint[] { 55, 35, 50, 55, 110, 85 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(167, "イトマル", "", new uint[] { 40, 60, 40, 40, 40, 30 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "ふみん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(168, "アリアドス", "", new uint[] { 70, 90, 70, 60, 60, 40 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new Species(169, "クロバット", "", new uint[] { 85, 90, 80, 70, 80, 130 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(170, "チョンチー", "", new uint[] { 75, 38, 38, 56, 56, 67 }, (PokeType.Water, PokeType.Electric), new string[] { "ちくでん", "はっこう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(171, "ランターン", "", new uint[] { 125, 58, 58, 76, 76, 67 }, (PokeType.Water, PokeType.Electric), new string[] { "ちくでん", "はっこう" }, GenderRatio.M1F1));
            dexData.Add(new Species(172, "ピチュー", "", new uint[] { 20, 40, 15, 35, 35, 60 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(173, "ピィ", "", new uint[] { 50, 25, 28, 45, 55, 15 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(174, "ププリン", "", new uint[] { 90, 30, 15, 40, 20, 15 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(175, "トゲピー", "", new uint[] { 35, 20, 65, 40, 65, 20 }, (PokeType.Normal, PokeType.None), new string[] { "はりきり", "てんのめぐみ" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(176, "トゲチック", "", new uint[] { 55, 40, 85, 80, 105, 40 }, (PokeType.Normal, PokeType.Flying), new string[] { "はりきり", "てんのめぐみ" }, GenderRatio.M7F1));
            dexData.Add(new Species(177, "ネイティ", "", new uint[] { 40, 50, 45, 70, 45, 70 }, (PokeType.Psychic, PokeType.Flying), new string[] { "シンクロ", "はやおき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(178, "ネイティオ", "", new uint[] { 65, 75, 70, 95, 70, 95 }, (PokeType.Psychic, PokeType.Flying), new string[] { "シンクロ", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(179, "メリープ", "", new uint[] { 55, 40, 40, 65, 45, 35 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(180, "モココ", "", new uint[] { 70, 55, 55, 80, 60, 45 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(181, "デンリュウ", "", new uint[] { 90, 75, 75, 115, 90, 55 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(182, "キレイハナ", "", new uint[] { 75, 80, 85, 90, 100, 50 }, (PokeType.Grass, PokeType.None), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(183, "マリル", "", new uint[] { 70, 20, 50, 20, 50, 40 }, (PokeType.Water, PokeType.None), new string[] { "あついしぼう", "ちからもち" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(184, "マリルリ", "", new uint[] { 100, 50, 80, 50, 80, 50 }, (PokeType.Water, PokeType.None), new string[] { "あついしぼう", "ちからもち" }, GenderRatio.M1F1));
            dexData.Add(new Species(185, "ウソッキー", "", new uint[] { 70, 100, 115, 30, 65, 30 }, (PokeType.Rock, PokeType.None), new string[] { "がんじょう", "いしあたま" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(186, "ニョロトノ", "", new uint[] { 90, 75, 75, 90, 100, 70 }, (PokeType.Water, PokeType.None), new string[] { "ちょすい", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(187, "ハネッコ", "", new uint[] { 35, 35, 40, 35, 55, 50 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(188, "ポポッコ", "", new uint[] { 55, 45, 50, 45, 65, 80 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(189, "ワタッコ", "", new uint[] { 75, 55, 70, 55, 85, 110 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(190, "エイパム", "", new uint[] { 55, 70, 55, 40, 55, 85 }, (PokeType.Normal, PokeType.None), new string[] { "にげあし", "ものひろい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(191, "ヒマナッツ", "", new uint[] { 30, 30, 30, 30, 30, 30 }, (PokeType.Grass, PokeType.None), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(192, "キマワリ", "", new uint[] { 75, 75, 55, 105, 85, 30 }, (PokeType.Grass, PokeType.None), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(193, "ヤンヤンマ", "", new uint[] { 65, 65, 45, 75, 45, 95 }, (PokeType.Bug, PokeType.Flying), new string[] { "かそく", "ふくがん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(194, "ウパー", "", new uint[] { 55, 45, 45, 25, 25, 15 }, (PokeType.Water, PokeType.Ground), new string[] { "しめりけ", "ちょすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(195, "ヌオー", "", new uint[] { 95, 85, 85, 65, 65, 35 }, (PokeType.Water, PokeType.Ground), new string[] { "しめりけ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(196, "エーフィ", "", new uint[] { 65, 65, 60, 130, 95, 110 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "シンクロ" }, GenderRatio.M7F1));
            dexData.Add(new Species(197, "ブラッキー", "", new uint[] { 95, 65, 110, 60, 130, 65 }, (PokeType.Dark, PokeType.None), new string[] { "シンクロ", "シンクロ" }, GenderRatio.M7F1));
            dexData.Add(new Species(198, "ヤミカラス", "", new uint[] { 60, 85, 42, 85, 42, 91 }, (PokeType.Dark, PokeType.Flying), new string[] { "ふみん", "ふみん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(199, "ヤドキング", "", new uint[] { 95, 75, 80, 100, 110, 30 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(200, "ムウマ", "", new uint[] { 60, 60, 60, 85, 85, 85 }, (PokeType.Ghost, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1, true));
            dexData.Add(new AnotherForm(201, "アンノーン", "A", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "B", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "C", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "D", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "E", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "F", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "G", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "H", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "I", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "J", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "K", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "L", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "M", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "N", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "O", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "P", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "Q", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "R", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "S", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "T", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "U", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "V", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "W", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "X", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "Y", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "Z", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "!", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(201, "アンノーン", "?", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(202, "ソーナンス", "", new uint[] { 190, 33, 58, 33, 58, 33 }, (PokeType.Psychic, PokeType.None), new string[] { "かげふみ", "かげふみ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(203, "キリンリキ", "", new uint[] { 70, 80, 65, 90, 65, 85 }, (PokeType.Normal, PokeType.Psychic), new string[] { "せいしんりょく", "はやおき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(204, "クヌギダマ", "", new uint[] { 50, 65, 90, 35, 35, 15 }, (PokeType.Bug, PokeType.None), new string[] { "がんじょう", "がんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(205, "フォレトス", "", new uint[] { 75, 90, 140, 60, 60, 40 }, (PokeType.Bug, PokeType.Steel), new string[] { "がんじょう", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(206, "ノコッチ", "", new uint[] { 100, 70, 70, 65, 65, 45 }, (PokeType.Normal, PokeType.None), new string[] { "てんのめぐみ", "にげあし" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(207, "グライガー", "", new uint[] { 65, 75, 105, 35, 65, 85 }, (PokeType.Ground, PokeType.Flying), new string[] { "かいりきバサミ", "すながくれ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(208, "ハガネール", "", new uint[] { 75, 85, 200, 55, 65, 30 }, (PokeType.Steel, PokeType.Ground), new string[] { "いしあたま", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(209, "ブルー", "", new uint[] { 60, 80, 50, 40, 40, 30 }, (PokeType.Normal, PokeType.None), new string[] { "いかく", "にげあし" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(210, "グランブル", "", new uint[] { 90, 120, 75, 60, 60, 45 }, (PokeType.Normal, PokeType.None), new string[] { "いかく", "にげあし" }, GenderRatio.M1F3));
            dexData.Add(new Species(211, "ハリーセン", "", new uint[] { 65, 95, 75, 55, 55, 85 }, (PokeType.Water, PokeType.Poison), new string[] { "どくのトゲ", "すいすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(212, "ハッサム", "", new uint[] { 70, 130, 100, 55, 80, 65 }, (PokeType.Bug, PokeType.Steel), new string[] { "むしのしらせ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(213, "ツボツボ", "", new uint[] { 20, 10, 230, 10, 230, 5 }, (PokeType.Bug, PokeType.Rock), new string[] { "がんじょう", "がんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(214, "ヘラクロス", "", new uint[] { 80, 125, 75, 40, 95, 85 }, (PokeType.Bug, PokeType.Fighting), new string[] { "むしのしらせ", "こんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(215, "ニューラ", "", new uint[] { 55, 95, 55, 35, 75, 115 }, (PokeType.Dark, PokeType.Ice), new string[] { "せいしんりょく", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(216, "ヒメグマ", "", new uint[] { 60, 80, 50, 50, 50, 40 }, (PokeType.Normal, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(217, "リングマ", "", new uint[] { 90, 130, 75, 75, 75, 55 }, (PokeType.Normal, PokeType.None), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(218, "マグマッグ", "", new uint[] { 40, 40, 40, 70, 40, 20 }, (PokeType.Fire, PokeType.None), new string[] { "マグマのよろい", "ほのおのからだ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(219, "マグカルゴ", "", new uint[] { 50, 50, 120, 80, 80, 30 }, (PokeType.Fire, PokeType.Rock), new string[] { "マグマのよろい", "ほのおのからだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(220, "ウリムー", "", new uint[] { 50, 50, 40, 30, 30, 50 }, (PokeType.Ice, PokeType.Ground), new string[] { "どんかん", "どんかん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(221, "イノムー", "", new uint[] { 100, 100, 80, 60, 60, 50 }, (PokeType.Ice, PokeType.Ground), new string[] { "どんかん", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(222, "サニーゴ", "", new uint[] { 55, 55, 85, 65, 85, 35 }, (PokeType.Water, PokeType.Rock), new string[] { "はりきり", "しぜんかいふく" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(223, "テッポウオ", "", new uint[] { 35, 65, 35, 65, 35, 65 }, (PokeType.Water, PokeType.None), new string[] { "はりきり", "はりきり" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(224, "オクタン", "", new uint[] { 75, 105, 75, 105, 75, 45 }, (PokeType.Water, PokeType.None), new string[] { "きゅうばん", "はいりき" }, GenderRatio.M1F1));
            dexData.Add(new Species(225, "デリバード", "", new uint[] { 45, 55, 45, 65, 45, 75 }, (PokeType.Ice, PokeType.Flying), new string[] { "やるき", "はりきり" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(226, "マンタイン", "", new uint[] { 65, 40, 70, 80, 140, 70 }, (PokeType.Water, PokeType.Flying), new string[] { "すいすい", "ちょすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(227, "エアームド", "", new uint[] { 65, 80, 140, 40, 70, 70 }, (PokeType.Steel, PokeType.Flying), new string[] { "するどいめ", "がんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(228, "デルビル", "", new uint[] { 45, 60, 30, 80, 50, 65 }, (PokeType.Dark, PokeType.Fire), new string[] { "はやおき", "もらいび" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(229, "ヘルガー", "", new uint[] { 75, 90, 50, 110, 80, 95 }, (PokeType.Dark, PokeType.Fire), new string[] { "はやおき", "もらいび" }, GenderRatio.M1F1));
            dexData.Add(new Species(230, "キングドラ", "", new uint[] { 75, 95, 95, 95, 95, 85 }, (PokeType.Water, PokeType.Dragon), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(231, "ゴマゾウ", "", new uint[] { 90, 60, 60, 40, 40, 40 }, (PokeType.Ground, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(232, "ドンファン", "", new uint[] { 90, 120, 120, 60, 60, 50 }, (PokeType.Ground, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(233, "ポリゴン2", "", new uint[] { 85, 80, 90, 105, 95, 60 }, (PokeType.Normal, PokeType.None), new string[] { "トレース", "トレース" }, GenderRatio.Genderless));
            dexData.Add(new Species(234, "オドシシ", "", new uint[] { 73, 95, 62, 85, 65, 85 }, (PokeType.Normal, PokeType.None), new string[] { "いかく", "いかく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(235, "ドーブル", "", new uint[] { 55, 20, 35, 20, 45, 75 }, (PokeType.Normal, PokeType.None), new string[] { "マイペース", "マイペース" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(236, "バルキー", "", new uint[] { 35, 35, 35, 35, 35, 35 }, (PokeType.Fighting, PokeType.None), new string[] { "こんじょう", "こんじょう" }, GenderRatio.MaleOnly, true));
            dexData.Add(new Species(237, "カポエラー", "", new uint[] { 50, 95, 95, 35, 110, 70 }, (PokeType.Fighting, PokeType.None), new string[] { "いかく", "いかく" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(238, "ムチュール", "", new uint[] { 45, 30, 15, 85, 65, 65 }, (PokeType.Ice, PokeType.Psychic), new string[] { "どんかん", "どんかん" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(239, "エレキッド", "", new uint[] { 45, 63, 37, 65, 55, 95 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "せいでんき" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(240, "ブビィ", "", new uint[] { 45, 75, 37, 70, 55, 83 }, (PokeType.Fire, PokeType.None), new string[] { "ほのおのからだ", "ほのおのからだ" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(241, "ミルタンク", "", new uint[] { 95, 80, 105, 40, 70, 100 }, (PokeType.Normal, PokeType.None), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(242, "ハピナス", "", new uint[] { 255, 10, 10, 75, 135, 55 }, (PokeType.Normal, PokeType.None), new string[] { "しぜんかいふく", "てんのめぐみ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(243, "ライコウ", "", new uint[] { 90, 85, 75, 115, 100, 115 }, (PokeType.Electric, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(244, "エンテイ", "", new uint[] { 115, 115, 85, 90, 75, 100 }, (PokeType.Fire, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(245, "スイクン", "", new uint[] { 100, 75, 115, 90, 115, 85 }, (PokeType.Water, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(246, "ヨーギラス", "", new uint[] { 50, 64, 50, 45, 50, 41 }, (PokeType.Rock, PokeType.Ground), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(247, "サナギラス", "", new uint[] { 70, 84, 70, 65, 70, 51 }, (PokeType.Rock, PokeType.Ground), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(248, "バンギラス", "", new uint[] { 100, 134, 110, 95, 100, 61 }, (PokeType.Rock, PokeType.Dark), new string[] { "すなおこし", "すなおこし" }, GenderRatio.M1F1));
            dexData.Add(new Species(249, "ルギア", "", new uint[] { 106, 90, 130, 90, 154, 110 }, (PokeType.Psychic, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(250, "ホウオウ", "", new uint[] { 106, 130, 90, 110, 154, 90 }, (PokeType.Fire, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(251, "セレビィ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Psychic, PokeType.Grass), new string[] { "しぜんかいふく", "しぜんかいふく" }, GenderRatio.Genderless));
            dexData.Add(new Species(252, "キモリ", "", new uint[] { 40, 45, 35, 65, 55, 70 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(253, "ジュプトル", "", new uint[] { 50, 65, 45, 85, 65, 95 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(254, "ジュカイン", "", new uint[] { 70, 85, 65, 105, 85, 120 }, (PokeType.Grass, PokeType.None), new string[] { "しんりょく", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(255, "アチャモ", "", new uint[] { 45, 60, 40, 70, 50, 45 }, (PokeType.Fire, PokeType.None), new string[] { "もうか", "もうか" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(256, "ワカシャモ", "", new uint[] { 60, 85, 60, 85, 60, 55 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(257, "バシャーモ", "", new uint[] { 80, 120, 70, 110, 70, 80 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(258, "ミズゴロウ", "", new uint[] { 50, 70, 50, 50, 50, 40 }, (PokeType.Water, PokeType.None), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(259, "ヌマクロー", "", new uint[] { 70, 85, 70, 60, 70, 50 }, (PokeType.Water, PokeType.Ground), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(260, "ラグラージ", "", new uint[] { 100, 110, 90, 85, 90, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "げきりゅう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(261, "ポチエナ", "", new uint[] { 35, 55, 35, 30, 30, 35 }, (PokeType.Dark, PokeType.None), new string[] { "にげあし", "にげあし" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(262, "グラエナ", "", new uint[] { 70, 90, 70, 60, 60, 70 }, (PokeType.Dark, PokeType.None), new string[] { "いかく", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(263, "ジグザグマ", "", new uint[] { 38, 30, 41, 30, 41, 60 }, (PokeType.Normal, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(264, "マッスグマ", "", new uint[] { 78, 70, 61, 50, 61, 100 }, (PokeType.Normal, PokeType.None), new string[] { "ものひろい", "ものひろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(265, "ケムッソ", "", new uint[] { 45, 45, 35, 20, 30, 20 }, (PokeType.Bug, PokeType.None), new string[] { "りんぷん", "りんぷん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(266, "カラサリス", "", new uint[] { 50, 35, 55, 25, 25, 15 }, (PokeType.Bug, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(267, "アゲハント", "", new uint[] { 60, 70, 50, 90, 50, 65 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(268, "マユルド", "", new uint[] { 50, 35, 55, 25, 25, 15 }, (PokeType.Bug, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(269, "ドクケイル", "", new uint[] { 60, 50, 70, 50, 90, 65 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "りんぷん" }, GenderRatio.M1F1));
            dexData.Add(new Species(270, "ハスボー", "", new uint[] { 40, 30, 30, 40, 50, 30 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(271, "ハスブレロ", "", new uint[] { 60, 50, 50, 60, 70, 50 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(272, "ルンパッパ", "", new uint[] { 80, 70, 70, 90, 100, 70 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(273, "タネボー", "", new uint[] { 40, 40, 50, 30, 30, 30 }, (PokeType.Grass, PokeType.None), new string[] { "ようりょくそ", "はやおき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(274, "コノハナ", "", new uint[] { 70, 70, 40, 60, 40, 60 }, (PokeType.Grass, PokeType.Dark), new string[] { "ようりょくそ", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(275, "ダーテング", "", new uint[] { 90, 100, 60, 90, 60, 80 }, (PokeType.Grass, PokeType.Dark), new string[] { "ようりょくそ", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(276, "スバメ", "", new uint[] { 40, 55, 30, 30, 30, 85 }, (PokeType.Normal, PokeType.Flying), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(277, "オオスバメ", "", new uint[] { 60, 85, 60, 50, 50, 125 }, (PokeType.Normal, PokeType.Flying), new string[] { "こんじょう", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(278, "キャモメ", "", new uint[] { 40, 30, 30, 55, 30, 85 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(279, "ペリッパー", "", new uint[] { 60, 50, 100, 85, 70, 65 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(280, "ラルトス", "", new uint[] { 28, 25, 25, 45, 35, 40 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "トレース" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(281, "キルリア", "", new uint[] { 38, 35, 35, 65, 55, 50 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "トレース" }, GenderRatio.M1F1));
            dexData.Add(new Species(282, "サーナイト", "", new uint[] { 68, 65, 65, 125, 115, 80 }, (PokeType.Psychic, PokeType.None), new string[] { "シンクロ", "トレース" }, GenderRatio.M1F1));
            dexData.Add(new Species(283, "アメタマ", "", new uint[] { 40, 30, 32, 50, 52, 65 }, (PokeType.Bug, PokeType.Water), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(284, "アメモース", "", new uint[] { 70, 60, 62, 80, 82, 60 }, (PokeType.Bug, PokeType.Flying), new string[] { "いかく", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(285, "キノココ", "", new uint[] { 60, 40, 60, 40, 60, 35 }, (PokeType.Grass, PokeType.None), new string[] { "ほうし", "ほうし" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(286, "キノガッサ", "", new uint[] { 60, 130, 80, 60, 60, 70 }, (PokeType.Grass, PokeType.Fighting), new string[] { "ほうし", "ほうし" }, GenderRatio.M1F1));
            dexData.Add(new Species(287, "ナマケロ", "", new uint[] { 60, 60, 60, 35, 35, 30 }, (PokeType.Normal, PokeType.None), new string[] { "なまけ", "なまけ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(288, "ヤルキモノ", "", new uint[] { 80, 80, 80, 55, 55, 90 }, (PokeType.Normal, PokeType.None), new string[] { "やるき", "やるき" }, GenderRatio.M1F1));
            dexData.Add(new Species(289, "ケッキング", "", new uint[] { 150, 160, 100, 95, 65, 100 }, (PokeType.Normal, PokeType.None), new string[] { "なまけ", "なまけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(290, "ツチニン", "", new uint[] { 31, 45, 90, 30, 30, 40 }, (PokeType.Bug, PokeType.Ground), new string[] { "ふくがん", "ふくがん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(291, "テッカニン", "", new uint[] { 61, 90, 45, 50, 50, 160 }, (PokeType.Bug, PokeType.Flying), new string[] { "かそく", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(292, "ヌケニン", "", new uint[] { 1, 90, 45, 30, 30, 40 }, (PokeType.Bug, PokeType.Ghost), new string[] { "ふしぎなまもり", "ふしぎなまもり" }, GenderRatio.Genderless));
            dexData.Add(new Species(293, "ゴニョニョ", "", new uint[] { 64, 51, 23, 51, 23, 28 }, (PokeType.Normal, PokeType.None), new string[] { "ぼうおん", "ぼうおん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(294, "ドゴーム", "", new uint[] { 84, 71, 43, 71, 43, 48 }, (PokeType.Normal, PokeType.None), new string[] { "ぼうおん", "ぼうおん" }, GenderRatio.M1F1));
            dexData.Add(new Species(295, "バクオング", "", new uint[] { 104, 91, 63, 91, 63, 68 }, (PokeType.Normal, PokeType.None), new string[] { "ぼうおん", "ぼうおん" }, GenderRatio.M1F1));
            dexData.Add(new Species(296, "マクノシタ", "", new uint[] { 72, 60, 30, 20, 30, 25 }, (PokeType.Fighting, PokeType.None), new string[] { "あついしぼう", "こんじょう" }, GenderRatio.M3F1, true));
            dexData.Add(new Species(297, "ハリテヤマ", "", new uint[] { 144, 120, 60, 40, 60, 50 }, (PokeType.Fighting, PokeType.None), new string[] { "あついしぼう", "こんじょう" }, GenderRatio.M3F1));
            dexData.Add(new Species(298, "ルリリ", "", new uint[] { 50, 20, 40, 20, 40, 20 }, (PokeType.Normal, PokeType.None), new string[] { "あついしぼう", "ちからもち" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(299, "ノズパス", "", new uint[] { 30, 45, 135, 45, 90, 30 }, (PokeType.Rock, PokeType.None), new string[] { "がんじょう", "じりょく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(300, "エネコ", "", new uint[] { 50, 45, 45, 35, 35, 50 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(301, "エネコロロ", "", new uint[] { 70, 65, 65, 55, 55, 70 }, (PokeType.Normal, PokeType.None), new string[] { "メロメロボディ", "メロメロボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(302, "ヤミラミ", "", new uint[] { 50, 75, 75, 65, 65, 50 }, (PokeType.Dark, PokeType.Ghost), new string[] { "するどいめ", "するどいめ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(303, "クチート", "", new uint[] { 50, 85, 85, 55, 55, 50 }, (PokeType.Steel, PokeType.None), new string[] { "かいりきバサミ", "いかく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(304, "ココドラ", "", new uint[] { 50, 70, 100, 40, 40, 30 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(305, "コドラ", "", new uint[] { 60, 90, 140, 50, 50, 40 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま" }, GenderRatio.M1F1));
            dexData.Add(new Species(306, "ボスゴドラ", "", new uint[] { 70, 110, 180, 60, 60, 50 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま" }, GenderRatio.M1F1));
            dexData.Add(new Species(307, "アサナン", "", new uint[] { 30, 40, 55, 40, 55, 60 }, (PokeType.Fighting, PokeType.Psychic), new string[] { "ヨガパワー", "ヨガパワー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(308, "チャーレム", "", new uint[] { 60, 60, 75, 60, 75, 80 }, (PokeType.Fighting, PokeType.Psychic), new string[] { "ヨガパワー", "ヨガパワー" }, GenderRatio.M1F1));
            dexData.Add(new Species(309, "ラクライ", "", new uint[] { 40, 45, 40, 65, 40, 65 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "ひらいしん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(310, "ライボルト", "", new uint[] { 70, 75, 60, 105, 60, 105 }, (PokeType.Electric, PokeType.None), new string[] { "せいでんき", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(311, "プラスル", "", new uint[] { 60, 50, 40, 85, 75, 95 }, (PokeType.Electric, PokeType.None), new string[] { "プラス", "プラス" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(312, "マイナン", "", new uint[] { 60, 40, 50, 75, 85, 95 }, (PokeType.Electric, PokeType.None), new string[] { "マイナス", "マイナス" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(313, "バルビート", "", new uint[] { 65, 73, 55, 47, 75, 85 }, (PokeType.Bug, PokeType.None), new string[] { "はっこう", "むしのしらせ" }, GenderRatio.MaleOnly, true));
            dexData.Add(new Species(314, "イルミーゼ", "", new uint[] { 65, 47, 55, 73, 75, 85 }, (PokeType.Bug, PokeType.None), new string[] { "どんかん", "どんかん" }, GenderRatio.FemaleOnly, true));
            dexData.Add(new Species(315, "ロゼリア", "", new uint[] { 50, 60, 45, 100, 80, 65 }, (PokeType.Grass, PokeType.Poison), new string[] { "しぜんかいふく", "どくのトゲ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(316, "ゴクリン", "", new uint[] { 70, 43, 53, 43, 53, 40 }, (PokeType.Poison, PokeType.None), new string[] { "ヘドロえき", "ねんちゃく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(317, "マルノーム", "", new uint[] { 100, 73, 83, 73, 83, 55 }, (PokeType.Poison, PokeType.None), new string[] { "ヘドロえき", "ねんちゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(318, "キバニア", "", new uint[] { 45, 90, 20, 65, 20, 65 }, (PokeType.Water, PokeType.Dark), new string[] { "さめはだ", "さめはだ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(319, "サメハダー", "", new uint[] { 70, 120, 40, 95, 40, 95 }, (PokeType.Water, PokeType.Dark), new string[] { "さめはだ", "さめはだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(320, "ホエルコ", "", new uint[] { 130, 70, 35, 70, 35, 60 }, (PokeType.Water, PokeType.None), new string[] { "みずのベール", "どんかん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(321, "ホエルオー", "", new uint[] { 170, 90, 45, 90, 45, 60 }, (PokeType.Water, PokeType.None), new string[] { "みずのベール", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(322, "ドンメル", "", new uint[] { 60, 60, 40, 65, 45, 35 }, (PokeType.Fire, PokeType.Ground), new string[] { "どんかん", "どんかん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(323, "バクーダ", "", new uint[] { 70, 100, 70, 105, 75, 40 }, (PokeType.Fire, PokeType.Ground), new string[] { "マグマのよろい", "マグマのよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(324, "コータス", "", new uint[] { 70, 85, 140, 85, 70, 20 }, (PokeType.Fire, PokeType.None), new string[] { "しろいけむり", "しろいけむり" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(325, "バネブー", "", new uint[] { 60, 25, 35, 70, 80, 60 }, (PokeType.Psychic, PokeType.None), new string[] { "あついしぼう", "マイペース" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(326, "ブーピッグ", "", new uint[] { 80, 45, 65, 90, 110, 80 }, (PokeType.Psychic, PokeType.None), new string[] { "あついしぼう", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(327, "パッチール", "", new uint[] { 60, 60, 60, 60, 60, 60 }, (PokeType.Normal, PokeType.None), new string[] { "マイペース", "マイペース" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(328, "ナックラー", "", new uint[] { 45, 100, 45, 45, 45, 10 }, (PokeType.Ground, PokeType.None), new string[] { "かいりきバサミ", "ありじごく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(329, "ビブラーバ", "", new uint[] { 50, 70, 50, 50, 50, 70 }, (PokeType.Ground, PokeType.Dragon), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(330, "フライゴン", "", new uint[] { 80, 100, 80, 80, 80, 100 }, (PokeType.Ground, PokeType.Dragon), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(331, "サボネア", "", new uint[] { 50, 85, 40, 85, 40, 35 }, (PokeType.Grass, PokeType.None), new string[] { "すながくれ", "すながくれ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(332, "ノクタス", "", new uint[] { 70, 115, 60, 115, 60, 55 }, (PokeType.Grass, PokeType.Dark), new string[] { "すながくれ", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(333, "チルット", "", new uint[] { 45, 40, 60, 40, 75, 50 }, (PokeType.Normal, PokeType.Flying), new string[] { "しぜんかいふく", "しぜんかいふく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(334, "チルタリス", "", new uint[] { 75, 70, 90, 70, 105, 80 }, (PokeType.Dragon, PokeType.Flying), new string[] { "しぜんかいふく", "しぜんかいふく" }, GenderRatio.M1F1));
            dexData.Add(new Species(335, "ザングース", "", new uint[] { 73, 115, 60, 60, 60, 90 }, (PokeType.Normal, PokeType.None), new string[] { "めんえき", "めんえき" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(336, "ハブネーク", "", new uint[] { 73, 100, 60, 100, 60, 65 }, (PokeType.Poison, PokeType.None), new string[] { "だっぴ", "だっぴ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(337, "ルナトーン", "", new uint[] { 70, 55, 65, 95, 85, 70 }, (PokeType.Rock, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(338, "ソルロック", "", new uint[] { 70, 95, 85, 55, 65, 70 }, (PokeType.Rock, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(339, "ドジョッチ", "", new uint[] { 50, 48, 43, 46, 41, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "どんかん", "どんかん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(340, "ナマズン", "", new uint[] { 110, 78, 73, 76, 71, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "どんかん", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(341, "ヘイガニ", "", new uint[] { 43, 80, 65, 50, 35, 35 }, (PokeType.Water, PokeType.None), new string[] { "かいりきバサミ", "シェルアーマー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(342, "シザリガー", "", new uint[] { 63, 120, 85, 90, 55, 55 }, (PokeType.Water, PokeType.Dark), new string[] { "かいりきバサミ", "シェルアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(343, "ヤジロン", "", new uint[] { 40, 40, 55, 40, 70, 55 }, (PokeType.Ground, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(344, "ネンドール", "", new uint[] { 60, 70, 105, 70, 120, 75 }, (PokeType.Ground, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(345, "リリーラ", "", new uint[] { 66, 41, 77, 61, 87, 23 }, (PokeType.Rock, PokeType.Grass), new string[] { "きゅうばん", "きゅうばん" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(346, "ユレイドル", "", new uint[] { 86, 81, 97, 81, 107, 43 }, (PokeType.Rock, PokeType.Grass), new string[] { "きゅうばん", "きゅうばん" }, GenderRatio.M7F1));
            dexData.Add(new Species(347, "アノプス", "", new uint[] { 45, 95, 50, 40, 50, 75 }, (PokeType.Rock, PokeType.Bug), new string[] { "カブトアーマー", "カブトアーマー" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(348, "アーマルド", "", new uint[] { 75, 125, 100, 70, 80, 45 }, (PokeType.Rock, PokeType.Bug), new string[] { "カブトアーマー", "カブトアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(349, "ヒンバス", "", new uint[] { 20, 15, 20, 10, 55, 80 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(350, "ミロカロス", "", new uint[] { 95, 60, 79, 100, 125, 81 }, (PokeType.Water, PokeType.None), new string[] { "ふしぎなうろこ", "ふしぎなうろこ" }, GenderRatio.M1F1));
            dexData.Add(new Species(351, "ポワルン", "", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Normal, PokeType.None), new string[] { "てんきや", "てんきや" }, GenderRatio.M1F1, true));
            dexData.Add(new AnotherForm(351, "ポワルン", "太陽", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Fire, PokeType.None), new string[] { "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(351, "ポワルン", "雨水", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Water, PokeType.None), new string[] { "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(351, "ポワルン", "雪雲", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Ice, PokeType.None), new string[] { "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new Species(352, "カクレオン", "", new uint[] { 60, 90, 70, 60, 120, 40 }, (PokeType.Normal, PokeType.None), new string[] { "へんしょく", "へんしょく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(353, "カゲボウズ", "", new uint[] { 44, 75, 35, 63, 33, 45 }, (PokeType.Ghost, PokeType.None), new string[] { "ふみん", "ふみん" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(354, "ジュペッタ", "", new uint[] { 64, 115, 65, 83, 63, 65 }, (PokeType.Ghost, PokeType.None), new string[] { "ふみん", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new Species(355, "ヨマワル", "", new uint[] { 20, 40, 90, 30, 90, 25 }, (PokeType.Ghost, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(356, "サマヨール", "", new uint[] { 40, 70, 130, 60, 130, 25 }, (PokeType.Ghost, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(357, "トロピウス", "", new uint[] { 99, 68, 83, 72, 87, 51 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(358, "チリーン", "", new uint[] { 65, 50, 70, 95, 80, 65 }, (PokeType.Psychic, PokeType.None), new string[] { "ふゆう", "ふゆう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(359, "アブソル", "", new uint[] { 65, 130, 60, 75, 60, 75 }, (PokeType.Dark, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(360, "ソーナノ", "", new uint[] { 95, 23, 48, 23, 48, 23 }, (PokeType.Psychic, PokeType.None), new string[] { "かげふみ", "かげふみ" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(361, "ユキワラシ", "", new uint[] { 50, 50, 50, 50, 50, 50 }, (PokeType.Ice, PokeType.None), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(362, "オニゴーリ", "", new uint[] { 80, 80, 80, 80, 80, 80 }, (PokeType.Ice, PokeType.None), new string[] { "せいしんりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(363, "タマザラシ", "", new uint[] { 70, 40, 50, 55, 50, 25 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(364, "トドグラー", "", new uint[] { 90, 60, 70, 75, 70, 45 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(365, "トドゼルガ", "", new uint[] { 110, 80, 90, 95, 90, 65 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(366, "パールル", "", new uint[] { 35, 64, 85, 74, 55, 32 }, (PokeType.Water, PokeType.None), new string[] { "シェルアーマー", "シェルアーマー" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(367, "ハンテール", "", new uint[] { 55, 104, 105, 94, 75, 52 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(368, "サクラビス", "", new uint[] { 55, 84, 105, 114, 75, 52 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(369, "ジーランス", "", new uint[] { 100, 90, 130, 45, 65, 55 }, (PokeType.Water, PokeType.Rock), new string[] { "すいすい", "いしあたま" }, GenderRatio.M7F1, true));
            dexData.Add(new Species(370, "ラブカス", "", new uint[] { 43, 30, 55, 40, 65, 97 }, (PokeType.Water, PokeType.None), new string[] { "すいすい", "すいすい" }, GenderRatio.M1F3, true));
            dexData.Add(new Species(371, "タツベイ", "", new uint[] { 45, 75, 60, 40, 30, 50 }, (PokeType.Dragon, PokeType.None), new string[] { "いしあたま", "いしあたま" }, GenderRatio.M1F1, true));
            dexData.Add(new Species(372, "コモルー", "", new uint[] { 65, 95, 100, 60, 50, 50 }, (PokeType.Dragon, PokeType.None), new string[] { "いしあたま", "いしあたま" }, GenderRatio.M1F1));
            dexData.Add(new Species(373, "ボーマンダ", "", new uint[] { 95, 135, 80, 110, 80, 100 }, (PokeType.Dragon, PokeType.Flying), new string[] { "いかく", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(374, "ダンバル", "", new uint[] { 40, 55, 80, 35, 60, 30 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless, true));
            dexData.Add(new Species(375, "メタング", "", new uint[] { 60, 75, 100, 55, 80, 50 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(376, "メタグロス", "", new uint[] { 80, 135, 130, 95, 90, 70 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(377, "レジロック", "", new uint[] { 80, 100, 200, 50, 100, 50 }, (PokeType.Rock, PokeType.None), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(378, "レジアイス", "", new uint[] { 80, 50, 100, 100, 200, 50 }, (PokeType.Ice, PokeType.None), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(379, "レジスチル", "", new uint[] { 80, 75, 150, 75, 150, 50 }, (PokeType.Steel, PokeType.None), new string[] { "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(380, "ラティアス", "", new uint[] { 80, 80, 90, 110, 130, 110 }, (PokeType.Dragon, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(381, "ラティオス", "", new uint[] { 80, 90, 80, 130, 110, 110 }, (PokeType.Dragon, PokeType.Psychic), new string[] { "ふゆう", "ふゆう" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(382, "カイオーガ", "", new uint[] { 100, 100, 90, 150, 140, 90 }, (PokeType.Water, PokeType.None), new string[] { "あめふらし", "あめふらし" }, GenderRatio.Genderless));
            dexData.Add(new Species(383, "グラードン", "", new uint[] { 100, 150, 140, 100, 90, 90 }, (PokeType.Ground, PokeType.None), new string[] { "ひでり", "ひでり" }, GenderRatio.Genderless));
            dexData.Add(new Species(384, "レックウザ", "", new uint[] { 105, 150, 90, 150, 90, 95 }, (PokeType.Dragon, PokeType.Flying), new string[] { "エアロック", "エアロック" }, GenderRatio.Genderless));
            dexData.Add(new Species(385, "ジラーチ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Steel, PokeType.Psychic), new string[] { "てんのめぐみ", "てんのめぐみ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "ノーマル", new uint[] { 50, 150, 50, 150, 50, 150 }, (PokeType.Psychic, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "アタック", new uint[] { 50, 180, 20, 180, 20, 150 }, (PokeType.Psychic, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "ディフェンス", new uint[] { 50, 70, 160, 70, 160, 90 }, (PokeType.Psychic, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "スピード", new uint[] { 50, 95, 90, 95, 90, 180 }, (PokeType.Psychic, PokeType.None), new string[] { "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));

            uniqueList = dexData.Distinct(new SpeciesComparer()).ToArray();
            uniqueDex = uniqueList.ToDictionary(_ => _.Name, _ => _);
            dexDictionary = dexData.ToDictionary(_ => $"{_.Name}#{_.Form}", _ => _);
            formDex = dexData.ToLookup(_ => _.Name);
        }
    }

    class SpeciesComparer : IEqualityComparer<Pokemon.Species>
    {
        public bool Equals(Pokemon.Species x, Pokemon.Species y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.Name == y.Name;
        }

        public int GetHashCode(Pokemon.Species s) => s.Name.GetHashCode();
    }
}
