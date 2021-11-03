﻿using System;
using System.Collections.Generic;
using System.Linq;
using PokemonStandardLibrary.CommonExtension;

namespace PokemonStandardLibrary.PokeDex.Gen7
{
    public class Pokemon
    {
        public class Species
        {
            public readonly int DexID;
            public readonly string Name;
            public readonly uint[] BS;
            public readonly string[] Ability;
            public readonly (PokeType Type1, PokeType Type2) Type;
            public readonly GenderRatio GenderRatio;
            public readonly string FormName;

            public virtual string GetDefaultName() { return Name; }

            public Individual GetIndividual(uint Lv, uint[] IVs, uint EC, uint PID, Nature nature, uint AbilityIndex, Gender gender)
            {
                return new Individual()
                {
                    Name = Name,
                    Form = FormName,
                    EC = EC,
                    PID = PID,
                    Lv = Lv,
                    Stats = GetStats(IVs, nature, Lv),
                    IVs = IVs,
                    Nature = nature,
                    Ability = Ability[AbilityIndex],
                    Gender = gender
                };
            }

            public (uint[] Min, uint[] Max) CalcIVsRange(uint[] Stats, uint Lv, Nature nature)
            {
                uint[] MinIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
                uint[] MaxIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };

                var mag = nature.ToMagnifications();

                uint stat;
                for (MinIVs[0] = 0; MinIVs[0] < 32; MinIVs[0]++)
                {
                    stat = (MinIVs[0] + BS[0] * 2) * Lv / 100 + 10 + Lv;
                    if (stat == Stats[0]) break;
                }
                if (MinIVs[0] != 32)
                {
                    for (MaxIVs[0] = MinIVs[0]; MaxIVs[0] < 32; MaxIVs[0]++)
                    {
                        stat = (MaxIVs[0] + 1 + BS[0] * 2) * Lv / 100 + 10 + Lv;
                        if (stat != Stats[0]) break;
                    }
                    MaxIVs[0] = Math.Min(MaxIVs[0], 31);
                }

                for (int i = 1; i < 6; i++)
                {
                    for (MinIVs[i] = 0; MinIVs[i] < 32; MinIVs[i]++)
                    {
                        stat = (uint)(((MinIVs[i] + BS[i] * 2) * Lv / 100 + 5) * mag[i]);
                        if (stat == Stats[i]) break;
                    }
                    if (MinIVs[i] != 32)
                    {
                        for (MaxIVs[i] = MinIVs[i]; MaxIVs[i] < 32; MaxIVs[i]++)
                        {
                            stat = (uint)(((MaxIVs[i] + 1 + BS[i] * 2) * Lv / 100 + 5) * mag[i]);
                            if (stat != Stats[i]) break;
                        }
                        MaxIVs[i] = Math.Min(MaxIVs[i], 31);
                    }
                }

                return (MinIVs, MaxIVs);
            }
            public (uint[] Min, uint[] Max) CalcIVsRange(uint[] Stats, uint[] EVs, uint Lv, Nature nature)
            {
                uint[] MinIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
                uint[] MaxIVs = new uint[6] { 32, 32, 32, 32, 32, 32 };
                EVs = EVs.Select(_ => _ / 4).ToArray();

                var mag = nature.ToMagnifications();

                uint stat;
                for (MinIVs[0] = 0; MinIVs[0] < 32; MinIVs[0]++)
                {
                    stat = (MinIVs[0] + EVs[0] + BS[0] * 2) * Lv / 100 + 10 + Lv;
                    if (stat == Stats[0]) break;
                }
                if (MinIVs[0] != 32)
                {
                    for (MaxIVs[0] = MinIVs[0]; MaxIVs[0] < 32; MaxIVs[0]++)
                    {
                        stat = (MaxIVs[0] + EVs[0] + 1 + BS[0] * 2) * Lv / 100 + 10 + Lv;
                        if (stat != Stats[0]) break;
                    }
                    MaxIVs[0] = Math.Min(MaxIVs[0], 31);
                }

                for (int i = 1; i < 6; i++)
                {
                    for (MinIVs[i] = 0; MinIVs[i] < 32; MinIVs[i]++)
                    {
                        stat = (uint)(((MinIVs[i] + EVs[i] + BS[i] * 2) * Lv / 100 + 5) * mag[i]);
                        if (stat == Stats[i]) break;
                    }
                    if (MinIVs[i] != 32)
                    {
                        for (MaxIVs[i] = MinIVs[i]; MaxIVs[i] < 32; MaxIVs[i]++)
                        {
                            stat = (uint)(((MaxIVs[i] + EVs[i] + 1 + BS[i] * 2) * Lv / 100 + 5) * mag[i]);
                            if (stat != Stats[i]) break;
                        }
                        MaxIVs[i] = Math.Min(MaxIVs[i], 31);
                    }
                }

                return (MinIVs, MaxIVs);
            }
            private uint[] GetStats(uint[] IVs, Nature Nature = Nature.Hardy, uint Lv = 50)
            {
                uint[] stats = new uint[6];
                var mag = Nature.ToMagnifications();

                stats[0] = (IVs[0] + BS[0] * 2) * Lv / 100 + 10 + Lv;
                if (Name == "ヌケニン") stats[0] = 1;
                for (int i = 1; i < 6; i++)
                    stats[i] = (uint)(((IVs[i] + BS[i] * 2) * Lv / 100 + 5) * mag[i]);

                return stats;
            }

            internal Species(int dexID, string name, string formName, uint[] bs, (PokeType type1, PokeType type2) type, string[] ability, GenderRatio ratio)
            {
                DexID = dexID;
                Name = name;
                FormName = formName;
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
            public override string GetDefaultName()
            {
                return $"{Name}({FormName})";
            }
        }

        public class Individual
        {
            public string Name { get; internal set; }
            public string Form { get; internal set; }
            public uint Lv { get; internal set; }
            public uint EC { get; internal set; }
            public uint PID { get; internal set; }
            public Nature Nature { get; internal set; }
            public Gender Gender { get; internal set; }
            public string Ability { get; internal set; }
            public uint[] IVs { get; internal set; }
            public uint[] Stats { get; internal set; }
            public ShinyType Shiny { get; internal set; }
            public Individual SetShinyType(ShinyType value) { Shiny = value; return this; }
            internal Individual() { }

            public static Individual Empty = GetPokemon("Dummy").GetIndividual(0, new uint[6], 0, 0, Nature.Hardy, 0, Gender.Genderless);
        }

        private static readonly IReadOnlyList<Species> uniqueList;
        private static readonly List<Species> dexData;
        private static readonly ILookup<string, Species> formDex;
        private static readonly Dictionary<string, Species> uniqueDex;
        private static readonly Dictionary<string, Species> dexDictionary;

        private Pokemon() { }
        public static Species GetPokemon(string Name)
        {
            if (!uniqueDex.ContainsKey(Name)) throw new Exception($"{Name}は登録されていません");
            return uniqueDex[Name];
        }
        public static Species GetPokemon(string Name, string Form)
        {
            if (!dexDictionary.ContainsKey(Name + Form)) throw new Exception($"{Name + Form}は登録されていません");
            return dexDictionary[Name + Form];
        }
        public static IReadOnlyList<Species> GetAllForms(string Name)
        {
            return formDex[Name].ToArray();
        }
        public static IReadOnlyList<Species> GetUniquePokemonList() => uniqueList.Where(_ => _.Name != "Dummy").ToArray();
        public static IReadOnlyList<Species> GetAllPokemonList() => dexData.Where(_ => _.Name != "Dummy").ToArray();
        static Pokemon()
        {
            dexData = new List<Species>();

            dexData.Add(new Species(-1, "Dummy", "Genderless", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.Genderless));
            dexData.Add(new Species(-1, "Dummy", "MaleOnly", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(-1, "Dummy", "M7F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M7F1));
            dexData.Add(new Species(-1, "Dummy", "M3F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M3F1));
            dexData.Add(new Species(-1, "Dummy", "M1F1", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M1F1));
            dexData.Add(new Species(-1, "Dummy", "M1F3", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.M1F3));
            dexData.Add(new Species(-1, "Dummy", "FemaleOnly", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "特性1", "特性2", "隠れ特性" }, GenderRatio.FemaleOnly));

            dexData.Add(new Species(1, "フシギダネ", "", new uint[] { 45, 49, 49, 65, 65, 45 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく", "ようりょくそ" }, GenderRatio.M7F1));
            dexData.Add(new Species(2, "フシギソウ", "", new uint[] { 60, 62, 63, 80, 80, 60 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく", "ようりょくそ" }, GenderRatio.M7F1));
            dexData.Add(new Species(3, "フシギバナ", "", new uint[] { 80, 82, 83, 100, 100, 80 }, (PokeType.Grass, PokeType.Poison), new string[] { "しんりょく", "しんりょく", "ようりょくそ" }, GenderRatio.M7F1));
            dexData.Add(new Species(4, "ヒトカゲ", "", new uint[] { 39, 52, 43, 60, 50, 65 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "サンパワー" }, GenderRatio.M7F1));
            dexData.Add(new Species(5, "リザード", "", new uint[] { 58, 64, 58, 80, 65, 80 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "サンパワー" }, GenderRatio.M7F1));
            dexData.Add(new Species(6, "リザードン", "", new uint[] { 78, 84, 78, 109, 85, 100 }, (PokeType.Fire, PokeType.Flying), new string[] { "もうか", "もうか", "サンパワー" }, GenderRatio.M7F1));
            dexData.Add(new Species(7, "ゼニガメ", "", new uint[] { 44, 48, 65, 50, 64, 43 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "あめうけざら" }, GenderRatio.M7F1));
            dexData.Add(new Species(8, "カメール", "", new uint[] { 59, 63, 80, 65, 80, 58 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "あめうけざら" }, GenderRatio.M7F1));
            dexData.Add(new Species(9, "カメックス", "", new uint[] { 79, 83, 100, 85, 105, 78 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "あめうけざら" }, GenderRatio.M7F1));
            dexData.Add(new Species(10, "キャタピー", "", new uint[] { 45, 30, 35, 20, 20, 45 }, (PokeType.Bug, PokeType.Non), new string[] { "りんぷん", "りんぷん", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(11, "トランセル", "", new uint[] { 50, 20, 55, 25, 25, 30 }, (PokeType.Bug, PokeType.Non), new string[] { "だっぴ", "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(12, "バタフリー", "", new uint[] { 60, 45, 50, 90, 80, 70 }, (PokeType.Bug, PokeType.Flying), new string[] { "ふくがん", "ふくがん", "いろめがね" }, GenderRatio.M1F1));
            dexData.Add(new Species(13, "ビードル", "", new uint[] { 40, 35, 30, 20, 20, 50 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "りんぷん", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(14, "コクーン", "", new uint[] { 45, 25, 50, 25, 25, 35 }, (PokeType.Bug, PokeType.Poison), new string[] { "だっぴ", "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(15, "スピアー", "", new uint[] { 65, 90, 40, 45, 80, 75 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "むしのしらせ", "スナイパー" }, GenderRatio.M1F1));
            dexData.Add(new Species(16, "ポッポ", "", new uint[] { 40, 45, 40, 35, 35, 56 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちどりあし", "はとむね" }, GenderRatio.M1F1));
            dexData.Add(new Species(17, "ピジョン", "", new uint[] { 63, 60, 55, 50, 50, 71 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちどりあし", "はとむね" }, GenderRatio.M1F1));
            dexData.Add(new Species(18, "ピジョット", "", new uint[] { 83, 80, 75, 70, 70, 101 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちどりあし", "はとむね" }, GenderRatio.M1F1));
            dexData.Add(new Species(19, "コラッタ", "", new uint[] { 30, 56, 35, 25, 35, 72 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "こんじょう", "はりきり" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(19, "コラッタ", "アローラ", new uint[] { 30, 56, 35, 25, 35, 72 }, (PokeType.Dark, PokeType.Normal), new string[] { "くいしんぼう", "はりきり", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(20, "ラッタ", "", new uint[] { 55, 81, 60, 50, 70, 97 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "こんじょう", "はりきり" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(20, "ラッタ", "アローラ", new uint[] { 75, 71, 70, 40, 80, 77 }, (PokeType.Dark, PokeType.Normal), new string[] { "くいしんぼう", "はりきり", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(21, "オニスズメ", "", new uint[] { 40, 60, 30, 31, 31, 70 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ", "スナイパー" }, GenderRatio.M1F1));
            dexData.Add(new Species(22, "オニドリル", "", new uint[] { 65, 90, 65, 61, 61, 100 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ", "スナイパー" }, GenderRatio.M1F1));
            dexData.Add(new Species(23, "アーボ", "", new uint[] { 35, 60, 44, 40, 54, 55 }, (PokeType.Poison, PokeType.Non), new string[] { "いかく", "だっぴ", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(24, "アーボック", "", new uint[] { 60, 95, 69, 65, 79, 80 }, (PokeType.Poison, PokeType.Non), new string[] { "いかく", "だっぴ", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(25, "ピカチュウ", "", new uint[] { 35, 55, 40, 50, 50, 90 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(26, "ライチュウ", "", new uint[] { 60, 90, 55, 90, 80, 110 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(26, "ライチュウ", "アローラ", new uint[] { 60, 85, 50, 95, 85, 110 }, (PokeType.Electric, PokeType.Psychic), new string[] { "サーフテール", "サーフテール", "サーフテール" }, GenderRatio.M1F1));
            dexData.Add(new Species(27, "サンド", "", new uint[] { 50, 75, 85, 20, 30, 40 }, (PokeType.Ground, PokeType.Non), new string[] { "すながくれ", "すながくれ", "すなかき" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(27, "サンド", "アローラ", new uint[] { 50, 75, 90, 10, 35, 40 }, (PokeType.Ice, PokeType.Steel), new string[] { "ゆきがくれ", "ゆきがくれ", "ゆきかき" }, GenderRatio.M1F1));
            dexData.Add(new Species(28, "サンドパン", "", new uint[] { 75, 100, 110, 45, 55, 65 }, (PokeType.Ground, PokeType.Non), new string[] { "すながくれ", "すながくれ", "すなかき" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(28, "サンドパン", "アローラ", new uint[] { 75, 100, 120, 25, 65, 65 }, (PokeType.Ice, PokeType.Steel), new string[] { "ゆきがくれ", "ゆきがくれ", "ゆきかき" }, GenderRatio.M1F1));
            dexData.Add(new Species(29, "ニドラン♀", "", new uint[] { 55, 47, 52, 40, 40, 41 }, (PokeType.Poison, PokeType.Non), new string[] { "どくのトゲ", "とうそうしん", "はりきり" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(30, "ニドリーナ", "", new uint[] { 70, 62, 67, 55, 55, 56 }, (PokeType.Poison, PokeType.Non), new string[] { "どくのトゲ", "とうそうしん", "はりきり" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(31, "ニドクイン", "", new uint[] { 90, 92, 87, 75, 85, 76 }, (PokeType.Poison, PokeType.Ground), new string[] { "どくのトゲ", "とうそうしん", "ちからずく" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(32, "ニドラン♂", "", new uint[] { 46, 57, 40, 40, 40, 50 }, (PokeType.Poison, PokeType.Non), new string[] { "どくのトゲ", "とうそうしん", "はりきり" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(33, "ニドリーノ", "", new uint[] { 61, 72, 57, 55, 55, 65 }, (PokeType.Poison, PokeType.Non), new string[] { "どくのトゲ", "とうそうしん", "はりきり" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(34, "ニドキング", "", new uint[] { 81, 102, 77, 85, 75, 85 }, (PokeType.Poison, PokeType.Ground), new string[] { "どくのトゲ", "とうそうしん", "ちからずく" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(35, "ピッピ", "", new uint[] { 70, 45, 48, 60, 65, 35 }, (PokeType.Fairy, PokeType.Non), new string[] { "メロメロボディ", "マジックガード", "フレンドガード" }, GenderRatio.M1F3));
            dexData.Add(new Species(36, "ピクシー", "", new uint[] { 95, 70, 73, 95, 90, 60 }, (PokeType.Fairy, PokeType.Non), new string[] { "メロメロボディ", "マジックガード", "てんねん" }, GenderRatio.M1F3));
            dexData.Add(new Species(37, "ロコン", "", new uint[] { 38, 41, 40, 50, 65, 65 }, (PokeType.Fire, PokeType.Non), new string[] { "もらいび", "もらいび", "ひでり" }, GenderRatio.M1F3));
            dexData.Add(new AnotherForm(37, "ロコン", "アローラ", new uint[] { 38, 41, 40, 50, 65, 65 }, (PokeType.Ice, PokeType.Non), new string[] { "ゆきがくれ", "ゆきがくれ", "ゆきふらし" }, GenderRatio.M1F3));
            dexData.Add(new Species(38, "キュウコン", "", new uint[] { 73, 76, 75, 81, 100, 100 }, (PokeType.Fire, PokeType.Non), new string[] { "もらいび", "もらいび", "ひでり" }, GenderRatio.M1F3));
            dexData.Add(new AnotherForm(38, "キュウコン", "アローラ", new uint[] { 73, 67, 75, 81, 100, 109 }, (PokeType.Ice, PokeType.Fairy), new string[] { "ゆきがくれ", "ゆきがくれ", "ゆきふらし" }, GenderRatio.M1F3));
            dexData.Add(new Species(39, "プリン", "", new uint[] { 115, 45, 20, 45, 25, 20 }, (PokeType.Normal, PokeType.Fairy), new string[] { "メロメロボディ", "かちき", "フレンドガード" }, GenderRatio.M1F3));
            dexData.Add(new Species(40, "プクリン", "", new uint[] { 140, 70, 45, 85, 50, 45 }, (PokeType.Normal, PokeType.Fairy), new string[] { "メロメロボディ", "かちき", "おみとおし" }, GenderRatio.M1F3));
            dexData.Add(new Species(41, "ズバット", "", new uint[] { 40, 45, 35, 30, 40, 55 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(42, "ゴルバット", "", new uint[] { 75, 80, 70, 65, 75, 90 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(43, "ナゾノクサ", "", new uint[] { 45, 50, 55, 75, 65, 30 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(44, "クサイハナ", "", new uint[] { 60, 65, 70, 85, 75, 40 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "あくしゅう" }, GenderRatio.M1F1));
            dexData.Add(new Species(45, "ラフレシア", "", new uint[] { 75, 80, 85, 110, 90, 50 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "ほうし" }, GenderRatio.M1F1));
            dexData.Add(new Species(46, "パラス", "", new uint[] { 35, 70, 55, 45, 55, 25 }, (PokeType.Bug, PokeType.Grass), new string[] { "ほうし", "かんそうはだ", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(47, "パラセクト", "", new uint[] { 60, 95, 80, 60, 80, 30 }, (PokeType.Bug, PokeType.Grass), new string[] { "ほうし", "かんそうはだ", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(48, "コンパン", "", new uint[] { 60, 55, 50, 40, 55, 45 }, (PokeType.Bug, PokeType.Poison), new string[] { "ふくがん", "いろめがね", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(49, "モルフォン", "", new uint[] { 70, 65, 60, 90, 75, 90 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "いろめがね", "ミラクルスキン" }, GenderRatio.M1F1));
            dexData.Add(new Species(50, "ディグダ", "", new uint[] { 10, 55, 25, 35, 45, 95 }, (PokeType.Ground, PokeType.Non), new string[] { "すながくれ", "ありじごく", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(50, "ディグダ", "アローラ", new uint[] { 10, 55, 30, 35, 45, 90 }, (PokeType.Ground, PokeType.Steel), new string[] { "すながくれ", "カーリーヘアー", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(51, "ダグトリオ", "", new uint[] { 35, 100, 50, 50, 70, 120 }, (PokeType.Ground, PokeType.Non), new string[] { "すながくれ", "ありじごく", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(51, "ダグトリオ", "アローラ", new uint[] { 35, 100, 60, 50, 70, 110 }, (PokeType.Ground, PokeType.Steel), new string[] { "すながくれ", "カーリーヘアー", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(52, "ニャース", "", new uint[] { 40, 45, 35, 40, 40, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "テクニシャン", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(52, "ニャース", "アローラ", new uint[] { 40, 35, 35, 50, 40, 90 }, (PokeType.Dark, PokeType.Non), new string[] { "ものひろい", "テクニシャン", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(53, "ペルシアン", "", new uint[] { 65, 70, 60, 65, 65, 115 }, (PokeType.Normal, PokeType.Non), new string[] { "じゅうなん", "テクニシャン", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(53, "ペルシアン", "アローラ", new uint[] { 65, 60, 60, 75, 65, 115 }, (PokeType.Dark, PokeType.Non), new string[] { "ファーコート", "テクニシャン", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(54, "コダック", "", new uint[] { 50, 52, 48, 65, 50, 55 }, (PokeType.Water, PokeType.Non), new string[] { "しめりけ", "ノーてんき", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(55, "ゴルダック", "", new uint[] { 80, 82, 78, 95, 80, 85 }, (PokeType.Water, PokeType.Non), new string[] { "しめりけ", "ノーてんき", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(56, "マンキー", "", new uint[] { 40, 80, 35, 35, 45, 70 }, (PokeType.Fighting, PokeType.Non), new string[] { "やるき", "いかりのつぼ", "まけんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(57, "オコリザル", "", new uint[] { 65, 105, 60, 60, 70, 95 }, (PokeType.Fighting, PokeType.Non), new string[] { "やるき", "いかりのつぼ", "まけんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(58, "ガーディ", "", new uint[] { 55, 70, 45, 70, 50, 60 }, (PokeType.Fire, PokeType.Non), new string[] { "いかく", "もらいび", "せいぎのこころ" }, GenderRatio.M3F1));
            dexData.Add(new Species(59, "ウインディ", "", new uint[] { 90, 110, 80, 100, 80, 95 }, (PokeType.Fire, PokeType.Non), new string[] { "いかく", "もらいび", "せいぎのこころ" }, GenderRatio.M3F1));
            dexData.Add(new Species(60, "ニョロモ", "", new uint[] { 40, 50, 40, 40, 40, 90 }, (PokeType.Water, PokeType.Non), new string[] { "ちょすい", "しめりけ", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(61, "ニョロゾ", "", new uint[] { 65, 65, 65, 50, 50, 90 }, (PokeType.Water, PokeType.Non), new string[] { "ちょすい", "しめりけ", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(62, "ニョロボン", "", new uint[] { 90, 95, 95, 70, 90, 70 }, (PokeType.Water, PokeType.Fighting), new string[] { "ちょすい", "しめりけ", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(63, "ケーシィ", "", new uint[] { 25, 20, 15, 105, 55, 90 }, (PokeType.Psychic, PokeType.Non), new string[] { "シンクロ", "せいしんりょく", "マジックガード" }, GenderRatio.M3F1));
            dexData.Add(new Species(64, "ユンゲラー", "", new uint[] { 40, 35, 30, 120, 70, 105 }, (PokeType.Psychic, PokeType.Non), new string[] { "シンクロ", "せいしんりょく", "マジックガード" }, GenderRatio.M3F1));
            dexData.Add(new Species(65, "フーディン", "", new uint[] { 55, 50, 45, 135, 95, 120 }, (PokeType.Psychic, PokeType.Non), new string[] { "シンクロ", "せいしんりょく", "マジックガード" }, GenderRatio.M3F1));
            dexData.Add(new Species(66, "ワンリキー", "", new uint[] { 70, 80, 50, 35, 35, 35 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ノーガード", "ふくつのこころ" }, GenderRatio.M3F1));
            dexData.Add(new Species(67, "ゴーリキー", "", new uint[] { 80, 100, 70, 50, 60, 45 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ノーガード", "ふくつのこころ" }, GenderRatio.M3F1));
            dexData.Add(new Species(68, "カイリキー", "", new uint[] { 90, 130, 80, 65, 85, 55 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ノーガード", "ふくつのこころ" }, GenderRatio.M3F1));
            dexData.Add(new Species(69, "マダツボミ", "", new uint[] { 50, 75, 35, 70, 30, 40 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(70, "ウツドン", "", new uint[] { 65, 90, 50, 85, 45, 55 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(71, "ウツボット", "", new uint[] { 80, 105, 65, 100, 70, 70 }, (PokeType.Grass, PokeType.Poison), new string[] { "ようりょくそ", "ようりょくそ", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(72, "メノクラゲ", "", new uint[] { 40, 40, 35, 50, 100, 70 }, (PokeType.Water, PokeType.Poison), new string[] { "クリアボディ", "ヘドロえき", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(73, "ドククラゲ", "", new uint[] { 80, 70, 65, 80, 120, 100 }, (PokeType.Water, PokeType.Poison), new string[] { "クリアボディ", "ヘドロえき", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(74, "イシツブテ", "", new uint[] { 40, 80, 100, 30, 30, 20 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(74, "イシツブテ", "アローラ", new uint[] { 40, 80, 100, 30, 30, 20 }, (PokeType.Rock, PokeType.Electric), new string[] { "じりょく", "がんじょう", "エレキスキン" }, GenderRatio.M1F1));
            dexData.Add(new Species(75, "ゴローン", "", new uint[] { 55, 95, 115, 45, 45, 35 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(75, "ゴローン", "アローラ", new uint[] { 55, 95, 115, 45, 45, 35 }, (PokeType.Rock, PokeType.Electric), new string[] { "じりょく", "がんじょう", "エレキスキン" }, GenderRatio.M1F1));
            dexData.Add(new Species(76, "ゴローニャ", "", new uint[] { 80, 120, 130, 55, 65, 45 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(76, "ゴローニャ", "アローラ", new uint[] { 80, 120, 130, 55, 65, 45 }, (PokeType.Rock, PokeType.Electric), new string[] { "じりょく", "がんじょう", "エレキスキン" }, GenderRatio.M1F1));
            dexData.Add(new Species(77, "ポニータ", "", new uint[] { 50, 85, 55, 65, 65, 90 }, (PokeType.Fire, PokeType.Non), new string[] { "にげあし", "もらいび", "ほのおのからだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(78, "ギャロップ", "", new uint[] { 65, 100, 70, 80, 80, 105 }, (PokeType.Fire, PokeType.Non), new string[] { "にげあし", "もらいび", "ほのおのからだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(79, "ヤドン", "", new uint[] { 90, 65, 65, 40, 40, 15 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(80, "ヤドラン", "", new uint[] { 95, 75, 110, 100, 80, 30 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(81, "コイル", "", new uint[] { 25, 35, 70, 95, 55, 45 }, (PokeType.Electric, PokeType.Steel), new string[] { "じりょく", "がんじょう", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(82, "レアコイル", "", new uint[] { 50, 60, 95, 120, 70, 70 }, (PokeType.Electric, PokeType.Steel), new string[] { "じりょく", "がんじょう", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(83, "カモネギ", "", new uint[] { 52, 90, 55, 58, 62, 60 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "せいしんりょく", "まけんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(84, "ドードー", "", new uint[] { 35, 85, 45, 35, 35, 75 }, (PokeType.Normal, PokeType.Flying), new string[] { "にげあし", "はやおき", "ちどりあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(85, "ドードリオ", "", new uint[] { 60, 110, 70, 60, 60, 110 }, (PokeType.Normal, PokeType.Flying), new string[] { "にげあし", "はやおき", "ちどりあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(86, "パウワウ", "", new uint[] { 65, 45, 55, 45, 70, 45 }, (PokeType.Water, PokeType.Non), new string[] { "あついしぼう", "うるおいボディ", "アイスボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(87, "ジュゴン", "", new uint[] { 90, 70, 80, 70, 95, 70 }, (PokeType.Water, PokeType.Ice), new string[] { "あついしぼう", "うるおいボディ", "アイスボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(88, "ベトベター", "", new uint[] { 80, 80, 50, 40, 50, 25 }, (PokeType.Poison, PokeType.Non), new string[] { "あくしゅう", "ねんちゃく", "どくしゅ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(88, "ベトベター", "アローラ", new uint[] { 80, 80, 50, 40, 50, 25 }, (PokeType.Poison, PokeType.Dark), new string[] { "どくしゅ", "くいしんぼう", "かがくのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(89, "ベトベトン", "", new uint[] { 105, 105, 75, 65, 100, 50 }, (PokeType.Poison, PokeType.Non), new string[] { "あくしゅう", "ねんちゃく", "どくしゅ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(89, "ベトベトン", "アローラ", new uint[] { 105, 105, 75, 65, 100, 50 }, (PokeType.Poison, PokeType.Dark), new string[] { "どくしゅ", "くいしんぼう", "かがくのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(90, "シェルダー", "", new uint[] { 30, 65, 100, 45, 25, 40 }, (PokeType.Water, PokeType.Non), new string[] { "シェルアーマー", "スキルリンク", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(91, "パルシェン", "", new uint[] { 50, 95, 180, 85, 45, 70 }, (PokeType.Water, PokeType.Ice), new string[] { "シェルアーマー", "スキルリンク", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(92, "ゴース", "", new uint[] { 30, 35, 30, 100, 35, 80 }, (PokeType.Ghost, PokeType.Poison), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(93, "ゴースト", "", new uint[] { 45, 50, 45, 115, 55, 95 }, (PokeType.Ghost, PokeType.Poison), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(94, "ゲンガー", "", new uint[] { 60, 65, 60, 130, 75, 110 }, (PokeType.Ghost, PokeType.Poison), new string[] { "のろわれボディ", "のろわれボディ", "のろわれボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(95, "イワーク", "", new uint[] { 35, 45, 160, 30, 45, 70 }, (PokeType.Rock, PokeType.Ground), new string[] { "いしあたま", "がんじょう", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(96, "スリープ", "", new uint[] { 60, 48, 45, 43, 90, 42 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふみん", "よちむ", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(97, "スリーパー", "", new uint[] { 85, 73, 70, 73, 115, 67 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふみん", "よちむ", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(98, "クラブ", "", new uint[] { 30, 105, 90, 25, 25, 50 }, (PokeType.Water, PokeType.Non), new string[] { "かいりきバサミ", "シェルアーマー", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(99, "キングラー", "", new uint[] { 55, 130, 115, 50, 50, 75 }, (PokeType.Water, PokeType.Non), new string[] { "かいりきバサミ", "シェルアーマー", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(100, "ビリリダマ", "", new uint[] { 40, 30, 50, 55, 55, 100 }, (PokeType.Electric, PokeType.Non), new string[] { "ぼうおん", "せいでんき", "ゆうばく" }, GenderRatio.Genderless));
            dexData.Add(new Species(101, "マルマイン", "", new uint[] { 60, 50, 70, 80, 80, 150 }, (PokeType.Electric, PokeType.Non), new string[] { "ぼうおん", "せいでんき", "ゆうばく" }, GenderRatio.Genderless));
            dexData.Add(new Species(102, "タマタマ", "", new uint[] { 60, 40, 80, 60, 45, 40 }, (PokeType.Grass, PokeType.Psychic), new string[] { "ようりょくそ", "ようりょくそ", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(103, "ナッシー", "", new uint[] { 95, 95, 85, 125, 75, 55 }, (PokeType.Grass, PokeType.Psychic), new string[] { "ようりょくそ", "ようりょくそ", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(103, "ナッシー", "アローラ", new uint[] { 95, 105, 85, 125, 75, 45 }, (PokeType.Grass, PokeType.Dragon), new string[] { "おみとおし", "おみとおし", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(104, "カラカラ", "", new uint[] { 50, 50, 95, 40, 50, 35 }, (PokeType.Ground, PokeType.Non), new string[] { "いしあたま", "ひらいしん", "カブトアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(105, "ガラガラ", "", new uint[] { 60, 80, 110, 50, 80, 45 }, (PokeType.Ground, PokeType.Non), new string[] { "いしあたま", "ひらいしん", "カブトアーマー" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(105, "ガラガラ", "アローラ", new uint[] { 60, 80, 110, 50, 80, 45 }, (PokeType.Fire, PokeType.Ghost), new string[] { "のろわれボディ", "ひらいしん", "いしあたま" }, GenderRatio.M1F1));
            dexData.Add(new Species(106, "サワムラー", "", new uint[] { 50, 120, 53, 35, 110, 87 }, (PokeType.Fighting, PokeType.Non), new string[] { "じゅうなん", "すてみ", "かるわざ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(107, "エビワラー", "", new uint[] { 50, 105, 79, 35, 110, 76 }, (PokeType.Fighting, PokeType.Non), new string[] { "するどいめ", "てつのこぶし", "せいしんりょく" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(108, "ベロリンガ", "", new uint[] { 90, 55, 75, 60, 75, 30 }, (PokeType.Normal, PokeType.Non), new string[] { "マイペース", "どんかん", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(109, "ドガース", "", new uint[] { 40, 65, 95, 60, 45, 35 }, (PokeType.Poison, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(110, "マタドガス", "", new uint[] { 65, 90, 120, 85, 70, 60 }, (PokeType.Poison, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(111, "サイホーン", "", new uint[] { 80, 85, 95, 30, 30, 25 }, (PokeType.Ground, PokeType.Rock), new string[] { "ひらいしん", "いしあたま", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(112, "サイドン", "", new uint[] { 105, 130, 120, 45, 45, 40 }, (PokeType.Ground, PokeType.Rock), new string[] { "ひらいしん", "いしあたま", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(113, "ラッキー", "", new uint[] { 250, 5, 5, 35, 105, 50 }, (PokeType.Normal, PokeType.Non), new string[] { "しぜんかいふく", "てんのめぐみ", "いやしのこころ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(114, "モンジャラ", "", new uint[] { 65, 55, 115, 100, 40, 60 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "リーフガード", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(115, "ガルーラ", "", new uint[] { 105, 95, 80, 40, 80, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "はやおき", "きもったま", "せいしんりょく" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(116, "タッツー", "", new uint[] { 30, 40, 70, 70, 25, 60 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "スナイパー", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(117, "シードラ", "", new uint[] { 55, 65, 95, 95, 45, 85 }, (PokeType.Water, PokeType.Non), new string[] { "どくのトゲ", "スナイパー", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(118, "トサキント", "", new uint[] { 45, 67, 60, 35, 50, 63 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "みずのベール", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(119, "アズマオウ", "", new uint[] { 80, 92, 65, 65, 80, 68 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "みずのベール", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(120, "ヒトデマン", "", new uint[] { 30, 45, 55, 70, 55, 85 }, (PokeType.Water, PokeType.Non), new string[] { "はっこう", "しぜんかいふく", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(121, "スターミー", "", new uint[] { 60, 75, 85, 100, 85, 115 }, (PokeType.Water, PokeType.Psychic), new string[] { "はっこう", "しぜんかいふく", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(122, "バリヤード", "", new uint[] { 40, 45, 65, 100, 120, 90 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "ぼうおん", "フィルター", "テクニシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(123, "ストライク", "", new uint[] { 70, 110, 80, 55, 80, 105 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "テクニシャン", "ふくつのこころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(124, "ルージュラ", "", new uint[] { 65, 50, 35, 115, 95, 95 }, (PokeType.Ice, PokeType.Psychic), new string[] { "どんかん", "よちむ", "かんそうはだ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(125, "エレブー", "", new uint[] { 65, 83, 57, 95, 85, 105 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(126, "ブーバー", "", new uint[] { 65, 95, 57, 100, 85, 93 }, (PokeType.Fire, PokeType.Non), new string[] { "ほのおのからだ", "ほのおのからだ", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(127, "カイロス", "", new uint[] { 65, 125, 100, 55, 70, 85 }, (PokeType.Bug, PokeType.Non), new string[] { "かいりきバサミ", "かたやぶり", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(128, "ケンタロス", "", new uint[] { 75, 100, 95, 40, 70, 110 }, (PokeType.Normal, PokeType.Non), new string[] { "いかく", "いかりのつぼ", "ちからずく" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(129, "コイキング", "", new uint[] { 20, 10, 55, 15, 20, 80 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(130, "ギャラドス", "", new uint[] { 95, 125, 79, 60, 100, 81 }, (PokeType.Water, PokeType.Flying), new string[] { "いかく", "いかく", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(131, "ラプラス", "", new uint[] { 130, 85, 80, 85, 95, 60 }, (PokeType.Water, PokeType.Ice), new string[] { "ちょすい", "シェルアーマー", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(132, "メタモン", "", new uint[] { 48, 48, 48, 48, 48, 48 }, (PokeType.Normal, PokeType.Non), new string[] { "じゅうなん", "じゅうなん", "かわりもの" }, GenderRatio.Genderless));
            dexData.Add(new Species(133, "イーブイ", "", new uint[] { 55, 55, 50, 45, 65, 55 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "てきおうりょく", "きけんよち" }, GenderRatio.M7F1));
            dexData.Add(new Species(134, "シャワーズ", "", new uint[] { 130, 65, 60, 110, 95, 65 }, (PokeType.Water, PokeType.Non), new string[] { "ちょすい", "ちょすい", "うるおいボディ" }, GenderRatio.M7F1));
            dexData.Add(new Species(135, "サンダース", "", new uint[] { 65, 65, 60, 110, 95, 130 }, (PokeType.Electric, PokeType.Non), new string[] { "ちくでん", "ちくでん", "はやあし" }, GenderRatio.M7F1));
            dexData.Add(new Species(136, "ブースター", "", new uint[] { 65, 130, 60, 95, 110, 65 }, (PokeType.Fire, PokeType.Non), new string[] { "もらいび", "もらいび", "こんじょう" }, GenderRatio.M7F1));
            dexData.Add(new Species(137, "ポリゴン", "", new uint[] { 65, 60, 70, 85, 75, 40 }, (PokeType.Normal, PokeType.Non), new string[] { "トレース", "ダウンロード", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(138, "オムナイト", "", new uint[] { 35, 40, 100, 90, 55, 35 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "シェルアーマー", "くだけるよろい" }, GenderRatio.M7F1));
            dexData.Add(new Species(139, "オムスター", "", new uint[] { 70, 60, 125, 115, 70, 55 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "シェルアーマー", "くだけるよろい" }, GenderRatio.M7F1));
            dexData.Add(new Species(140, "カブト", "", new uint[] { 30, 80, 90, 55, 45, 55 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "カブトアーマー", "くだけるよろい" }, GenderRatio.M7F1));
            dexData.Add(new Species(141, "カブトプス", "", new uint[] { 60, 115, 105, 65, 70, 80 }, (PokeType.Rock, PokeType.Water), new string[] { "すいすい", "カブトアーマー", "くだけるよろい" }, GenderRatio.M7F1));
            dexData.Add(new Species(142, "プテラ", "", new uint[] { 80, 105, 65, 60, 75, 130 }, (PokeType.Rock, PokeType.Flying), new string[] { "いしあたま", "プレッシャー", "きんちょうかん" }, GenderRatio.M7F1));
            dexData.Add(new Species(143, "カビゴン", "", new uint[] { 160, 110, 65, 65, 110, 30 }, (PokeType.Normal, PokeType.Non), new string[] { "めんえき", "あついしぼう", "くいしんぼう" }, GenderRatio.M7F1));
            dexData.Add(new Species(144, "フリーザー", "", new uint[] { 90, 85, 100, 95, 125, 85 }, (PokeType.Ice, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "ゆきがくれ" }, GenderRatio.Genderless));
            dexData.Add(new Species(145, "サンダー", "", new uint[] { 90, 90, 85, 125, 90, 100 }, (PokeType.Electric, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "せいでんき" }, GenderRatio.Genderless));
            dexData.Add(new Species(146, "ファイヤー", "", new uint[] { 90, 100, 90, 125, 85, 90 }, (PokeType.Fire, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "ほのおのからだ" }, GenderRatio.Genderless));
            dexData.Add(new Species(147, "ミニリュウ", "", new uint[] { 41, 64, 45, 50, 50, 50 }, (PokeType.Dragon, PokeType.Non), new string[] { "だっぴ", "だっぴ", "ふしぎなうろこ" }, GenderRatio.M1F1));
            dexData.Add(new Species(148, "ハクリュー", "", new uint[] { 61, 84, 65, 70, 70, 70 }, (PokeType.Dragon, PokeType.Non), new string[] { "だっぴ", "だっぴ", "ふしぎなうろこ" }, GenderRatio.M1F1));
            dexData.Add(new Species(149, "カイリュー", "", new uint[] { 91, 134, 95, 100, 100, 80 }, (PokeType.Dragon, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく", "マルチスケイル" }, GenderRatio.M1F1));
            dexData.Add(new Species(150, "ミュウツー", "", new uint[] { 106, 110, 90, 154, 90, 130 }, (PokeType.Psychic, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "きんちょうかん" }, GenderRatio.Genderless));
            dexData.Add(new Species(151, "ミュウ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Psychic, PokeType.Non), new string[] { "シンクロ", "シンクロ", "シンクロ" }, GenderRatio.Genderless));
            dexData.Add(new Species(152, "チコリータ", "", new uint[] { 45, 49, 65, 49, 65, 45 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "リーフガード" }, GenderRatio.M7F1));
            dexData.Add(new Species(153, "ベイリーフ", "", new uint[] { 60, 62, 80, 63, 80, 60 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "リーフガード" }, GenderRatio.M7F1));
            dexData.Add(new Species(154, "メガニウム", "", new uint[] { 80, 82, 100, 83, 100, 80 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "リーフガード" }, GenderRatio.M7F1));
            dexData.Add(new Species(155, "ヒノアラシ", "", new uint[] { 39, 52, 43, 60, 50, 65 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "もらいび" }, GenderRatio.M7F1));
            dexData.Add(new Species(156, "マグマラシ", "", new uint[] { 58, 64, 58, 80, 65, 80 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "もらいび" }, GenderRatio.M7F1));
            dexData.Add(new Species(157, "バクフーン", "", new uint[] { 78, 84, 78, 109, 85, 100 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "もらいび" }, GenderRatio.M7F1));
            dexData.Add(new Species(158, "ワニノコ", "", new uint[] { 50, 65, 64, 44, 48, 43 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "ちからずく" }, GenderRatio.M7F1));
            dexData.Add(new Species(159, "アリゲイツ", "", new uint[] { 65, 80, 80, 59, 63, 58 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "ちからずく" }, GenderRatio.M7F1));
            dexData.Add(new Species(160, "オーダイル", "", new uint[] { 85, 105, 100, 79, 83, 78 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "ちからずく" }, GenderRatio.M7F1));
            dexData.Add(new Species(161, "オタチ", "", new uint[] { 35, 46, 34, 35, 45, 20 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "するどいめ", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(162, "オオタチ", "", new uint[] { 85, 76, 64, 45, 55, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "するどいめ", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(163, "ホーホー", "", new uint[] { 60, 30, 30, 36, 56, 50 }, (PokeType.Normal, PokeType.Flying), new string[] { "ふみん", "するどいめ", "いろめがね" }, GenderRatio.M1F1));
            dexData.Add(new Species(164, "ヨルノズク", "", new uint[] { 100, 50, 50, 86, 96, 70 }, (PokeType.Normal, PokeType.Flying), new string[] { "ふみん", "するどいめ", "いろめがね" }, GenderRatio.M1F1));
            dexData.Add(new Species(165, "レディバ", "", new uint[] { 40, 20, 30, 40, 80, 55 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "はやおき", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(166, "レディアン", "", new uint[] { 55, 35, 50, 55, 110, 85 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "はやおき", "てつのこぶし" }, GenderRatio.M1F1));
            dexData.Add(new Species(167, "イトマル", "", new uint[] { 40, 60, 40, 40, 40, 30 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "ふみん", "スナイパー" }, GenderRatio.M1F1));
            dexData.Add(new Species(168, "アリアドス", "", new uint[] { 70, 90, 70, 60, 70, 40 }, (PokeType.Bug, PokeType.Poison), new string[] { "むしのしらせ", "ふみん", "スナイパー" }, GenderRatio.M1F1));
            dexData.Add(new Species(169, "クロバット", "", new uint[] { 85, 90, 80, 70, 80, 130 }, (PokeType.Poison, PokeType.Flying), new string[] { "せいしんりょく", "せいしんりょく", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(170, "チョンチー", "", new uint[] { 75, 38, 38, 56, 56, 67 }, (PokeType.Water, PokeType.Electric), new string[] { "ちくでん", "はっこう", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(171, "ランターン", "", new uint[] { 125, 58, 58, 76, 76, 67 }, (PokeType.Water, PokeType.Electric), new string[] { "ちくでん", "はっこう", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(172, "ピチュー", "", new uint[] { 20, 40, 15, 35, 35, 60 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(173, "ピィ", "", new uint[] { 50, 25, 28, 45, 55, 15 }, (PokeType.Fairy, PokeType.Non), new string[] { "メロメロボディ", "マジックガード", "フレンドガード" }, GenderRatio.M1F3));
            dexData.Add(new Species(174, "ププリン", "", new uint[] { 90, 30, 15, 40, 20, 15 }, (PokeType.Normal, PokeType.Fairy), new string[] { "メロメロボディ", "かちき", "フレンドガード" }, GenderRatio.M1F3));
            dexData.Add(new Species(175, "トゲピー", "", new uint[] { 35, 20, 65, 40, 65, 20 }, (PokeType.Fairy, PokeType.Non), new string[] { "はりきり", "てんのめぐみ", "きょううん" }, GenderRatio.M7F1));
            dexData.Add(new Species(176, "トゲチック", "", new uint[] { 55, 40, 85, 80, 105, 40 }, (PokeType.Fairy, PokeType.Flying), new string[] { "はりきり", "てんのめぐみ", "きょううん" }, GenderRatio.M7F1));
            dexData.Add(new Species(177, "ネイティ", "", new uint[] { 40, 50, 45, 70, 45, 70 }, (PokeType.Psychic, PokeType.Flying), new string[] { "シンクロ", "はやおき", "マジックミラー" }, GenderRatio.M1F1));
            dexData.Add(new Species(178, "ネイティオ", "", new uint[] { 65, 75, 70, 95, 70, 95 }, (PokeType.Psychic, PokeType.Flying), new string[] { "シンクロ", "はやおき", "マジックミラー" }, GenderRatio.M1F1));
            dexData.Add(new Species(179, "メリープ", "", new uint[] { 55, 40, 40, 65, 45, 35 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "プラス" }, GenderRatio.M1F1));
            dexData.Add(new Species(180, "モココ", "", new uint[] { 70, 55, 55, 80, 60, 45 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "プラス" }, GenderRatio.M1F1));
            dexData.Add(new Species(181, "デンリュウ", "", new uint[] { 90, 75, 85, 115, 90, 55 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "プラス" }, GenderRatio.M1F1));
            dexData.Add(new Species(182, "キレイハナ", "", new uint[] { 75, 80, 95, 90, 100, 50 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "ようりょくそ", "いやしのこころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(183, "マリル", "", new uint[] { 70, 20, 50, 20, 50, 40 }, (PokeType.Water, PokeType.Fairy), new string[] { "あついしぼう", "ちからもち", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(184, "マリルリ", "", new uint[] { 100, 50, 80, 60, 80, 50 }, (PokeType.Water, PokeType.Fairy), new string[] { "あついしぼう", "ちからもち", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(185, "ウソッキー", "", new uint[] { 70, 100, 115, 30, 65, 30 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "いしあたま", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(186, "ニョロトノ", "", new uint[] { 90, 75, 75, 90, 100, 70 }, (PokeType.Water, PokeType.Non), new string[] { "ちょすい", "しめりけ", "あめふらし" }, GenderRatio.M1F1));
            dexData.Add(new Species(187, "ハネッコ", "", new uint[] { 35, 35, 40, 35, 55, 50 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "リーフガード", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(188, "ポポッコ", "", new uint[] { 55, 45, 50, 45, 65, 80 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "リーフガード", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(189, "ワタッコ", "", new uint[] { 75, 55, 70, 55, 95, 110 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "リーフガード", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(190, "エイパム", "", new uint[] { 55, 70, 55, 40, 55, 85 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "ものひろい", "スキルリンク" }, GenderRatio.M1F1));
            dexData.Add(new Species(191, "ヒマナッツ", "", new uint[] { 30, 30, 30, 30, 30, 30 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "サンパワー", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(192, "キマワリ", "", new uint[] { 75, 75, 55, 105, 85, 30 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "サンパワー", "はやおき" }, GenderRatio.M1F1));
            dexData.Add(new Species(193, "ヤンヤンマ", "", new uint[] { 65, 65, 45, 75, 45, 95 }, (PokeType.Bug, PokeType.Flying), new string[] { "かそく", "ふくがん", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(194, "ウパー", "", new uint[] { 55, 45, 45, 25, 25, 15 }, (PokeType.Water, PokeType.Ground), new string[] { "しめりけ", "ちょすい", "てんねん" }, GenderRatio.M1F1));
            dexData.Add(new Species(195, "ヌオー", "", new uint[] { 95, 85, 85, 65, 65, 35 }, (PokeType.Water, PokeType.Ground), new string[] { "しめりけ", "ちょすい", "てんねん" }, GenderRatio.M1F1));
            dexData.Add(new Species(196, "エーフィ", "", new uint[] { 65, 65, 60, 130, 95, 110 }, (PokeType.Psychic, PokeType.Non), new string[] { "シンクロ", "シンクロ", "マジックミラー" }, GenderRatio.M7F1));
            dexData.Add(new Species(197, "ブラッキー", "", new uint[] { 95, 65, 110, 60, 130, 65 }, (PokeType.Dark, PokeType.Non), new string[] { "シンクロ", "シンクロ", "せいしんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(198, "ヤミカラス", "", new uint[] { 60, 85, 42, 85, 42, 91 }, (PokeType.Dark, PokeType.Flying), new string[] { "ふみん", "きょううん", "いたずらごころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(199, "ヤドキング", "", new uint[] { 95, 75, 80, 100, 110, 30 }, (PokeType.Water, PokeType.Psychic), new string[] { "どんかん", "マイペース", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(200, "ムウマ", "", new uint[] { 60, 60, 60, 85, 85, 85 }, (PokeType.Ghost, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(201, "アンノーン", "", new uint[] { 48, 72, 48, 72, 48, 48 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(202, "ソーナンス", "", new uint[] { 190, 33, 58, 33, 58, 33 }, (PokeType.Psychic, PokeType.Non), new string[] { "かげふみ", "かげふみ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(203, "キリンリキ", "", new uint[] { 70, 80, 65, 90, 65, 85 }, (PokeType.Normal, PokeType.Psychic), new string[] { "せいしんりょく", "はやおき", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(204, "クヌギダマ", "", new uint[] { 50, 65, 90, 35, 35, 15 }, (PokeType.Bug, PokeType.Non), new string[] { "がんじょう", "がんじょう", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(205, "フォレトス", "", new uint[] { 75, 90, 140, 60, 60, 40 }, (PokeType.Bug, PokeType.Steel), new string[] { "がんじょう", "がんじょう", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(206, "ノコッチ", "", new uint[] { 100, 70, 70, 65, 65, 45 }, (PokeType.Normal, PokeType.Non), new string[] { "てんのめぐみ", "にげあし", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(207, "グライガー", "", new uint[] { 65, 75, 105, 35, 65, 85 }, (PokeType.Ground, PokeType.Flying), new string[] { "かいりきバサミ", "すながくれ", "めんえき" }, GenderRatio.M1F1));
            dexData.Add(new Species(208, "ハガネール", "", new uint[] { 75, 85, 200, 55, 65, 30 }, (PokeType.Steel, PokeType.Ground), new string[] { "いしあたま", "がんじょう", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(209, "ブルー", "", new uint[] { 60, 80, 50, 40, 40, 30 }, (PokeType.Fairy, PokeType.Non), new string[] { "いかく", "にげあし", "びびり" }, GenderRatio.M1F3));
            dexData.Add(new Species(210, "グランブル", "", new uint[] { 90, 120, 75, 60, 60, 45 }, (PokeType.Fairy, PokeType.Non), new string[] { "いかく", "にげあし", "びびり" }, GenderRatio.M1F3));
            dexData.Add(new Species(211, "ハリーセン", "", new uint[] { 65, 95, 85, 55, 55, 85 }, (PokeType.Water, PokeType.Poison), new string[] { "どくのトゲ", "すいすい", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(212, "ハッサム", "", new uint[] { 70, 130, 100, 55, 80, 65 }, (PokeType.Bug, PokeType.Steel), new string[] { "むしのしらせ", "テクニシャン", "ライトメタル" }, GenderRatio.M1F1));
            dexData.Add(new Species(213, "ツボツボ", "", new uint[] { 20, 10, 230, 10, 230, 5 }, (PokeType.Bug, PokeType.Rock), new string[] { "がんじょう", "くいしんぼう", "あまのじゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(214, "ヘラクロス", "", new uint[] { 80, 125, 75, 40, 95, 85 }, (PokeType.Bug, PokeType.Fighting), new string[] { "むしのしらせ", "こんじょう", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(215, "ニューラ", "", new uint[] { 55, 95, 55, 35, 75, 115 }, (PokeType.Dark, PokeType.Ice), new string[] { "せいしんりょく", "するどいめ", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(216, "ヒメグマ", "", new uint[] { 60, 80, 50, 50, 50, 40 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "はやあし", "みつあつめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(217, "リングマ", "", new uint[] { 90, 130, 75, 75, 75, 55 }, (PokeType.Normal, PokeType.Non), new string[] { "こんじょう", "はやあし", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(218, "マグマッグ", "", new uint[] { 40, 40, 40, 70, 40, 20 }, (PokeType.Fire, PokeType.Non), new string[] { "マグマのよろい", "ほのおのからだ", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(219, "マグカルゴ", "", new uint[] { 60, 50, 120, 90, 80, 30 }, (PokeType.Fire, PokeType.Rock), new string[] { "マグマのよろい", "ほのおのからだ", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(220, "ウリムー", "", new uint[] { 50, 50, 40, 30, 30, 50 }, (PokeType.Ice, PokeType.Ground), new string[] { "どんかん", "ゆきがくれ", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(221, "イノムー", "", new uint[] { 100, 100, 80, 60, 60, 50 }, (PokeType.Ice, PokeType.Ground), new string[] { "どんかん", "ゆきがくれ", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(222, "サニーゴ", "", new uint[] { 65, 55, 95, 65, 95, 35 }, (PokeType.Water, PokeType.Rock), new string[] { "はりきり", "しぜんかいふく", "さいせいりょく" }, GenderRatio.M1F3));
            dexData.Add(new Species(223, "テッポウオ", "", new uint[] { 35, 65, 35, 65, 35, 65 }, (PokeType.Water, PokeType.Non), new string[] { "はりきり", "スナイパー", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(224, "オクタン", "", new uint[] { 75, 105, 75, 105, 75, 45 }, (PokeType.Water, PokeType.Non), new string[] { "きゅうばん", "スナイパー", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(225, "デリバード", "", new uint[] { 45, 55, 45, 65, 45, 75 }, (PokeType.Ice, PokeType.Flying), new string[] { "やるき", "はりきり", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new Species(226, "マンタイン", "", new uint[] { 85, 40, 70, 80, 140, 70 }, (PokeType.Water, PokeType.Flying), new string[] { "すいすい", "ちょすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(227, "エアームド", "", new uint[] { 65, 80, 140, 40, 70, 70 }, (PokeType.Steel, PokeType.Flying), new string[] { "するどいめ", "がんじょう", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(228, "デルビル", "", new uint[] { 45, 60, 30, 80, 50, 65 }, (PokeType.Dark, PokeType.Fire), new string[] { "はやおき", "もらいび", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(229, "ヘルガー", "", new uint[] { 75, 90, 50, 110, 80, 95 }, (PokeType.Dark, PokeType.Fire), new string[] { "はやおき", "もらいび", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(230, "キングドラ", "", new uint[] { 75, 95, 95, 95, 95, 85 }, (PokeType.Water, PokeType.Dragon), new string[] { "すいすい", "スナイパー", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(231, "ゴマゾウ", "", new uint[] { 90, 60, 60, 40, 40, 40 }, (PokeType.Ground, PokeType.Non), new string[] { "ものひろい", "ものひろい", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(232, "ドンファン", "", new uint[] { 90, 120, 120, 60, 60, 50 }, (PokeType.Ground, PokeType.Non), new string[] { "ものひろい", "ものひろい", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(233, "ポリゴン2", "", new uint[] { 85, 80, 90, 105, 95, 60 }, (PokeType.Normal, PokeType.Non), new string[] { "トレース", "ダウンロード", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(234, "オドシシ", "", new uint[] { 73, 95, 62, 85, 65, 85 }, (PokeType.Normal, PokeType.Non), new string[] { "いかく", "おみとおし", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(235, "ドーブル", "", new uint[] { 55, 20, 35, 20, 45, 75 }, (PokeType.Normal, PokeType.Non), new string[] { "マイペース", "テクニシャン", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(236, "バルキー", "", new uint[] { 35, 35, 35, 35, 35, 35 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ふくつのこころ", "やるき" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(237, "カポエラー", "", new uint[] { 50, 95, 95, 35, 110, 70 }, (PokeType.Fighting, PokeType.Non), new string[] { "いかく", "テクニシャン", "ふくつのこころ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(238, "ムチュール", "", new uint[] { 45, 30, 15, 85, 65, 65 }, (PokeType.Ice, PokeType.Psychic), new string[] { "どんかん", "よちむ", "うるおいボディ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(239, "エレキッド", "", new uint[] { 45, 63, 37, 65, 55, 95 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "せいでんき", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(240, "ブビィ", "", new uint[] { 45, 75, 37, 70, 55, 83 }, (PokeType.Fire, PokeType.Non), new string[] { "ほのおのからだ", "ほのおのからだ", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(241, "ミルタンク", "", new uint[] { 95, 80, 105, 40, 70, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "あついしぼう", "きもったま", "そうしょく" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(242, "ハピナス", "", new uint[] { 255, 10, 10, 75, 135, 55 }, (PokeType.Normal, PokeType.Non), new string[] { "しぜんかいふく", "てんのめぐみ", "いやしのこころ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(243, "ライコウ", "", new uint[] { 90, 85, 75, 115, 100, 115 }, (PokeType.Electric, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "せいしんりょく" }, GenderRatio.Genderless));
            dexData.Add(new Species(244, "エンテイ", "", new uint[] { 115, 115, 85, 90, 75, 100 }, (PokeType.Fire, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "せいしんりょく" }, GenderRatio.Genderless));
            dexData.Add(new Species(245, "スイクン", "", new uint[] { 100, 75, 115, 90, 115, 85 }, (PokeType.Water, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "せいしんりょく" }, GenderRatio.Genderless));
            dexData.Add(new Species(246, "ヨーギラス", "", new uint[] { 50, 64, 50, 45, 50, 41 }, (PokeType.Rock, PokeType.Ground), new string[] { "こんじょう", "こんじょう", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(247, "サナギラス", "", new uint[] { 70, 84, 70, 65, 70, 51 }, (PokeType.Rock, PokeType.Ground), new string[] { "だっぴ", "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(248, "バンギラス", "", new uint[] { 100, 134, 110, 95, 100, 61 }, (PokeType.Rock, PokeType.Dark), new string[] { "すなおこし", "すなおこし", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(249, "ルギア", "", new uint[] { 106, 90, 130, 90, 154, 110 }, (PokeType.Psychic, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "マルチスケイル" }, GenderRatio.Genderless));
            dexData.Add(new Species(250, "ホウオウ", "", new uint[] { 106, 130, 90, 110, 154, 90 }, (PokeType.Fire, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "さいせいりょく" }, GenderRatio.Genderless));
            dexData.Add(new Species(251, "セレビィ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Psychic, PokeType.Grass), new string[] { "しぜんかいふく", "しぜんかいふく", "しぜんかいふく" }, GenderRatio.Genderless));
            dexData.Add(new Species(252, "キモリ", "", new uint[] { 40, 45, 35, 65, 55, 70 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "かるわざ" }, GenderRatio.M7F1));
            dexData.Add(new Species(253, "ジュプトル", "", new uint[] { 50, 65, 45, 85, 65, 95 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "かるわざ" }, GenderRatio.M7F1));
            dexData.Add(new Species(254, "ジュカイン", "", new uint[] { 70, 85, 65, 105, 85, 120 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "かるわざ" }, GenderRatio.M7F1));
            dexData.Add(new Species(255, "アチャモ", "", new uint[] { 45, 60, 40, 70, 50, 45 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "かそく" }, GenderRatio.M7F1));
            dexData.Add(new Species(256, "ワカシャモ", "", new uint[] { 60, 85, 60, 85, 60, 55 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "かそく" }, GenderRatio.M7F1));
            dexData.Add(new Species(257, "バシャーモ", "", new uint[] { 80, 120, 70, 110, 70, 80 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "かそく" }, GenderRatio.M7F1));
            dexData.Add(new Species(258, "ミズゴロウ", "", new uint[] { 50, 70, 50, 50, 50, 40 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "しめりけ" }, GenderRatio.M7F1));
            dexData.Add(new Species(259, "ヌマクロー", "", new uint[] { 70, 85, 70, 60, 70, 50 }, (PokeType.Water, PokeType.Ground), new string[] { "げきりゅう", "げきりゅう", "しめりけ" }, GenderRatio.M7F1));
            dexData.Add(new Species(260, "ラグラージ", "", new uint[] { 100, 110, 90, 85, 90, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "げきりゅう", "げきりゅう", "しめりけ" }, GenderRatio.M7F1));
            dexData.Add(new Species(261, "ポチエナ", "", new uint[] { 35, 55, 35, 30, 30, 35 }, (PokeType.Dark, PokeType.Non), new string[] { "にげあし", "はやあし", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(262, "グラエナ", "", new uint[] { 70, 90, 70, 60, 60, 70 }, (PokeType.Dark, PokeType.Non), new string[] { "いかく", "はやあし", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(263, "ジグザグマ", "", new uint[] { 38, 30, 41, 30, 41, 60 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "くいしんぼう", "はやあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(264, "マッスグマ", "", new uint[] { 78, 70, 61, 50, 61, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "くいしんぼう", "はやあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(265, "ケムッソ", "", new uint[] { 45, 45, 35, 20, 30, 20 }, (PokeType.Bug, PokeType.Non), new string[] { "りんぷん", "りんぷん", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(266, "カラサリス", "", new uint[] { 50, 35, 55, 25, 25, 15 }, (PokeType.Bug, PokeType.Non), new string[] { "だっぴ", "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(267, "アゲハント", "", new uint[] { 60, 70, 50, 100, 50, 65 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "むしのしらせ", "とうそうしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(268, "マユルド", "", new uint[] { 50, 35, 55, 25, 25, 15 }, (PokeType.Bug, PokeType.Non), new string[] { "だっぴ", "だっぴ", "だっぴ" }, GenderRatio.M1F1));
            dexData.Add(new Species(269, "ドクケイル", "", new uint[] { 60, 50, 70, 50, 90, 65 }, (PokeType.Bug, PokeType.Poison), new string[] { "りんぷん", "りんぷん", "ふくがん" }, GenderRatio.M1F1));
            dexData.Add(new Species(270, "ハスボー", "", new uint[] { 40, 30, 30, 40, 50, 30 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(271, "ハスブレロ", "", new uint[] { 60, 50, 50, 60, 70, 50 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(272, "ルンパッパ", "", new uint[] { 80, 70, 70, 90, 100, 70 }, (PokeType.Water, PokeType.Grass), new string[] { "すいすい", "あめうけざら", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(273, "タネボー", "", new uint[] { 40, 40, 50, 30, 30, 30 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "はやおき", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(274, "コノハナ", "", new uint[] { 70, 70, 40, 60, 40, 60 }, (PokeType.Grass, PokeType.Dark), new string[] { "ようりょくそ", "はやおき", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(275, "ダーテング", "", new uint[] { 90, 100, 60, 90, 60, 80 }, (PokeType.Grass, PokeType.Dark), new string[] { "ようりょくそ", "はやおき", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(276, "スバメ", "", new uint[] { 40, 55, 30, 30, 30, 85 }, (PokeType.Normal, PokeType.Flying), new string[] { "こんじょう", "こんじょう", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(277, "オオスバメ", "", new uint[] { 60, 85, 60, 75, 50, 125 }, (PokeType.Normal, PokeType.Flying), new string[] { "こんじょう", "こんじょう", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(278, "キャモメ", "", new uint[] { 40, 30, 30, 55, 30, 85 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "うるおいボディ", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(279, "ペリッパー", "", new uint[] { 60, 50, 100, 95, 70, 65 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "あめふらし", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(280, "ラルトス", "", new uint[] { 28, 25, 25, 45, 35, 40 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "シンクロ", "トレース", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(281, "キルリア", "", new uint[] { 38, 35, 35, 65, 55, 50 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "シンクロ", "トレース", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(282, "サーナイト", "", new uint[] { 68, 65, 65, 125, 115, 80 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "シンクロ", "トレース", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(283, "アメタマ", "", new uint[] { 40, 30, 32, 50, 52, 65 }, (PokeType.Bug, PokeType.Water), new string[] { "すいすい", "すいすい", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(284, "アメモース", "", new uint[] { 70, 60, 62, 100, 82, 80 }, (PokeType.Bug, PokeType.Flying), new string[] { "いかく", "いかく", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(285, "キノココ", "", new uint[] { 60, 40, 60, 40, 60, 35 }, (PokeType.Grass, PokeType.Non), new string[] { "ほうし", "ポイズンヒール", "はやあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(286, "キノガッサ", "", new uint[] { 60, 130, 80, 60, 60, 70 }, (PokeType.Grass, PokeType.Fighting), new string[] { "ほうし", "ポイズンヒール", "テクニシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(287, "ナマケロ", "", new uint[] { 60, 60, 60, 35, 35, 30 }, (PokeType.Normal, PokeType.Non), new string[] { "なまけ", "なまけ", "なまけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(288, "ヤルキモノ", "", new uint[] { 80, 80, 80, 55, 55, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "やるき", "やるき", "やるき" }, GenderRatio.M1F1));
            dexData.Add(new Species(289, "ケッキング", "", new uint[] { 150, 160, 100, 95, 65, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "なまけ", "なまけ", "なまけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(290, "ツチニン", "", new uint[] { 31, 45, 90, 30, 30, 40 }, (PokeType.Bug, PokeType.Ground), new string[] { "ふくがん", "ふくがん", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(291, "テッカニン", "", new uint[] { 61, 90, 45, 50, 50, 160 }, (PokeType.Bug, PokeType.Flying), new string[] { "かそく", "かそく", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(292, "ヌケニン", "", new uint[] { 1, 90, 45, 30, 30, 40 }, (PokeType.Bug, PokeType.Ghost), new string[] { "ふしぎなまもり", "ふしぎなまもり", "ふしぎなまもり" }, GenderRatio.Genderless));
            dexData.Add(new Species(293, "ゴニョニョ", "", new uint[] { 64, 51, 23, 51, 23, 28 }, (PokeType.Normal, PokeType.Non), new string[] { "ぼうおん", "ぼうおん", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(294, "ドゴーム", "", new uint[] { 84, 71, 43, 71, 43, 48 }, (PokeType.Normal, PokeType.Non), new string[] { "ぼうおん", "ぼうおん", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(295, "バクオング", "", new uint[] { 104, 91, 63, 91, 73, 68 }, (PokeType.Normal, PokeType.Non), new string[] { "ぼうおん", "ぼうおん", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(296, "マクノシタ", "", new uint[] { 72, 60, 30, 20, 30, 25 }, (PokeType.Fighting, PokeType.Non), new string[] { "あついしぼう", "こんじょう", "ちからずく" }, GenderRatio.M3F1));
            dexData.Add(new Species(297, "ハリテヤマ", "", new uint[] { 144, 120, 60, 40, 60, 50 }, (PokeType.Fighting, PokeType.Non), new string[] { "あついしぼう", "こんじょう", "ちからずく" }, GenderRatio.M3F1));
            dexData.Add(new Species(298, "ルリリ", "", new uint[] { 50, 20, 40, 20, 40, 20 }, (PokeType.Normal, PokeType.Fairy), new string[] { "あついしぼう", "ちからもち", "そうしょく" }, GenderRatio.M1F3));
            dexData.Add(new Species(299, "ノズパス", "", new uint[] { 30, 45, 135, 45, 90, 30 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "じりょく", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(300, "エネコ", "", new uint[] { 50, 45, 45, 35, 35, 50 }, (PokeType.Normal, PokeType.Non), new string[] { "メロメロボディ", "ノーマルスキン", "ミラクルスキン" }, GenderRatio.M1F3));
            dexData.Add(new Species(301, "エネコロロ", "", new uint[] { 70, 65, 65, 55, 55, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "メロメロボディ", "ノーマルスキン", "ミラクルスキン" }, GenderRatio.M1F3));
            dexData.Add(new Species(302, "ヤミラミ", "", new uint[] { 50, 75, 75, 65, 65, 50 }, (PokeType.Dark, PokeType.Ghost), new string[] { "するどいめ", "あとだし", "いたずらごころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(303, "クチート", "", new uint[] { 50, 85, 85, 55, 55, 50 }, (PokeType.Steel, PokeType.Fairy), new string[] { "かいりきバサミ", "いかく", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(304, "ココドラ", "", new uint[] { 50, 70, 100, 40, 40, 30 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま", "ヘヴィメタル" }, GenderRatio.M1F1));
            dexData.Add(new Species(305, "コドラ", "", new uint[] { 60, 90, 140, 50, 50, 40 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま", "ヘヴィメタル" }, GenderRatio.M1F1));
            dexData.Add(new Species(306, "ボスゴドラ", "", new uint[] { 70, 110, 180, 60, 60, 50 }, (PokeType.Steel, PokeType.Rock), new string[] { "がんじょう", "いしあたま", "ヘヴィメタル" }, GenderRatio.M1F1));
            dexData.Add(new Species(307, "アサナン", "", new uint[] { 30, 40, 55, 40, 55, 60 }, (PokeType.Fighting, PokeType.Psychic), new string[] { "ヨガパワー", "ヨガパワー", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(308, "チャーレム", "", new uint[] { 60, 60, 75, 60, 75, 80 }, (PokeType.Fighting, PokeType.Psychic), new string[] { "ヨガパワー", "ヨガパワー", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(309, "ラクライ", "", new uint[] { 40, 45, 40, 65, 40, 65 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "ひらいしん", "マイナス" }, GenderRatio.M1F1));
            dexData.Add(new Species(310, "ライボルト", "", new uint[] { 70, 75, 60, 105, 60, 105 }, (PokeType.Electric, PokeType.Non), new string[] { "せいでんき", "ひらいしん", "マイナス" }, GenderRatio.M1F1));
            dexData.Add(new Species(311, "プラスル", "", new uint[] { 60, 50, 40, 85, 75, 95 }, (PokeType.Electric, PokeType.Non), new string[] { "プラス", "プラス", "ひらいしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(312, "マイナン", "", new uint[] { 60, 40, 50, 75, 85, 95 }, (PokeType.Electric, PokeType.Non), new string[] { "マイナス", "マイナス", "ちくでん" }, GenderRatio.M1F1));
            dexData.Add(new Species(313, "バルビート", "", new uint[] { 65, 73, 75, 47, 85, 85 }, (PokeType.Bug, PokeType.Non), new string[] { "はっこう", "むしのしらせ", "いたずらごころ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(314, "イルミーゼ", "", new uint[] { 65, 47, 75, 73, 85, 85 }, (PokeType.Bug, PokeType.Non), new string[] { "どんかん", "いろめがね", "いたずらごころ" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(315, "ロゼリア", "", new uint[] { 50, 60, 45, 100, 80, 65 }, (PokeType.Grass, PokeType.Poison), new string[] { "しぜんかいふく", "どくのトゲ", "リーフガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(316, "ゴクリン", "", new uint[] { 70, 43, 53, 43, 53, 40 }, (PokeType.Poison, PokeType.Non), new string[] { "ヘドロえき", "ねんちゃく", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(317, "マルノーム", "", new uint[] { 100, 73, 83, 73, 83, 55 }, (PokeType.Poison, PokeType.Non), new string[] { "ヘドロえき", "ねんちゃく", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(318, "キバニア", "", new uint[] { 45, 90, 20, 65, 20, 65 }, (PokeType.Water, PokeType.Dark), new string[] { "さめはだ", "さめはだ", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(319, "サメハダー", "", new uint[] { 70, 120, 40, 95, 40, 95 }, (PokeType.Water, PokeType.Dark), new string[] { "さめはだ", "さめはだ", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(320, "ホエルコ", "", new uint[] { 130, 70, 35, 70, 35, 60 }, (PokeType.Water, PokeType.Non), new string[] { "みずのベール", "どんかん", "プレッシャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(321, "ホエルオー", "", new uint[] { 170, 90, 45, 90, 45, 60 }, (PokeType.Water, PokeType.Non), new string[] { "みずのベール", "どんかん", "プレッシャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(322, "ドンメル", "", new uint[] { 60, 60, 40, 65, 45, 35 }, (PokeType.Fire, PokeType.Ground), new string[] { "どんかん", "たんじゅん", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new Species(323, "バクーダ", "", new uint[] { 70, 100, 70, 105, 75, 40 }, (PokeType.Fire, PokeType.Ground), new string[] { "マグマのよろい", "ハードロック", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new Species(324, "コータス", "", new uint[] { 70, 85, 140, 85, 70, 20 }, (PokeType.Fire, PokeType.Non), new string[] { "しろいけむり", "ひでり", "シェルアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(325, "バネブー", "", new uint[] { 60, 25, 35, 70, 80, 60 }, (PokeType.Psychic, PokeType.Non), new string[] { "あついしぼう", "マイペース", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(326, "ブーピッグ", "", new uint[] { 80, 45, 65, 90, 110, 80 }, (PokeType.Psychic, PokeType.Non), new string[] { "あついしぼう", "マイペース", "くいしんぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(327, "パッチール", "", new uint[] { 60, 60, 60, 60, 60, 60 }, (PokeType.Normal, PokeType.Non), new string[] { "マイペース", "ちどりあし", "あまのじゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(328, "ナックラー", "", new uint[] { 45, 100, 45, 45, 45, 10 }, (PokeType.Ground, PokeType.Non), new string[] { "かいりきバサミ", "ありじごく", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(329, "ビブラーバ", "", new uint[] { 50, 70, 50, 50, 50, 70 }, (PokeType.Ground, PokeType.Dragon), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(330, "フライゴン", "", new uint[] { 80, 100, 80, 80, 80, 100 }, (PokeType.Ground, PokeType.Dragon), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(331, "サボネア", "", new uint[] { 50, 85, 40, 85, 40, 35 }, (PokeType.Grass, PokeType.Non), new string[] { "すながくれ", "すながくれ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(332, "ノクタス", "", new uint[] { 70, 115, 60, 115, 60, 55 }, (PokeType.Grass, PokeType.Dark), new string[] { "すながくれ", "すながくれ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(333, "チルット", "", new uint[] { 45, 40, 60, 40, 75, 50 }, (PokeType.Normal, PokeType.Flying), new string[] { "しぜんかいふく", "しぜんかいふく", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(334, "チルタリス", "", new uint[] { 75, 70, 90, 70, 105, 80 }, (PokeType.Dragon, PokeType.Flying), new string[] { "しぜんかいふく", "しぜんかいふく", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(335, "ザングース", "", new uint[] { 73, 115, 60, 60, 60, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "めんえき", "めんえき", "どくぼうそう" }, GenderRatio.M1F1));
            dexData.Add(new Species(336, "ハブネーク", "", new uint[] { 73, 100, 60, 100, 60, 65 }, (PokeType.Poison, PokeType.Non), new string[] { "だっぴ", "だっぴ", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(337, "ルナトーン", "", new uint[] { 90, 55, 65, 95, 85, 70 }, (PokeType.Rock, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(338, "ソルロック", "", new uint[] { 90, 95, 85, 55, 65, 70 }, (PokeType.Rock, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(339, "ドジョッチ", "", new uint[] { 50, 48, 43, 46, 41, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "どんかん", "きけんよち", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(340, "ナマズン", "", new uint[] { 110, 78, 73, 76, 71, 60 }, (PokeType.Water, PokeType.Ground), new string[] { "どんかん", "きけんよち", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(341, "ヘイガニ", "", new uint[] { 43, 80, 65, 50, 35, 35 }, (PokeType.Water, PokeType.Non), new string[] { "かいりきバサミ", "シェルアーマー", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(342, "シザリガー", "", new uint[] { 63, 120, 85, 90, 55, 55 }, (PokeType.Water, PokeType.Dark), new string[] { "かいりきバサミ", "シェルアーマー", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(343, "ヤジロン", "", new uint[] { 40, 40, 55, 40, 70, 55 }, (PokeType.Ground, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(344, "ネンドール", "", new uint[] { 60, 70, 105, 70, 120, 75 }, (PokeType.Ground, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(345, "リリーラ", "", new uint[] { 66, 41, 77, 61, 87, 23 }, (PokeType.Rock, PokeType.Grass), new string[] { "きゅうばん", "きゅうばん", "よびみず" }, GenderRatio.M7F1));
            dexData.Add(new Species(346, "ユレイドル", "", new uint[] { 86, 81, 97, 81, 107, 43 }, (PokeType.Rock, PokeType.Grass), new string[] { "きゅうばん", "きゅうばん", "よびみず" }, GenderRatio.M7F1));
            dexData.Add(new Species(347, "アノプス", "", new uint[] { 45, 95, 50, 40, 50, 75 }, (PokeType.Rock, PokeType.Bug), new string[] { "カブトアーマー", "カブトアーマー", "すいすい" }, GenderRatio.M7F1));
            dexData.Add(new Species(348, "アーマルド", "", new uint[] { 75, 125, 100, 70, 80, 45 }, (PokeType.Rock, PokeType.Bug), new string[] { "カブトアーマー", "カブトアーマー", "すいすい" }, GenderRatio.M7F1));
            dexData.Add(new Species(349, "ヒンバス", "", new uint[] { 20, 15, 20, 10, 55, 80 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "どんかん", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(350, "ミロカロス", "", new uint[] { 95, 60, 79, 100, 125, 81 }, (PokeType.Water, PokeType.Non), new string[] { "ふしぎなうろこ", "かちき", "メロメロボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(351, "ポワルン", "", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Normal, PokeType.Non), new string[] { "てんきや", "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(351, "ポワルン", "太陽", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Fire, PokeType.Non), new string[] { "てんきや", "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(351, "ポワルン", "雨水", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Water, PokeType.Non), new string[] { "てんきや", "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(351, "ポワルン", "雪雲", new uint[] { 70, 70, 70, 70, 70, 70 }, (PokeType.Ice, PokeType.Non), new string[] { "てんきや", "てんきや", "てんきや" }, GenderRatio.M1F1));
            dexData.Add(new Species(352, "カクレオン", "", new uint[] { 60, 90, 70, 60, 120, 40 }, (PokeType.Normal, PokeType.Non), new string[] { "へんしょく", "へんしょく", "へんげんじざい" }, GenderRatio.M1F1));
            dexData.Add(new Species(353, "カゲボウズ", "", new uint[] { 44, 75, 35, 63, 33, 45 }, (PokeType.Ghost, PokeType.Non), new string[] { "ふみん", "おみとおし", "のろわれボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(354, "ジュペッタ", "", new uint[] { 64, 115, 65, 83, 63, 65 }, (PokeType.Ghost, PokeType.Non), new string[] { "ふみん", "おみとおし", "のろわれボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(355, "ヨマワル", "", new uint[] { 20, 40, 90, 30, 90, 25 }, (PokeType.Ghost, PokeType.Non), new string[] { "ふゆう", "ふゆう", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(356, "サマヨール", "", new uint[] { 40, 70, 130, 60, 130, 25 }, (PokeType.Ghost, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(357, "トロピウス", "", new uint[] { 99, 68, 83, 72, 87, 51 }, (PokeType.Grass, PokeType.Flying), new string[] { "ようりょくそ", "サンパワー", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(358, "チリーン", "", new uint[] { 75, 50, 80, 95, 90, 65 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(359, "アブソル", "", new uint[] { 65, 130, 60, 75, 60, 75 }, (PokeType.Dark, PokeType.Non), new string[] { "プレッシャー", "きょううん", "せいぎのこころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(360, "ソーナノ", "", new uint[] { 95, 23, 48, 23, 48, 23 }, (PokeType.Psychic, PokeType.Non), new string[] { "かげふみ", "かげふみ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(361, "ユキワラシ", "", new uint[] { 50, 50, 50, 50, 50, 50 }, (PokeType.Ice, PokeType.Non), new string[] { "せいしんりょく", "アイスボディ", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(362, "オニゴーリ", "", new uint[] { 80, 80, 80, 80, 80, 80 }, (PokeType.Ice, PokeType.Non), new string[] { "せいしんりょく", "アイスボディ", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(363, "タマザラシ", "", new uint[] { 70, 40, 50, 55, 50, 25 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "アイスボディ", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(364, "トドグラー", "", new uint[] { 90, 60, 70, 75, 70, 45 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "アイスボディ", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(365, "トドゼルガ", "", new uint[] { 110, 80, 90, 95, 90, 65 }, (PokeType.Ice, PokeType.Water), new string[] { "あついしぼう", "アイスボディ", "どんかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(366, "パールル", "", new uint[] { 35, 64, 85, 74, 55, 32 }, (PokeType.Water, PokeType.Non), new string[] { "シェルアーマー", "シェルアーマー", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(367, "ハンテール", "", new uint[] { 55, 104, 105, 94, 75, 52 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(368, "サクラビス", "", new uint[] { 55, 84, 105, 114, 75, 52 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(369, "ジーランス", "", new uint[] { 100, 90, 130, 45, 65, 55 }, (PokeType.Water, PokeType.Rock), new string[] { "すいすい", "いしあたま", "がんじょう" }, GenderRatio.M7F1));
            dexData.Add(new Species(370, "ラブカス", "", new uint[] { 43, 30, 55, 40, 65, 97 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "うるおいボディ" }, GenderRatio.M1F3));
            dexData.Add(new Species(371, "タツベイ", "", new uint[] { 45, 75, 60, 40, 30, 50 }, (PokeType.Dragon, PokeType.Non), new string[] { "いしあたま", "いしあたま", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(372, "コモルー", "", new uint[] { 65, 95, 100, 60, 50, 50 }, (PokeType.Dragon, PokeType.Non), new string[] { "いしあたま", "いしあたま", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(373, "ボーマンダ", "", new uint[] { 95, 135, 80, 110, 80, 100 }, (PokeType.Dragon, PokeType.Flying), new string[] { "いかく", "いかく", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(374, "ダンバル", "", new uint[] { 40, 55, 80, 35, 60, 30 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ", "ライトメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(375, "メタング", "", new uint[] { 60, 75, 100, 55, 80, 50 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ", "ライトメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(376, "メタグロス", "", new uint[] { 80, 135, 130, 95, 90, 70 }, (PokeType.Steel, PokeType.Psychic), new string[] { "クリアボディ", "クリアボディ", "ライトメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(377, "レジロック", "", new uint[] { 80, 100, 200, 50, 100, 50 }, (PokeType.Rock, PokeType.Non), new string[] { "クリアボディ", "クリアボディ", "がんじょう" }, GenderRatio.Genderless));
            dexData.Add(new Species(378, "レジアイス", "", new uint[] { 80, 50, 100, 100, 200, 50 }, (PokeType.Ice, PokeType.Non), new string[] { "クリアボディ", "クリアボディ", "アイスボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(379, "レジスチル", "", new uint[] { 80, 75, 150, 75, 150, 50 }, (PokeType.Steel, PokeType.Non), new string[] { "クリアボディ", "クリアボディ", "ライトメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(380, "ラティアス", "", new uint[] { 80, 80, 90, 110, 130, 110 }, (PokeType.Dragon, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(381, "ラティオス", "", new uint[] { 80, 90, 80, 130, 110, 110 }, (PokeType.Dragon, PokeType.Psychic), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(382, "カイオーガ", "", new uint[] { 100, 100, 90, 150, 140, 90 }, (PokeType.Water, PokeType.Non), new string[] { "あめふらし", "あめふらし", "あめふらし" }, GenderRatio.Genderless));
            dexData.Add(new Species(383, "グラードン", "", new uint[] { 100, 150, 140, 100, 90, 90 }, (PokeType.Ground, PokeType.Non), new string[] { "ひでり", "ひでり", "ひでり" }, GenderRatio.Genderless));
            dexData.Add(new Species(384, "レックウザ", "", new uint[] { 105, 150, 90, 150, 90, 95 }, (PokeType.Dragon, PokeType.Flying), new string[] { "エアロック", "エアロック", "エアロック" }, GenderRatio.Genderless));
            dexData.Add(new Species(385, "ジラーチ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Steel, PokeType.Psychic), new string[] { "てんのめぐみ", "てんのめぐみ", "てんのめぐみ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "ノーマル", new uint[] { 50, 150, 50, 150, 50, 150 }, (PokeType.Psychic, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "アタック", new uint[] { 50, 180, 20, 180, 20, 150 }, (PokeType.Psychic, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "ディフェンス", new uint[] { 50, 70, 160, 70, 160, 90 }, (PokeType.Psychic, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(386, "デオキシス", "スピード", new uint[] { 50, 95, 90, 95, 90, 180 }, (PokeType.Psychic, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new Species(387, "ナエトル", "", new uint[] { 55, 68, 64, 45, 55, 31 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(388, "ハヤシガメ", "", new uint[] { 75, 89, 85, 55, 65, 36 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(389, "ドダイトス", "", new uint[] { 95, 109, 105, 75, 85, 56 }, (PokeType.Grass, PokeType.Ground), new string[] { "しんりょく", "しんりょく", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(390, "ヒコザル", "", new uint[] { 44, 58, 44, 58, 44, 61 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "てつのこぶし" }, GenderRatio.M7F1));
            dexData.Add(new Species(391, "モウカザル", "", new uint[] { 64, 78, 52, 78, 52, 81 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "てつのこぶし" }, GenderRatio.M7F1));
            dexData.Add(new Species(392, "ゴウカザル", "", new uint[] { 76, 104, 71, 104, 71, 108 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "てつのこぶし" }, GenderRatio.M7F1));
            dexData.Add(new Species(393, "ポッチャマ", "", new uint[] { 53, 51, 53, 61, 56, 40 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "まけんき" }, GenderRatio.M7F1));
            dexData.Add(new Species(394, "ポッタイシ", "", new uint[] { 64, 66, 68, 81, 76, 50 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "まけんき" }, GenderRatio.M7F1));
            dexData.Add(new Species(395, "エンペルト", "", new uint[] { 84, 86, 88, 111, 101, 60 }, (PokeType.Water, PokeType.Steel), new string[] { "げきりゅう", "げきりゅう", "まけんき" }, GenderRatio.M7F1));
            dexData.Add(new Species(396, "ムックル", "", new uint[] { 40, 55, 30, 30, 30, 60 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "するどいめ", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(397, "ムクバード", "", new uint[] { 55, 75, 50, 40, 40, 80 }, (PokeType.Normal, PokeType.Flying), new string[] { "いかく", "いかく", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(398, "ムクホーク", "", new uint[] { 85, 120, 70, 50, 60, 100 }, (PokeType.Normal, PokeType.Flying), new string[] { "いかく", "いかく", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(399, "ビッパ", "", new uint[] { 59, 45, 40, 35, 40, 31 }, (PokeType.Normal, PokeType.Non), new string[] { "たんじゅん", "てんねん", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(400, "ビーダル", "", new uint[] { 79, 85, 60, 55, 60, 71 }, (PokeType.Normal, PokeType.Non), new string[] { "たんじゅん", "てんねん", "ムラっけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(401, "コロボーシ", "", new uint[] { 37, 25, 41, 25, 41, 25 }, (PokeType.Bug, PokeType.Non), new string[] { "だっぴ", "だっぴ", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(402, "コロトック", "", new uint[] { 77, 85, 51, 55, 51, 65 }, (PokeType.Bug, PokeType.Non), new string[] { "むしのしらせ", "むしのしらせ", "テクニシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(403, "コリンク", "", new uint[] { 45, 65, 34, 40, 34, 45 }, (PokeType.Electric, PokeType.Non), new string[] { "とうそうしん", "いかく", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(404, "ルクシオ", "", new uint[] { 60, 85, 49, 60, 49, 60 }, (PokeType.Electric, PokeType.Non), new string[] { "とうそうしん", "いかく", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(405, "レントラー", "", new uint[] { 80, 120, 79, 95, 79, 70 }, (PokeType.Electric, PokeType.Non), new string[] { "とうそうしん", "いかく", "こんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(406, "スボミー", "", new uint[] { 40, 30, 35, 50, 70, 55 }, (PokeType.Grass, PokeType.Poison), new string[] { "しぜんかいふく", "どくのトゲ", "リーフガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(407, "ロズレイド", "", new uint[] { 60, 70, 65, 125, 105, 90 }, (PokeType.Grass, PokeType.Poison), new string[] { "しぜんかいふく", "どくのトゲ", "テクニシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(408, "ズガイドス", "", new uint[] { 67, 125, 40, 30, 30, 58 }, (PokeType.Rock, PokeType.Non), new string[] { "かたやぶり", "かたやぶり", "ちからずく" }, GenderRatio.M7F1));
            dexData.Add(new Species(409, "ラムパルド", "", new uint[] { 97, 165, 60, 65, 50, 58 }, (PokeType.Rock, PokeType.Non), new string[] { "かたやぶり", "かたやぶり", "ちからずく" }, GenderRatio.M7F1));
            dexData.Add(new Species(410, "タテトプス", "", new uint[] { 30, 42, 118, 42, 88, 30 }, (PokeType.Rock, PokeType.Steel), new string[] { "がんじょう", "がんじょう", "ぼうおん" }, GenderRatio.M7F1));
            dexData.Add(new Species(411, "トリデプス", "", new uint[] { 60, 52, 168, 47, 138, 30 }, (PokeType.Rock, PokeType.Steel), new string[] { "がんじょう", "がんじょう", "ぼうおん" }, GenderRatio.M7F1));
            dexData.Add(new Species(412, "ミノムッチ", "", new uint[] { 40, 29, 45, 29, 45, 36 }, (PokeType.Bug, PokeType.Grass), new string[] { "だっぴ", "だっぴ", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(413, "ミノマダム", "草木", new uint[] { 60, 59, 85, 79, 105, 36 }, (PokeType.Bug, PokeType.Grass), new string[] { "きけんよち", "きけんよち", "ぼうじん" }, GenderRatio.FemaleOnly));
            dexData.Add(new AnotherForm(413, "ミノマダム", "砂地", new uint[] { 60, 79, 105, 59, 85, 36 }, (PokeType.Bug, PokeType.Ground), new string[] { "きけんよち", "きけんよち", "ぼうじん" }, GenderRatio.FemaleOnly));
            dexData.Add(new AnotherForm(413, "ミノマダム", "ゴミ", new uint[] { 60, 69, 95, 69, 95, 36 }, (PokeType.Bug, PokeType.Steel), new string[] { "きけんよち", "きけんよち", "ぼうじん" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(414, "ガーメイル", "", new uint[] { 70, 94, 50, 94, 50, 66 }, (PokeType.Bug, PokeType.Flying), new string[] { "むしのしらせ", "むしのしらせ", "いろめがね" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(415, "ミツハニー", "", new uint[] { 30, 30, 42, 30, 42, 70 }, (PokeType.Bug, PokeType.Flying), new string[] { "みつあつめ", "みつあつめ", "はりきり" }, GenderRatio.M7F1));
            dexData.Add(new Species(416, "ビークイン", "", new uint[] { 70, 80, 102, 80, 102, 40 }, (PokeType.Bug, PokeType.Flying), new string[] { "プレッシャー", "プレッシャー", "きんちょうかん" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(417, "パチリス", "", new uint[] { 60, 45, 70, 45, 90, 95 }, (PokeType.Electric, PokeType.Non), new string[] { "にげあし", "ものひろい", "ちくでん" }, GenderRatio.M1F1));
            dexData.Add(new Species(418, "ブイゼル", "", new uint[] { 55, 65, 35, 60, 30, 85 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(419, "フローゼル", "", new uint[] { 85, 105, 55, 85, 50, 115 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "すいすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(420, "チェリンボ", "", new uint[] { 45, 35, 45, 62, 53, 35 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "ようりょくそ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(421, "チェリム", "", new uint[] { 70, 60, 70, 87, 78, 85 }, (PokeType.Grass, PokeType.Non), new string[] { "フラワーギフト", "フラワーギフト", "フラワーギフト" }, GenderRatio.M1F1));
            dexData.Add(new Species(422, "カラナクシ", "", new uint[] { 76, 48, 48, 57, 62, 34 }, (PokeType.Water, PokeType.Non), new string[] { "ねんちゃく", "よびみず", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(423, "トリトドン", "", new uint[] { 111, 83, 68, 92, 82, 39 }, (PokeType.Water, PokeType.Ground), new string[] { "ねんちゃく", "よびみず", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(424, "エテボース", "", new uint[] { 75, 100, 66, 60, 66, 115 }, (PokeType.Normal, PokeType.Non), new string[] { "テクニシャン", "ものひろい", "スキルリンク" }, GenderRatio.M1F1));
            dexData.Add(new Species(425, "フワンテ", "", new uint[] { 90, 50, 34, 60, 44, 70 }, (PokeType.Ghost, PokeType.Flying), new string[] { "ゆうばく", "かるわざ", "ねつぼうそう" }, GenderRatio.M1F1));
            dexData.Add(new Species(426, "フワライド", "", new uint[] { 150, 80, 44, 90, 54, 80 }, (PokeType.Ghost, PokeType.Flying), new string[] { "ゆうばく", "かるわざ", "ねつぼうそう" }, GenderRatio.M1F1));
            dexData.Add(new Species(427, "ミミロル", "", new uint[] { 55, 66, 44, 44, 56, 85 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "ぶきよう", "じゅうなん" }, GenderRatio.M1F1));
            dexData.Add(new Species(428, "ミミロップ", "", new uint[] { 65, 76, 84, 54, 96, 105 }, (PokeType.Normal, PokeType.Non), new string[] { "メロメロボディ", "ぶきよう", "じゅうなん" }, GenderRatio.M1F1));
            dexData.Add(new Species(429, "ムウマージ", "", new uint[] { 60, 60, 60, 105, 105, 105 }, (PokeType.Ghost, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(430, "ドンカラス", "", new uint[] { 100, 125, 52, 105, 52, 71 }, (PokeType.Dark, PokeType.Flying), new string[] { "ふみん", "きょううん", "じしんかじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(431, "ニャルマー", "", new uint[] { 49, 55, 42, 42, 37, 85 }, (PokeType.Normal, PokeType.Non), new string[] { "じゅうなん", "マイペース", "するどいめ" }, GenderRatio.M1F3));
            dexData.Add(new Species(432, "ブニャット", "", new uint[] { 71, 82, 64, 64, 59, 112 }, (PokeType.Normal, PokeType.Non), new string[] { "あついしぼう", "マイペース", "まけんき" }, GenderRatio.M1F3));
            dexData.Add(new Species(433, "リーシャン", "", new uint[] { 45, 30, 50, 65, 50, 45 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(434, "スカンプー", "", new uint[] { 63, 63, 47, 41, 41, 74 }, (PokeType.Poison, PokeType.Dark), new string[] { "あくしゅう", "ゆうばく", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(435, "スカタンク", "", new uint[] { 103, 93, 67, 71, 61, 84 }, (PokeType.Poison, PokeType.Dark), new string[] { "あくしゅう", "ゆうばく", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(436, "ドーミラー", "", new uint[] { 57, 24, 86, 24, 86, 23 }, (PokeType.Steel, PokeType.Psychic), new string[] { "ふゆう", "たいねつ", "ヘヴィメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(437, "ドータクン", "", new uint[] { 67, 89, 116, 79, 116, 33 }, (PokeType.Steel, PokeType.Psychic), new string[] { "ふゆう", "たいねつ", "ヘヴィメタル" }, GenderRatio.Genderless));
            dexData.Add(new Species(438, "ウソハチ", "", new uint[] { 50, 80, 95, 10, 45, 10 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "いしあたま", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(439, "マネネ", "", new uint[] { 20, 25, 45, 70, 90, 60 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "ぼうおん", "フィルター", "テクニシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(440, "ピンプク", "", new uint[] { 100, 5, 5, 15, 65, 30 }, (PokeType.Normal, PokeType.Non), new string[] { "しぜんかいふく", "てんのめぐみ", "フレンドガード" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(441, "ペラップ", "", new uint[] { 76, 65, 45, 92, 42, 91 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちどりあし", "はとむね" }, GenderRatio.M1F1));
            dexData.Add(new Species(442, "ミカルゲ", "", new uint[] { 50, 92, 108, 92, 108, 35 }, (PokeType.Ghost, PokeType.Dark), new string[] { "プレッシャー", "プレッシャー", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(443, "フカマル", "", new uint[] { 58, 70, 45, 40, 45, 42 }, (PokeType.Dragon, PokeType.Ground), new string[] { "すながくれ", "すながくれ", "さめはだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(444, "ガバイト", "", new uint[] { 68, 90, 65, 50, 55, 82 }, (PokeType.Dragon, PokeType.Ground), new string[] { "すながくれ", "すながくれ", "さめはだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(445, "ガブリアス", "", new uint[] { 108, 130, 95, 80, 85, 102 }, (PokeType.Dragon, PokeType.Ground), new string[] { "すながくれ", "すながくれ", "さめはだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(446, "ゴンベ", "", new uint[] { 135, 85, 40, 40, 85, 5 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "あついしぼう", "くいしんぼう" }, GenderRatio.M7F1));
            dexData.Add(new Species(447, "リオル", "", new uint[] { 40, 70, 40, 35, 40, 60 }, (PokeType.Fighting, PokeType.Non), new string[] { "ふくつのこころ", "せいしんりょく", "いたずらごころ" }, GenderRatio.M7F1));
            dexData.Add(new Species(448, "ルカリオ", "", new uint[] { 70, 110, 70, 115, 70, 90 }, (PokeType.Fighting, PokeType.Steel), new string[] { "ふくつのこころ", "せいしんりょく", "せいぎのこころ" }, GenderRatio.M7F1));
            dexData.Add(new Species(449, "ヒポポタス", "", new uint[] { 68, 72, 78, 38, 42, 32 }, (PokeType.Ground, PokeType.Non), new string[] { "すなおこし", "すなおこし", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(450, "カバルドン", "", new uint[] { 108, 112, 118, 68, 72, 47 }, (PokeType.Ground, PokeType.Non), new string[] { "すなおこし", "すなおこし", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(451, "スコルピ", "", new uint[] { 40, 50, 90, 30, 55, 65 }, (PokeType.Poison, PokeType.Bug), new string[] { "カブトアーマー", "スナイパー", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(452, "ドラピオン", "", new uint[] { 70, 90, 110, 60, 75, 95 }, (PokeType.Poison, PokeType.Dark), new string[] { "カブトアーマー", "スナイパー", "するどいめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(453, "グレッグル", "", new uint[] { 48, 61, 40, 61, 40, 50 }, (PokeType.Poison, PokeType.Fighting), new string[] { "きけんよち", "かんそうはだ", "どくしゅ" }, GenderRatio.M1F1));
            dexData.Add(new Species(454, "ドクロッグ", "", new uint[] { 83, 106, 65, 86, 65, 85 }, (PokeType.Poison, PokeType.Fighting), new string[] { "きけんよち", "かんそうはだ", "どくしゅ" }, GenderRatio.M1F1));
            dexData.Add(new Species(455, "マスキッパ", "", new uint[] { 74, 100, 72, 90, 72, 46 }, (PokeType.Grass, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(456, "ケイコウオ", "", new uint[] { 49, 49, 56, 49, 61, 66 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "よびみず", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(457, "ネオラント", "", new uint[] { 69, 69, 76, 69, 86, 91 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "よびみず", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(458, "タマンタ", "", new uint[] { 45, 20, 50, 60, 120, 50 }, (PokeType.Water, PokeType.Flying), new string[] { "すいすい", "ちょすい", "みずのベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(459, "ユキカブリ", "", new uint[] { 60, 62, 50, 62, 60, 40 }, (PokeType.Grass, PokeType.Ice), new string[] { "ゆきふらし", "ゆきふらし", "ぼうおん" }, GenderRatio.M1F1));
            dexData.Add(new Species(460, "ユキノオー", "", new uint[] { 90, 92, 75, 92, 85, 60 }, (PokeType.Grass, PokeType.Ice), new string[] { "ゆきふらし", "ゆきふらし", "ぼうおん" }, GenderRatio.M1F1));
            dexData.Add(new Species(461, "マニューラ", "", new uint[] { 70, 120, 65, 45, 85, 125 }, (PokeType.Dark, PokeType.Ice), new string[] { "プレッシャー", "プレッシャー", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(462, "ジバコイル", "", new uint[] { 70, 70, 115, 130, 90, 60 }, (PokeType.Electric, PokeType.Steel), new string[] { "じりょく", "がんじょう", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(463, "ベロベルト", "", new uint[] { 110, 85, 95, 80, 95, 50 }, (PokeType.Normal, PokeType.Non), new string[] { "マイペース", "どんかん", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(464, "ドサイドン", "", new uint[] { 115, 140, 130, 55, 55, 40 }, (PokeType.Ground, PokeType.Rock), new string[] { "ひらいしん", "ハードロック", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(465, "モジャンボ", "", new uint[] { 100, 100, 125, 110, 50, 50 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "リーフガード", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(466, "エレキブル", "", new uint[] { 75, 123, 67, 95, 85, 95 }, (PokeType.Electric, PokeType.Non), new string[] { "でんきエンジン", "でんきエンジン", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(467, "ブーバーン", "", new uint[] { 75, 95, 67, 125, 95, 83 }, (PokeType.Fire, PokeType.Non), new string[] { "ほのおのからだ", "ほのおのからだ", "やるき" }, GenderRatio.M3F1));
            dexData.Add(new Species(468, "トゲキッス", "", new uint[] { 85, 50, 95, 120, 115, 80 }, (PokeType.Fairy, PokeType.Flying), new string[] { "はりきり", "てんのめぐみ", "きょううん" }, GenderRatio.M7F1));
            dexData.Add(new Species(469, "メガヤンマ", "", new uint[] { 86, 76, 86, 116, 56, 95 }, (PokeType.Bug, PokeType.Flying), new string[] { "かそく", "いろめがね", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(470, "リーフィア", "", new uint[] { 65, 110, 130, 60, 65, 95 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "リーフガード", "ようりょくそ" }, GenderRatio.M7F1));
            dexData.Add(new Species(471, "グレイシア", "", new uint[] { 65, 60, 110, 130, 95, 65 }, (PokeType.Ice, PokeType.Non), new string[] { "ゆきがくれ", "ゆきがくれ", "アイスボディ" }, GenderRatio.M7F1));
            dexData.Add(new Species(472, "グライオン", "", new uint[] { 75, 95, 125, 45, 75, 95 }, (PokeType.Ground, PokeType.Flying), new string[] { "かいりきバサミ", "すながくれ", "ポイズンヒール" }, GenderRatio.M1F1));
            dexData.Add(new Species(473, "マンムー", "", new uint[] { 110, 130, 80, 70, 60, 80 }, (PokeType.Ice, PokeType.Ground), new string[] { "どんかん", "ゆきがくれ", "あついしぼう" }, GenderRatio.M1F1));
            dexData.Add(new Species(474, "ポリゴンZ", "", new uint[] { 85, 80, 70, 135, 75, 90 }, (PokeType.Normal, PokeType.Non), new string[] { "てきおうりょく", "ダウンロード", "アナライズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(475, "エルレイド", "", new uint[] { 68, 125, 65, 65, 115, 80 }, (PokeType.Psychic, PokeType.Fighting), new string[] { "ふくつのこころ", "ふくつのこころ", "せいぎのこころ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(476, "ダイノーズ", "", new uint[] { 60, 55, 145, 75, 150, 40 }, (PokeType.Rock, PokeType.Steel), new string[] { "がんじょう", "じりょく", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(477, "ヨノワール", "", new uint[] { 45, 100, 135, 65, 135, 45 }, (PokeType.Ghost, PokeType.Non), new string[] { "プレッシャー", "プレッシャー", "おみとおし" }, GenderRatio.M1F1));
            dexData.Add(new Species(478, "ユキメノコ", "", new uint[] { 70, 80, 70, 80, 70, 110 }, (PokeType.Ice, PokeType.Ghost), new string[] { "ゆきがくれ", "ゆきがくれ", "のろわれボディ" }, GenderRatio.FemaleOnly));
            dexData.Add(new AnotherForm(479, "ロトム", "ノーマル", new uint[] { 50, 50, 77, 95, 77, 91 }, (PokeType.Electric, PokeType.Ghost), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(479, "ロトム", "ヒート", new uint[] { 50, 65, 107, 105, 107, 86 }, (PokeType.Electric, PokeType.Fire), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(479, "ロトム", "ウォッシュ", new uint[] { 50, 65, 107, 105, 107, 86 }, (PokeType.Electric, PokeType.Water), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(479, "ロトム", "フロスト", new uint[] { 50, 65, 107, 105, 107, 86 }, (PokeType.Electric, PokeType.Ice), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(479, "ロトム", "スピン", new uint[] { 50, 65, 107, 105, 107, 86 }, (PokeType.Electric, PokeType.Flying), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(479, "ロトム", "カット", new uint[] { 50, 65, 107, 105, 107, 86 }, (PokeType.Electric, PokeType.Grass), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(480, "ユクシー", "", new uint[] { 75, 75, 130, 75, 130, 95 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(481, "エムリット", "", new uint[] { 80, 105, 105, 105, 105, 80 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(482, "アグノム", "", new uint[] { 75, 125, 70, 125, 70, 115 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(483, "ディアルガ", "", new uint[] { 100, 120, 120, 150, 100, 90 }, (PokeType.Steel, PokeType.Dragon), new string[] { "プレッシャー", "プレッシャー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(484, "パルキア", "", new uint[] { 90, 120, 100, 150, 120, 100 }, (PokeType.Water, PokeType.Dragon), new string[] { "プレッシャー", "プレッシャー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(485, "ヒードラン", "", new uint[] { 91, 90, 106, 130, 106, 77 }, (PokeType.Fire, PokeType.Steel), new string[] { "もらいび", "もらいび", "ほのおのからだ" }, GenderRatio.M1F1));
            dexData.Add(new Species(486, "レジギガス", "", new uint[] { 110, 160, 110, 80, 110, 100 }, (PokeType.Normal, PokeType.Non), new string[] { "スロースタート", "スロースタート", "スロースタート" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(487, "ギラティナ", "アナザー", new uint[] { 150, 100, 120, 100, 120, 90 }, (PokeType.Ghost, PokeType.Dragon), new string[] { "プレッシャー", "プレッシャー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(487, "ギラティナ", "オリジン", new uint[] { 150, 120, 100, 120, 100, 90 }, (PokeType.Ghost, PokeType.Dragon), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(488, "クレセリア", "", new uint[] { 120, 70, 120, 75, 130, 85 }, (PokeType.Psychic, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(489, "フィオネ", "", new uint[] { 80, 80, 80, 80, 80, 80 }, (PokeType.Water, PokeType.Non), new string[] { "うるおいボディ", "うるおいボディ", "うるおいボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(490, "マナフィ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Water, PokeType.Non), new string[] { "うるおいボディ", "うるおいボディ", "うるおいボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(491, "ダークライ", "", new uint[] { 70, 90, 90, 135, 90, 125 }, (PokeType.Dark, PokeType.Non), new string[] { "ナイトメア", "ナイトメア", "ナイトメア" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(492, "シェイミ", "ランド", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Grass, PokeType.Non), new string[] { "しぜんかいふく", "しぜんかいふく", "しぜんかいふく" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(492, "シェイミ", "スカイ", new uint[] { 100, 103, 75, 120, 75, 127 }, (PokeType.Grass, PokeType.Flying), new string[] { "てんのめぐみ", "てんのめぐみ", "てんのめぐみ" }, GenderRatio.Genderless));
            dexData.Add(new Species(493, "アルセウス", "", new uint[] { 120, 120, 120, 120, 120, 120 }, (PokeType.Normal, PokeType.Non), new string[] { "マルチタイプ", "マルチタイプ", "マルチタイプ" }, GenderRatio.Genderless));
            dexData.Add(new Species(494, "ビクティニ", "", new uint[] { 100, 100, 100, 100, 100, 100 }, (PokeType.Psychic, PokeType.Fire), new string[] { "しょうりのほし", "しょうりのほし", "しょうりのほし" }, GenderRatio.Genderless));
            dexData.Add(new Species(495, "ツタージャ", "", new uint[] { 45, 45, 55, 45, 55, 63 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "あまのじゃく" }, GenderRatio.M7F1));
            dexData.Add(new Species(496, "ジャノビー", "", new uint[] { 60, 60, 75, 60, 75, 83 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "あまのじゃく" }, GenderRatio.M7F1));
            dexData.Add(new Species(497, "ジャローダ", "", new uint[] { 75, 75, 95, 75, 95, 113 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "あまのじゃく" }, GenderRatio.M7F1));
            dexData.Add(new Species(498, "ポカブ", "", new uint[] { 65, 63, 45, 45, 45, 45 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "あついしぼう" }, GenderRatio.M7F1));
            dexData.Add(new Species(499, "チャオブー", "", new uint[] { 90, 93, 55, 70, 55, 55 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "あついしぼう" }, GenderRatio.M7F1));
            dexData.Add(new Species(500, "エンブオー", "", new uint[] { 110, 123, 65, 100, 65, 65 }, (PokeType.Fire, PokeType.Fighting), new string[] { "もうか", "もうか", "すてみ" }, GenderRatio.M7F1));
            dexData.Add(new Species(501, "ミジュマル", "", new uint[] { 55, 55, 45, 63, 45, 45 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(502, "フタチマル", "", new uint[] { 75, 75, 60, 83, 60, 60 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(503, "ダイケンキ", "", new uint[] { 95, 100, 85, 108, 70, 70 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "シェルアーマー" }, GenderRatio.M7F1));
            dexData.Add(new Species(504, "ミネズミ", "", new uint[] { 45, 55, 39, 35, 39, 42 }, (PokeType.Normal, PokeType.Non), new string[] { "にげあし", "するどいめ", "アナライズ" }, GenderRatio.M1F1));
            dexData.Add(new Species(505, "ミルホッグ", "", new uint[] { 60, 85, 69, 60, 69, 77 }, (PokeType.Normal, PokeType.Non), new string[] { "はっこう", "するどいめ", "アナライズ" }, GenderRatio.M1F1));
            dexData.Add(new Species(506, "ヨーテリー", "", new uint[] { 45, 60, 45, 25, 45, 55 }, (PokeType.Normal, PokeType.Non), new string[] { "やるき", "ものひろい", "にげあし" }, GenderRatio.M1F1));
            dexData.Add(new Species(507, "ハーデリア", "", new uint[] { 65, 80, 65, 35, 65, 60 }, (PokeType.Normal, PokeType.Non), new string[] { "いかく", "すなかき", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(508, "ムーランド", "", new uint[] { 85, 110, 90, 45, 90, 80 }, (PokeType.Normal, PokeType.Non), new string[] { "いかく", "すなかき", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(509, "チョロネコ", "", new uint[] { 41, 50, 37, 50, 37, 66 }, (PokeType.Dark, PokeType.Non), new string[] { "じゅうなん", "かるわざ", "いたずらごころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(510, "レパルダス", "", new uint[] { 64, 88, 50, 88, 50, 106 }, (PokeType.Dark, PokeType.Non), new string[] { "じゅうなん", "かるわざ", "いたずらごころ" }, GenderRatio.M1F1));
            dexData.Add(new Species(511, "ヤナップ", "", new uint[] { 50, 53, 48, 53, 48, 64 }, (PokeType.Grass, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(512, "ヤナッキー", "", new uint[] { 75, 98, 63, 98, 63, 101 }, (PokeType.Grass, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "しんりょく" }, GenderRatio.M7F1));
            dexData.Add(new Species(513, "バオップ", "", new uint[] { 50, 53, 48, 53, 48, 64 }, (PokeType.Fire, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(514, "バオッキー", "", new uint[] { 75, 98, 63, 98, 63, 101 }, (PokeType.Fire, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "もうか" }, GenderRatio.M7F1));
            dexData.Add(new Species(515, "ヒヤップ", "", new uint[] { 50, 53, 48, 53, 48, 64 }, (PokeType.Water, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(516, "ヒヤッキー", "", new uint[] { 75, 98, 63, 98, 63, 101 }, (PokeType.Water, PokeType.Non), new string[] { "くいしんぼう", "くいしんぼう", "げきりゅう" }, GenderRatio.M7F1));
            dexData.Add(new Species(517, "ムンナ", "", new uint[] { 76, 25, 45, 67, 55, 24 }, (PokeType.Psychic, PokeType.Non), new string[] { "よちむ", "シンクロ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(518, "ムシャーナ", "", new uint[] { 116, 55, 85, 107, 95, 29 }, (PokeType.Psychic, PokeType.Non), new string[] { "よちむ", "シンクロ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(519, "マメパト", "", new uint[] { 50, 55, 50, 36, 30, 43 }, (PokeType.Normal, PokeType.Flying), new string[] { "はとむね", "きょううん", "とうそうしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(520, "ハトーボー", "", new uint[] { 62, 77, 62, 50, 42, 65 }, (PokeType.Normal, PokeType.Flying), new string[] { "はとむね", "きょううん", "とうそうしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(521, "ケンホロウ", "", new uint[] { 80, 115, 80, 65, 55, 93 }, (PokeType.Normal, PokeType.Flying), new string[] { "はとむね", "きょううん", "とうそうしん" }, GenderRatio.M1F1));
            dexData.Add(new Species(522, "シママ", "", new uint[] { 45, 60, 32, 50, 32, 76 }, (PokeType.Electric, PokeType.Non), new string[] { "ひらいしん", "でんきエンジン", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(523, "ゼブライカ", "", new uint[] { 75, 100, 63, 80, 63, 116 }, (PokeType.Electric, PokeType.Non), new string[] { "ひらいしん", "でんきエンジン", "そうしょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(524, "ダンゴロ", "", new uint[] { 55, 75, 85, 25, 25, 15 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "くだけるよろい", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(525, "ガントル", "", new uint[] { 70, 105, 105, 50, 40, 20 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "くだけるよろい", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(526, "ギガイアス", "", new uint[] { 85, 135, 130, 60, 80, 25 }, (PokeType.Rock, PokeType.Non), new string[] { "がんじょう", "すなおこし", "すなのちから" }, GenderRatio.M1F1));
            dexData.Add(new Species(527, "コロモリ", "", new uint[] { 65, 45, 43, 55, 43, 72 }, (PokeType.Psychic, PokeType.Flying), new string[] { "てんねん", "ぶきよう", "たんじゅん" }, GenderRatio.M1F1));
            dexData.Add(new Species(528, "ココロモリ", "", new uint[] { 67, 57, 55, 77, 55, 114 }, (PokeType.Psychic, PokeType.Flying), new string[] { "てんねん", "ぶきよう", "たんじゅん" }, GenderRatio.M1F1));
            dexData.Add(new Species(529, "モグリュー", "", new uint[] { 60, 85, 40, 30, 45, 68 }, (PokeType.Ground, PokeType.Non), new string[] { "すなかき", "すなのちから", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new Species(530, "ドリュウズ", "", new uint[] { 110, 135, 60, 50, 65, 88 }, (PokeType.Ground, PokeType.Steel), new string[] { "すなかき", "すなのちから", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new Species(531, "タブンネ", "", new uint[] { 103, 60, 86, 60, 86, 50 }, (PokeType.Normal, PokeType.Non), new string[] { "いやしのこころ", "さいせいりょく", "ぶきよう" }, GenderRatio.M1F1));
            dexData.Add(new Species(532, "ドッコラー", "", new uint[] { 75, 80, 55, 25, 35, 35 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ちからずく", "てつのこぶし" }, GenderRatio.M3F1));
            dexData.Add(new Species(533, "ドテッコツ", "", new uint[] { 85, 105, 85, 40, 50, 40 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ちからずく", "てつのこぶし" }, GenderRatio.M3F1));
            dexData.Add(new Species(534, "ローブシン", "", new uint[] { 105, 140, 95, 55, 65, 45 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "ちからずく", "てつのこぶし" }, GenderRatio.M3F1));
            dexData.Add(new Species(535, "オタマロ", "", new uint[] { 50, 50, 40, 50, 40, 64 }, (PokeType.Water, PokeType.Non), new string[] { "すいすい", "うるおいボディ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(536, "ガマガル", "", new uint[] { 75, 65, 55, 65, 55, 69 }, (PokeType.Water, PokeType.Ground), new string[] { "すいすい", "うるおいボディ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(537, "ガマゲロゲ", "", new uint[] { 105, 95, 75, 85, 75, 74 }, (PokeType.Water, PokeType.Ground), new string[] { "すいすい", "どくしゅ", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(538, "ナゲキ", "", new uint[] { 120, 100, 85, 30, 85, 45 }, (PokeType.Fighting, PokeType.Non), new string[] { "こんじょう", "せいしんりょく", "かたやぶり" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(539, "ダゲキ", "", new uint[] { 75, 125, 75, 30, 75, 85 }, (PokeType.Fighting, PokeType.Non), new string[] { "がんじょう", "せいしんりょく", "かたやぶり" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(540, "クルミル", "", new uint[] { 45, 53, 70, 40, 60, 42 }, (PokeType.Bug, PokeType.Grass), new string[] { "むしのしらせ", "ようりょくそ", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(541, "クルマユ", "", new uint[] { 55, 63, 90, 50, 80, 42 }, (PokeType.Bug, PokeType.Grass), new string[] { "リーフガード", "ようりょくそ", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(542, "ハハコモリ", "", new uint[] { 75, 103, 80, 70, 80, 92 }, (PokeType.Bug, PokeType.Grass), new string[] { "むしのしらせ", "ようりょくそ", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(543, "フシデ", "", new uint[] { 30, 45, 59, 30, 39, 57 }, (PokeType.Bug, PokeType.Poison), new string[] { "どくのトゲ", "むしのしらせ", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(544, "ホイーガ", "", new uint[] { 40, 55, 99, 40, 79, 47 }, (PokeType.Bug, PokeType.Poison), new string[] { "どくのトゲ", "むしのしらせ", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(545, "ペンドラー", "", new uint[] { 60, 100, 89, 55, 69, 112 }, (PokeType.Bug, PokeType.Poison), new string[] { "どくのトゲ", "むしのしらせ", "かそく" }, GenderRatio.M1F1));
            dexData.Add(new Species(546, "モンメン", "", new uint[] { 40, 27, 60, 37, 50, 66 }, (PokeType.Grass, PokeType.Fairy), new string[] { "いたずらごころ", "すりぬけ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(547, "エルフーン", "", new uint[] { 60, 67, 85, 77, 75, 116 }, (PokeType.Grass, PokeType.Fairy), new string[] { "いたずらごころ", "すりぬけ", "ようりょくそ" }, GenderRatio.M1F1));
            dexData.Add(new Species(548, "チュリネ", "", new uint[] { 45, 35, 50, 70, 50, 30 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "マイペース", "リーフガード" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(549, "ドレディア", "", new uint[] { 70, 60, 75, 110, 75, 90 }, (PokeType.Grass, PokeType.Non), new string[] { "ようりょくそ", "マイペース", "リーフガード" }, GenderRatio.FemaleOnly));
            dexData.Add(new AnotherForm(550, "バスラオ", "あか", new uint[] { 70, 92, 65, 80, 55, 98 }, (PokeType.Water, PokeType.Non), new string[] { "すてみ", "てきおうりょく", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(550, "バスラオ", "あお", new uint[] { 70, 92, 65, 80, 55, 98 }, (PokeType.Water, PokeType.Non), new string[] { "いしあたま", "てきおうりょく", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new Species(551, "メグロコ", "", new uint[] { 50, 72, 35, 35, 35, 65 }, (PokeType.Ground, PokeType.Dark), new string[] { "いかく", "じしんかじょう", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new Species(552, "ワルビル", "", new uint[] { 60, 82, 45, 45, 45, 74 }, (PokeType.Ground, PokeType.Dark), new string[] { "いかく", "じしんかじょう", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new Species(553, "ワルビアル", "", new uint[] { 95, 117, 80, 65, 70, 92 }, (PokeType.Ground, PokeType.Dark), new string[] { "いかく", "じしんかじょう", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new Species(554, "ダルマッカ", "", new uint[] { 70, 90, 45, 15, 45, 50 }, (PokeType.Fire, PokeType.Non), new string[] { "はりきり", "はりきり", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(555, "ヒヒダルマ", "", new uint[] { 105, 140, 55, 30, 55, 95 }, (PokeType.Fire, PokeType.Non), new string[] { "ちからずく", "ちからずく", "ダルマモード" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(555, "ヒヒダルマ", "ダルマ", new uint[] { 105, 30, 105, 140, 105, 55 }, (PokeType.Fire, PokeType.Psychic), new string[] { "ちからずく", "ちからずく", "ダルマモード" }, GenderRatio.M1F1));
            dexData.Add(new Species(556, "マラカッチ", "", new uint[] { 75, 86, 67, 106, 67, 60 }, (PokeType.Grass, PokeType.Non), new string[] { "ちょすい", "ようりょくそ", "よびみず" }, GenderRatio.M1F1));
            dexData.Add(new Species(557, "イシズマイ", "", new uint[] { 50, 65, 85, 35, 35, 55 }, (PokeType.Bug, PokeType.Rock), new string[] { "がんじょう", "シェルアーマー", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(558, "イワパレス", "", new uint[] { 70, 105, 125, 65, 75, 45 }, (PokeType.Bug, PokeType.Rock), new string[] { "がんじょう", "シェルアーマー", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(559, "ズルッグ", "", new uint[] { 50, 75, 70, 35, 70, 48 }, (PokeType.Dark, PokeType.Fighting), new string[] { "だっぴ", "じしんかじょう", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(560, "ズルズキン", "", new uint[] { 65, 90, 115, 45, 115, 58 }, (PokeType.Dark, PokeType.Fighting), new string[] { "だっぴ", "じしんかじょう", "いかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(561, "シンボラー", "", new uint[] { 72, 58, 80, 103, 80, 97 }, (PokeType.Psychic, PokeType.Flying), new string[] { "ミラクルスキン", "マジックガード", "いろめがね" }, GenderRatio.M1F1));
            dexData.Add(new Species(562, "デスマス", "", new uint[] { 38, 30, 85, 55, 65, 30 }, (PokeType.Ghost, PokeType.Non), new string[] { "ミイラ", "ミイラ", "ミイラ" }, GenderRatio.M1F1));
            dexData.Add(new Species(563, "デスカーン", "", new uint[] { 58, 50, 145, 95, 105, 30 }, (PokeType.Ghost, PokeType.Non), new string[] { "ミイラ", "ミイラ", "ミイラ" }, GenderRatio.M1F1));
            dexData.Add(new Species(564, "プロトーガ", "", new uint[] { 54, 78, 103, 53, 45, 22 }, (PokeType.Water, PokeType.Rock), new string[] { "ハードロック", "がんじょう", "すいすい" }, GenderRatio.M7F1));
            dexData.Add(new Species(565, "アバゴーラ", "", new uint[] { 74, 108, 133, 83, 65, 32 }, (PokeType.Water, PokeType.Rock), new string[] { "ハードロック", "がんじょう", "すいすい" }, GenderRatio.M7F1));
            dexData.Add(new Species(566, "アーケン", "", new uint[] { 55, 112, 45, 74, 45, 70 }, (PokeType.Rock, PokeType.Flying), new string[] { "よわき", "よわき", "よわき" }, GenderRatio.M7F1));
            dexData.Add(new Species(567, "アーケオス", "", new uint[] { 75, 140, 65, 112, 65, 110 }, (PokeType.Rock, PokeType.Flying), new string[] { "よわき", "よわき", "よわき" }, GenderRatio.M7F1));
            dexData.Add(new Species(568, "ヤブクロン", "", new uint[] { 50, 50, 62, 40, 62, 65 }, (PokeType.Poison, PokeType.Non), new string[] { "あくしゅう", "ねんちゃく", "ゆうばく" }, GenderRatio.M1F1));
            dexData.Add(new Species(569, "ダストダス", "", new uint[] { 80, 95, 82, 60, 82, 75 }, (PokeType.Poison, PokeType.Non), new string[] { "あくしゅう", "くだけるよろい", "ゆうばく" }, GenderRatio.M1F1));
            dexData.Add(new Species(570, "ゾロア", "", new uint[] { 40, 65, 40, 80, 40, 65 }, (PokeType.Dark, PokeType.Non), new string[] { "イリュージョン", "イリュージョン", "イリュージョン" }, GenderRatio.M7F1));
            dexData.Add(new Species(571, "ゾロアーク", "", new uint[] { 60, 105, 60, 120, 60, 105 }, (PokeType.Dark, PokeType.Non), new string[] { "イリュージョン", "イリュージョン", "イリュージョン" }, GenderRatio.M7F1));
            dexData.Add(new Species(572, "チラーミィ", "", new uint[] { 55, 50, 40, 40, 40, 75 }, (PokeType.Normal, PokeType.Non), new string[] { "メロメロボディ", "テクニシャン", "スキルリンク" }, GenderRatio.M1F3));
            dexData.Add(new Species(573, "チラチーノ", "", new uint[] { 75, 95, 60, 65, 60, 115 }, (PokeType.Normal, PokeType.Non), new string[] { "メロメロボディ", "テクニシャン", "スキルリンク" }, GenderRatio.M1F3));
            dexData.Add(new Species(574, "ゴチム", "", new uint[] { 45, 30, 50, 55, 65, 45 }, (PokeType.Psychic, PokeType.Non), new string[] { "おみとおし", "かちき", "かげふみ" }, GenderRatio.M1F3));
            dexData.Add(new Species(575, "ゴチミル", "", new uint[] { 60, 45, 70, 75, 85, 55 }, (PokeType.Psychic, PokeType.Non), new string[] { "おみとおし", "かちき", "かげふみ" }, GenderRatio.M1F3));
            dexData.Add(new Species(576, "ゴチルゼル", "", new uint[] { 70, 55, 95, 95, 110, 65 }, (PokeType.Psychic, PokeType.Non), new string[] { "おみとおし", "かちき", "かげふみ" }, GenderRatio.M1F3));
            dexData.Add(new Species(577, "ユニラン", "", new uint[] { 45, 30, 40, 105, 50, 20 }, (PokeType.Psychic, PokeType.Non), new string[] { "ぼうじん", "マジックガード", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(578, "ダブラン", "", new uint[] { 65, 40, 50, 125, 60, 30 }, (PokeType.Psychic, PokeType.Non), new string[] { "ぼうじん", "マジックガード", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(579, "ランクルス", "", new uint[] { 110, 65, 75, 125, 85, 30 }, (PokeType.Psychic, PokeType.Non), new string[] { "ぼうじん", "マジックガード", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(580, "コアルヒー", "", new uint[] { 62, 44, 50, 44, 50, 55 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "はとむね", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(581, "スワンナ", "", new uint[] { 75, 87, 63, 87, 63, 98 }, (PokeType.Water, PokeType.Flying), new string[] { "するどいめ", "はとむね", "うるおいボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(582, "バニプッチ", "", new uint[] { 36, 50, 50, 65, 60, 44 }, (PokeType.Ice, PokeType.Non), new string[] { "アイスボディ", "ゆきがくれ", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(583, "バニリッチ", "", new uint[] { 51, 65, 65, 80, 75, 59 }, (PokeType.Ice, PokeType.Non), new string[] { "アイスボディ", "ゆきがくれ", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(584, "バイバニラ", "", new uint[] { 71, 95, 85, 110, 95, 79 }, (PokeType.Ice, PokeType.Non), new string[] { "アイスボディ", "ゆきふらし", "くだけるよろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(585, "シキジカ", "", new uint[] { 60, 60, 50, 40, 50, 75 }, (PokeType.Normal, PokeType.Grass), new string[] { "ようりょくそ", "そうしょく", "てんのめぐみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(586, "メブキジカ", "", new uint[] { 80, 100, 70, 60, 70, 95 }, (PokeType.Normal, PokeType.Grass), new string[] { "ようりょくそ", "そうしょく", "てんのめぐみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(587, "エモンガ", "", new uint[] { 55, 75, 60, 75, 60, 103 }, (PokeType.Electric, PokeType.Flying), new string[] { "せいでんき", "せいでんき", "でんきエンジン" }, GenderRatio.M1F1));
            dexData.Add(new Species(588, "カブルモ", "", new uint[] { 50, 75, 45, 40, 45, 60 }, (PokeType.Bug, PokeType.Non), new string[] { "むしのしらせ", "だっぴ", "ノーガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(589, "シュバルゴ", "", new uint[] { 70, 135, 105, 60, 105, 20 }, (PokeType.Bug, PokeType.Steel), new string[] { "むしのしらせ", "シェルアーマー", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(590, "タマゲタケ", "", new uint[] { 69, 55, 45, 55, 55, 15 }, (PokeType.Grass, PokeType.Poison), new string[] { "ほうし", "ほうし", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(591, "モロバレル", "", new uint[] { 114, 85, 70, 85, 80, 30 }, (PokeType.Grass, PokeType.Poison), new string[] { "ほうし", "ほうし", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(592, "プルリル", "", new uint[] { 55, 40, 50, 65, 85, 40 }, (PokeType.Water, PokeType.Ghost), new string[] { "ちょすい", "のろわれボディ", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(593, "ブルンゲル", "", new uint[] { 100, 60, 70, 85, 105, 60 }, (PokeType.Water, PokeType.Ghost), new string[] { "ちょすい", "のろわれボディ", "しめりけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(594, "ママンボウ", "", new uint[] { 165, 75, 80, 40, 45, 65 }, (PokeType.Water, PokeType.Non), new string[] { "いやしのこころ", "うるおいボディ", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(595, "バチュル", "", new uint[] { 50, 47, 50, 57, 50, 65 }, (PokeType.Bug, PokeType.Electric), new string[] { "ふくがん", "きんちょうかん", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(596, "デンチュラ", "", new uint[] { 70, 77, 60, 97, 60, 108 }, (PokeType.Bug, PokeType.Electric), new string[] { "ふくがん", "きんちょうかん", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(597, "テッシード", "", new uint[] { 44, 50, 91, 24, 86, 10 }, (PokeType.Grass, PokeType.Steel), new string[] { "てつのトゲ", "てつのトゲ", "てつのトゲ" }, GenderRatio.M1F1));
            dexData.Add(new Species(598, "ナットレイ", "", new uint[] { 74, 94, 131, 54, 116, 20 }, (PokeType.Grass, PokeType.Steel), new string[] { "てつのトゲ", "てつのトゲ", "きけんよち" }, GenderRatio.M1F1));
            dexData.Add(new Species(599, "ギアル", "", new uint[] { 40, 55, 70, 45, 60, 30 }, (PokeType.Steel, PokeType.Non), new string[] { "プラス", "マイナス", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(600, "ギギアル", "", new uint[] { 60, 80, 95, 70, 85, 50 }, (PokeType.Steel, PokeType.Non), new string[] { "プラス", "マイナス", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(601, "ギギギアル", "", new uint[] { 60, 100, 115, 70, 85, 90 }, (PokeType.Steel, PokeType.Non), new string[] { "プラス", "マイナス", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new Species(602, "シビシラス", "", new uint[] { 35, 55, 40, 45, 40, 60 }, (PokeType.Electric, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(603, "シビビール", "", new uint[] { 65, 85, 70, 75, 70, 40 }, (PokeType.Electric, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(604, "シビルドン", "", new uint[] { 85, 115, 80, 105, 80, 50 }, (PokeType.Electric, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(605, "リグレー", "", new uint[] { 55, 55, 55, 85, 55, 30 }, (PokeType.Psychic, PokeType.Non), new string[] { "テレパシー", "シンクロ", "アナライズ" }, GenderRatio.M1F1));
            dexData.Add(new Species(606, "オーベム", "", new uint[] { 75, 75, 75, 125, 95, 40 }, (PokeType.Psychic, PokeType.Non), new string[] { "テレパシー", "シンクロ", "アナライズ" }, GenderRatio.M1F1));
            dexData.Add(new Species(607, "ヒトモシ", "", new uint[] { 50, 30, 55, 65, 55, 20 }, (PokeType.Ghost, PokeType.Fire), new string[] { "もらいび", "ほのおのからだ", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(608, "ランプラー", "", new uint[] { 60, 40, 60, 95, 60, 55 }, (PokeType.Ghost, PokeType.Fire), new string[] { "もらいび", "ほのおのからだ", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(609, "シャンデラ", "", new uint[] { 60, 55, 90, 145, 90, 80 }, (PokeType.Ghost, PokeType.Fire), new string[] { "もらいび", "ほのおのからだ", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(610, "キバゴ", "", new uint[] { 46, 87, 60, 30, 40, 57 }, (PokeType.Dragon, PokeType.Non), new string[] { "とうそうしん", "かたやぶり", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(611, "オノンド", "", new uint[] { 66, 117, 70, 40, 50, 67 }, (PokeType.Dragon, PokeType.Non), new string[] { "とうそうしん", "かたやぶり", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(612, "オノノクス", "", new uint[] { 76, 147, 90, 60, 70, 97 }, (PokeType.Dragon, PokeType.Non), new string[] { "とうそうしん", "かたやぶり", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(613, "クマシュン", "", new uint[] { 55, 70, 40, 60, 40, 40 }, (PokeType.Ice, PokeType.Non), new string[] { "ゆきがくれ", "ゆきかき", "びびり" }, GenderRatio.M1F1));
            dexData.Add(new Species(614, "ツンベアー", "", new uint[] { 95, 130, 80, 70, 80, 50 }, (PokeType.Ice, PokeType.Non), new string[] { "ゆきがくれ", "ゆきかき", "すいすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(615, "フリージオ", "", new uint[] { 80, 50, 50, 95, 135, 105 }, (PokeType.Ice, PokeType.Non), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.Genderless));
            dexData.Add(new Species(616, "チョボマキ", "", new uint[] { 50, 40, 85, 40, 65, 25 }, (PokeType.Bug, PokeType.Non), new string[] { "うるおいボディ", "シェルアーマー", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(617, "アギルダー", "", new uint[] { 80, 70, 40, 100, 60, 145 }, (PokeType.Bug, PokeType.Non), new string[] { "うるおいボディ", "ねんちゃく", "かるわざ" }, GenderRatio.M1F1));
            dexData.Add(new Species(618, "マッギョ", "", new uint[] { 109, 66, 84, 81, 99, 32 }, (PokeType.Ground, PokeType.Electric), new string[] { "せいでんき", "じゅうなん", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(619, "コジョフー", "", new uint[] { 45, 85, 50, 55, 50, 65 }, (PokeType.Fighting, PokeType.Non), new string[] { "せいしんりょく", "さいせいりょく", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(620, "コジョンド", "", new uint[] { 65, 125, 60, 95, 60, 105 }, (PokeType.Fighting, PokeType.Non), new string[] { "せいしんりょく", "さいせいりょく", "すてみ" }, GenderRatio.M1F1));
            dexData.Add(new Species(621, "クリムガン", "", new uint[] { 77, 120, 90, 60, 90, 48 }, (PokeType.Dragon, PokeType.Non), new string[] { "さめはだ", "ちからずく", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new Species(622, "ゴビット", "", new uint[] { 59, 74, 50, 35, 50, 35 }, (PokeType.Ground, PokeType.Ghost), new string[] { "てつのこぶし", "ぶきよう", "ノーガード" }, GenderRatio.Genderless));
            dexData.Add(new Species(623, "ゴルーグ", "", new uint[] { 89, 124, 80, 55, 80, 55 }, (PokeType.Ground, PokeType.Ghost), new string[] { "てつのこぶし", "ぶきよう", "ノーガード" }, GenderRatio.Genderless));
            dexData.Add(new Species(624, "コマタナ", "", new uint[] { 45, 85, 70, 40, 40, 60 }, (PokeType.Dark, PokeType.Steel), new string[] { "まけんき", "せいしんりょく", "プレッシャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(625, "キリキザン", "", new uint[] { 65, 125, 100, 60, 70, 70 }, (PokeType.Dark, PokeType.Steel), new string[] { "まけんき", "せいしんりょく", "プレッシャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(626, "バッフロン", "", new uint[] { 95, 110, 95, 40, 95, 55 }, (PokeType.Normal, PokeType.Non), new string[] { "すてみ", "そうしょく", "ぼうおん" }, GenderRatio.M1F1));
            dexData.Add(new Species(627, "ワシボン", "", new uint[] { 70, 83, 50, 37, 50, 60 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちからずく", "はりきり" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(628, "ウォーグル", "", new uint[] { 100, 123, 75, 57, 75, 80 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "ちからずく", "まけんき" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(629, "バルチャイ", "", new uint[] { 70, 55, 75, 45, 65, 60 }, (PokeType.Dark, PokeType.Flying), new string[] { "はとむね", "ぼうじん", "くだけるよろい" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(630, "バルジーナ", "", new uint[] { 110, 65, 105, 55, 95, 80 }, (PokeType.Dark, PokeType.Flying), new string[] { "はとむね", "ぼうじん", "くだけるよろい" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(631, "クイタラン", "", new uint[] { 85, 97, 66, 105, 66, 65 }, (PokeType.Fire, PokeType.Non), new string[] { "くいしんぼう", "もらいび", "しろいけむり" }, GenderRatio.M1F1));
            dexData.Add(new Species(632, "アイアント", "", new uint[] { 58, 109, 112, 48, 48, 109 }, (PokeType.Bug, PokeType.Steel), new string[] { "むしのしらせ", "はりきり", "なまけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(633, "モノズ", "", new uint[] { 52, 65, 50, 45, 50, 38 }, (PokeType.Dark, PokeType.Dragon), new string[] { "はりきり", "はりきり", "はりきり" }, GenderRatio.M1F1));
            dexData.Add(new Species(634, "ジヘッド", "", new uint[] { 72, 85, 70, 65, 70, 58 }, (PokeType.Dark, PokeType.Dragon), new string[] { "はりきり", "はりきり", "はりきり" }, GenderRatio.M1F1));
            dexData.Add(new Species(635, "サザンドラ", "", new uint[] { 92, 105, 90, 125, 90, 98 }, (PokeType.Dark, PokeType.Dragon), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(636, "メラルバ", "", new uint[] { 55, 85, 55, 50, 55, 60 }, (PokeType.Bug, PokeType.Fire), new string[] { "ほのおのからだ", "ほのおのからだ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(637, "ウルガモス", "", new uint[] { 85, 60, 65, 135, 105, 100 }, (PokeType.Bug, PokeType.Fire), new string[] { "ほのおのからだ", "ほのおのからだ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(638, "コバルオン", "", new uint[] { 91, 90, 129, 90, 72, 108 }, (PokeType.Steel, PokeType.Fighting), new string[] { "せいぎのこころ", "せいぎのこころ", "せいぎのこころ" }, GenderRatio.Genderless));
            dexData.Add(new Species(639, "テラキオン", "", new uint[] { 91, 129, 90, 72, 90, 108 }, (PokeType.Rock, PokeType.Fighting), new string[] { "せいぎのこころ", "せいぎのこころ", "せいぎのこころ" }, GenderRatio.Genderless));
            dexData.Add(new Species(640, "ビリジオン", "", new uint[] { 91, 90, 72, 90, 129, 108 }, (PokeType.Grass, PokeType.Fighting), new string[] { "せいぎのこころ", "せいぎのこころ", "せいぎのこころ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(641, "トルネロス", "化身", new uint[] { 79, 115, 70, 125, 80, 111 }, (PokeType.Flying, PokeType.Non), new string[] { "いたずらごころ", "いたずらごころ", "まけんき" }, GenderRatio.MaleOnly));
            dexData.Add(new AnotherForm(641, "トルネロス", "霊獣", new uint[] { 79, 100, 80, 110, 90, 121 }, (PokeType.Flying, PokeType.Non), new string[] { "さいせいりょく", "さいせいりょく", "さいせいりょく" }, GenderRatio.MaleOnly));
            dexData.Add(new AnotherForm(642, "ボルトロス", "化身", new uint[] { 79, 115, 70, 125, 80, 111 }, (PokeType.Electric, PokeType.Flying), new string[] { "いたずらごころ", "いたずらごころ", "まけんき" }, GenderRatio.MaleOnly));
            dexData.Add(new AnotherForm(642, "ボルトロス", "霊獣", new uint[] { 79, 105, 70, 145, 80, 101 }, (PokeType.Electric, PokeType.Flying), new string[] { "ちくでん", "ちくでん", "ちくでん" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(643, "レシラム", "", new uint[] { 100, 120, 100, 150, 120, 90 }, (PokeType.Dragon, PokeType.Fire), new string[] { "ターボブレイズ", "ターボブレイズ", "ターボブレイズ" }, GenderRatio.Genderless));
            dexData.Add(new Species(644, "ゼクロム", "", new uint[] { 100, 150, 120, 120, 100, 90 }, (PokeType.Dragon, PokeType.Electric), new string[] { "テラボルテージ", "テラボルテージ", "テラボルテージ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(645, "ランドロス", "化身", new uint[] { 89, 125, 90, 115, 80, 101 }, (PokeType.Ground, PokeType.Flying), new string[] { "すなのちから", "すなのちから", "ちからずく" }, GenderRatio.MaleOnly));
            dexData.Add(new AnotherForm(645, "ランドロス", "霊獣", new uint[] { 89, 145, 90, 105, 80, 91 }, (PokeType.Ground, PokeType.Flying), new string[] { "いかく", "いかく", "いかく" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(646, "キュレム", "", new uint[] { 125, 130, 90, 130, 90, 95 }, (PokeType.Dragon, PokeType.Ice), new string[] { "プレッシャー", "プレッシャー", "プレッシャー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(646, "キュレム", "ブラック", new uint[] { 125, 120, 90, 170, 100, 95 }, (PokeType.Dragon, PokeType.Ice), new string[] { "ターボブレイズ", "ターボブレイズ", "ターボブレイズ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(646, "キュレム", "ホワイト", new uint[] { 125, 170, 100, 120, 90, 95 }, (PokeType.Dragon, PokeType.Ice), new string[] { "テラボルテージ", "テラボルテージ", "テラボルテージ" }, GenderRatio.Genderless));
            dexData.Add(new Species(647, "ケルディオ", "", new uint[] { 91, 72, 90, 129, 90, 108 }, (PokeType.Water, PokeType.Fighting), new string[] { "せいぎのこころ", "せいぎのこころ", "せいぎのこころ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(648, "メロエッタ", "ボイス", new uint[] { 100, 77, 77, 128, 128, 90 }, (PokeType.Normal, PokeType.Psychic), new string[] { "てんのめぐみ", "てんのめぐみ", "てんのめぐみ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(648, "メロエッタ", "ステップ", new uint[] { 100, 128, 90, 77, 77, 128 }, (PokeType.Normal, PokeType.Fighting), new string[] { "てんのめぐみ", "てんのめぐみ", "てんのめぐみ" }, GenderRatio.Genderless));
            dexData.Add(new Species(649, "ゲノセクト", "", new uint[] { 71, 120, 95, 120, 95, 99 }, (PokeType.Bug, PokeType.Steel), new string[] { "ダウンロード", "ダウンロード", "ダウンロード" }, GenderRatio.Genderless));
            dexData.Add(new Species(650, "ハリマロン", "", new uint[] { 56, 61, 65, 48, 45, 38 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "ぼうだん" }, GenderRatio.M7F1));
            dexData.Add(new Species(651, "ハリボーグ", "", new uint[] { 61, 78, 95, 56, 58, 57 }, (PokeType.Grass, PokeType.Non), new string[] { "しんりょく", "しんりょく", "ぼうだん" }, GenderRatio.M7F1));
            dexData.Add(new Species(652, "ブリガロン", "", new uint[] { 88, 107, 122, 74, 75, 64 }, (PokeType.Grass, PokeType.Fighting), new string[] { "しんりょく", "しんりょく", "ぼうだん" }, GenderRatio.M7F1));
            dexData.Add(new Species(653, "フォッコ", "", new uint[] { 40, 45, 40, 62, 60, 60 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "マジシャン" }, GenderRatio.M7F1));
            dexData.Add(new Species(654, "テールナー", "", new uint[] { 59, 59, 58, 90, 70, 73 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "マジシャン" }, GenderRatio.M7F1));
            dexData.Add(new Species(655, "マフォクシー", "", new uint[] { 75, 69, 72, 114, 100, 104 }, (PokeType.Fire, PokeType.Psychic), new string[] { "もうか", "もうか", "マジシャン" }, GenderRatio.M7F1));
            dexData.Add(new Species(656, "ケロマツ", "", new uint[] { 41, 56, 40, 62, 44, 71 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "へんげんじざい" }, GenderRatio.M7F1));
            dexData.Add(new Species(657, "ゲコガシラ", "", new uint[] { 54, 63, 52, 83, 56, 97 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "へんげんじざい" }, GenderRatio.M7F1));
            dexData.Add(new Species(658, "ゲッコウガ", "", new uint[] { 72, 95, 67, 103, 71, 122 }, (PokeType.Water, PokeType.Dark), new string[] { "げきりゅう", "げきりゅう", "へんげんじざい" }, GenderRatio.M7F1));
            dexData.Add(new AnotherForm(658, "ゲッコウガ", "サトシ", new uint[] { 72, 145, 67, 153, 71, 132 }, (PokeType.Water, PokeType.Dark), new string[] { "きずなへんげ", "きずなへんげ", "きずなへんげ" }, GenderRatio.MaleOnly));
            dexData.Add(new Species(659, "ホルビー", "", new uint[] { 38, 36, 38, 32, 36, 57 }, (PokeType.Normal, PokeType.Non), new string[] { "ものひろい", "ほおぶくろ", "ちからもち" }, GenderRatio.M1F1));
            dexData.Add(new Species(660, "ホルード", "", new uint[] { 85, 56, 77, 50, 77, 78 }, (PokeType.Normal, PokeType.Ground), new string[] { "ものひろい", "ほおぶくろ", "ちからもち" }, GenderRatio.M1F1));
            dexData.Add(new Species(661, "ヤヤコマ", "", new uint[] { 45, 50, 43, 40, 38, 62 }, (PokeType.Normal, PokeType.Flying), new string[] { "はとむね", "はとむね", "はやてのつばさ" }, GenderRatio.M1F1));
            dexData.Add(new Species(662, "ヒノヤコマ", "", new uint[] { 62, 73, 55, 56, 52, 84 }, (PokeType.Fire, PokeType.Flying), new string[] { "ほのおのからだ", "ほのおのからだ", "はやてのつばさ" }, GenderRatio.M1F1));
            dexData.Add(new Species(663, "ファイアロー", "", new uint[] { 78, 81, 71, 74, 69, 126 }, (PokeType.Fire, PokeType.Flying), new string[] { "ほのおのからだ", "ほのおのからだ", "はやてのつばさ" }, GenderRatio.M1F1));
            dexData.Add(new Species(664, "コフキムシ", "", new uint[] { 38, 35, 40, 27, 25, 35 }, (PokeType.Bug, PokeType.Non), new string[] { "りんぷん", "ふくがん", "フレンドガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(665, "コフーライ", "", new uint[] { 45, 22, 60, 27, 30, 29 }, (PokeType.Bug, PokeType.Non), new string[] { "だっぴ", "だっぴ", "フレンドガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(666, "ビビヨン", "", new uint[] { 80, 52, 50, 90, 50, 89 }, (PokeType.Bug, PokeType.Flying), new string[] { "りんぷん", "ふくがん", "フレンドガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(667, "シシコ", "", new uint[] { 62, 50, 58, 73, 54, 72 }, (PokeType.Fire, PokeType.Normal), new string[] { "とうそうしん", "きんちょうかん", "じしんかじょう" }, GenderRatio.M1F7));
            dexData.Add(new Species(668, "カエンジシ", "", new uint[] { 86, 68, 72, 109, 66, 106 }, (PokeType.Fire, PokeType.Normal), new string[] { "とうそうしん", "きんちょうかん", "じしんかじょう" }, GenderRatio.M1F7));
            dexData.Add(new Species(669, "フラベベ", "", new uint[] { 44, 38, 39, 61, 79, 42 }, (PokeType.Fairy, PokeType.Non), new string[] { "フラワーベール", "フラワーベール", "きょうせい" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(670, "フラエッテ", "", new uint[] { 54, 45, 47, 75, 98, 52 }, (PokeType.Fairy, PokeType.Non), new string[] { "フラワーベール", "フラワーベール", "きょうせい" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(671, "フラージェス", "", new uint[] { 78, 65, 68, 112, 154, 75 }, (PokeType.Fairy, PokeType.Non), new string[] { "フラワーベール", "フラワーベール", "きょうせい" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(672, "メェークル", "", new uint[] { 66, 65, 48, 62, 57, 52 }, (PokeType.Grass, PokeType.Non), new string[] { "そうしょく", "そうしょく", "くさのけがわ" }, GenderRatio.M1F1));
            dexData.Add(new Species(673, "ゴーゴート", "", new uint[] { 123, 100, 62, 97, 81, 68 }, (PokeType.Grass, PokeType.Non), new string[] { "そうしょく", "そうしょく", "くさのけがわ" }, GenderRatio.M1F1));
            dexData.Add(new Species(674, "ヤンチャム", "", new uint[] { 67, 82, 62, 46, 48, 43 }, (PokeType.Fighting, PokeType.Non), new string[] { "てつのこぶし", "かたやぶり", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(675, "ゴロンダ", "", new uint[] { 95, 124, 78, 69, 71, 58 }, (PokeType.Fighting, PokeType.Dark), new string[] { "てつのこぶし", "かたやぶり", "きもったま" }, GenderRatio.M1F1));
            dexData.Add(new Species(676, "トリミアン", "", new uint[] { 75, 80, 60, 65, 90, 102 }, (PokeType.Normal, PokeType.Non), new string[] { "ファーコート", "ファーコート", "ファーコート" }, GenderRatio.M1F1));
            dexData.Add(new Species(677, "ニャスパー", "", new uint[] { 62, 48, 54, 63, 60, 68 }, (PokeType.Psychic, PokeType.Non), new string[] { "するどいめ", "すりぬけ", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(678, "ニャオニクス", "♂", new uint[] { 74, 48, 76, 83, 81, 104 }, (PokeType.Psychic, PokeType.Non), new string[] { "するどいめ", "すりぬけ", "いたずらごころ" }, GenderRatio.MaleOnly));
            dexData.Add(new AnotherForm(678, "ニャオニクス", "♀", new uint[] { 74, 48, 76, 83, 81, 104 }, (PokeType.Psychic, PokeType.Non), new string[] { "するどいめ", "すりぬけ", "かちき" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(679, "ヒトツキ", "", new uint[] { 45, 80, 100, 35, 37, 28 }, (PokeType.Steel, PokeType.Ghost), new string[] { "ノーガード", "ノーガード", "ノーガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(680, "ニダンギル", "", new uint[] { 59, 110, 150, 45, 49, 35 }, (PokeType.Steel, PokeType.Ghost), new string[] { "ノーガード", "ノーガード", "ノーガード" }, GenderRatio.M1F1));
            dexData.Add(new Species(681, "ギルガルド", "シールド", new uint[] { 60, 50, 150, 50, 150, 60 }, (PokeType.Steel, PokeType.Ghost), new string[] { "バトルスイッチ", "バトルスイッチ", "バトルスイッチ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(681, "ギルガルド", "ブレード", new uint[] { 60, 150, 50, 150, 50, 60 }, (PokeType.Steel, PokeType.Ghost), new string[] { "バトルスイッチ", "バトルスイッチ", "バトルスイッチ" }, GenderRatio.M1F1));
            dexData.Add(new Species(682, "シュシュプ", "", new uint[] { 78, 52, 60, 63, 65, 23 }, (PokeType.Fairy, PokeType.Non), new string[] { "いやしのこころ", "いやしのこころ", "アロマベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(683, "フレフワン", "", new uint[] { 101, 72, 72, 99, 89, 29 }, (PokeType.Fairy, PokeType.Non), new string[] { "いやしのこころ", "いやしのこころ", "アロマベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(684, "ペロッパフ", "", new uint[] { 62, 48, 66, 59, 57, 49 }, (PokeType.Fairy, PokeType.Non), new string[] { "スイートベール", "スイートベール", "かるわざ" }, GenderRatio.M1F1));
            dexData.Add(new Species(685, "ペロリーム", "", new uint[] { 82, 80, 86, 85, 75, 72 }, (PokeType.Fairy, PokeType.Non), new string[] { "スイートベール", "スイートベール", "かるわざ" }, GenderRatio.M1F1));
            dexData.Add(new Species(686, "マーイーカ", "", new uint[] { 53, 54, 53, 37, 46, 45 }, (PokeType.Dark, PokeType.Psychic), new string[] { "あまのじゃく", "きゅうばん", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(687, "カラマネロ", "", new uint[] { 86, 92, 88, 68, 75, 73 }, (PokeType.Dark, PokeType.Psychic), new string[] { "あまのじゃく", "きゅうばん", "すりぬけ" }, GenderRatio.M1F1));
            dexData.Add(new Species(688, "カメテテ", "", new uint[] { 42, 52, 67, 39, 56, 50 }, (PokeType.Rock, PokeType.Water), new string[] { "かたいツメ", "スナイパー", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(689, "ガメノデス", "", new uint[] { 72, 105, 115, 54, 86, 68 }, (PokeType.Rock, PokeType.Water), new string[] { "かたいツメ", "スナイパー", "わるいてぐせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(690, "クズモー", "", new uint[] { 50, 60, 60, 60, 60, 30 }, (PokeType.Poison, PokeType.Water), new string[] { "どくのトゲ", "どくしゅ", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(691, "ドラミドロ", "", new uint[] { 65, 75, 90, 97, 123, 44 }, (PokeType.Poison, PokeType.Dragon), new string[] { "どくのトゲ", "どくしゅ", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(692, "ウデッポウ", "", new uint[] { 50, 53, 62, 58, 63, 44 }, (PokeType.Water, PokeType.Non), new string[] { "メガランチャー", "メガランチャー", "メガランチャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(693, "ブロスター", "", new uint[] { 71, 73, 88, 120, 89, 59 }, (PokeType.Water, PokeType.Non), new string[] { "メガランチャー", "メガランチャー", "メガランチャー" }, GenderRatio.M1F1));
            dexData.Add(new Species(694, "エリキテル", "", new uint[] { 44, 38, 33, 61, 43, 70 }, (PokeType.Electric, PokeType.Normal), new string[] { "かんそうはだ", "すながくれ", "サンパワー" }, GenderRatio.M1F1));
            dexData.Add(new Species(695, "エレザード", "", new uint[] { 62, 55, 52, 109, 94, 109 }, (PokeType.Electric, PokeType.Normal), new string[] { "かんそうはだ", "すながくれ", "サンパワー" }, GenderRatio.M1F1));
            dexData.Add(new Species(696, "チゴラス", "", new uint[] { 58, 89, 77, 45, 45, 48 }, (PokeType.Rock, PokeType.Dragon), new string[] { "がんじょうあご", "がんじょうあご", "がんじょう" }, GenderRatio.M7F1));
            dexData.Add(new Species(697, "ガチゴラス", "", new uint[] { 82, 121, 119, 69, 59, 71 }, (PokeType.Rock, PokeType.Dragon), new string[] { "がんじょうあご", "がんじょうあご", "がんじょう" }, GenderRatio.M7F1));
            dexData.Add(new Species(698, "アマルス", "", new uint[] { 77, 59, 50, 67, 63, 46 }, (PokeType.Rock, PokeType.Ice), new string[] { "フリーズスキン", "フリーズスキン", "ゆきふらし" }, GenderRatio.M7F1));
            dexData.Add(new Species(699, "アマルルガ", "", new uint[] { 123, 77, 72, 99, 92, 58 }, (PokeType.Rock, PokeType.Ice), new string[] { "フリーズスキン", "フリーズスキン", "ゆきふらし" }, GenderRatio.M7F1));
            dexData.Add(new Species(700, "ニンフィア", "", new uint[] { 95, 65, 65, 110, 130, 60 }, (PokeType.Fairy, PokeType.Non), new string[] { "メロメロボディ", "メロメロボディ", "フェアリースキン" }, GenderRatio.M7F1));
            dexData.Add(new Species(701, "ルチャブル", "", new uint[] { 78, 92, 75, 74, 63, 118 }, (PokeType.Fighting, PokeType.Flying), new string[] { "じゅうなん", "かるわざ", "かたやぶり" }, GenderRatio.M1F1));
            dexData.Add(new Species(702, "デデンネ", "", new uint[] { 67, 58, 57, 81, 67, 101 }, (PokeType.Electric, PokeType.Fairy), new string[] { "ほおぶくろ", "ものひろい", "プラス" }, GenderRatio.M1F1));
            dexData.Add(new Species(703, "メレシー", "", new uint[] { 50, 50, 150, 50, 150, 50 }, (PokeType.Rock, PokeType.Fairy), new string[] { "クリアボディ", "クリアボディ", "がんじょう" }, GenderRatio.Genderless));
            dexData.Add(new Species(704, "ヌメラ", "", new uint[] { 45, 50, 35, 55, 75, 40 }, (PokeType.Dragon, PokeType.Non), new string[] { "そうしょく", "うるおいボディ", "ぬめぬめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(705, "ヌメイル", "", new uint[] { 68, 75, 53, 83, 113, 60 }, (PokeType.Dragon, PokeType.Non), new string[] { "そうしょく", "うるおいボディ", "ぬめぬめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(706, "ヌメルゴン", "", new uint[] { 90, 100, 70, 110, 150, 80 }, (PokeType.Dragon, PokeType.Non), new string[] { "そうしょく", "うるおいボディ", "ぬめぬめ" }, GenderRatio.M1F1));
            dexData.Add(new Species(707, "クレッフィ", "", new uint[] { 57, 80, 91, 80, 87, 75 }, (PokeType.Steel, PokeType.Fairy), new string[] { "いたずらごころ", "いたずらごころ", "マジシャン" }, GenderRatio.M1F1));
            dexData.Add(new Species(708, "ボクレー", "", new uint[] { 43, 70, 48, 50, 60, 38 }, (PokeType.Ghost, PokeType.Grass), new string[] { "しぜんかいふく", "おみとおし", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new Species(709, "オーロット", "", new uint[] { 85, 110, 76, 65, 82, 56 }, (PokeType.Ghost, PokeType.Grass), new string[] { "しぜんかいふく", "おみとおし", "しゅうかく" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(710, "バケッチャ", "普通", new uint[] { 49, 66, 70, 44, 55, 51 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(710, "バケッチャ", "小", new uint[] { 44, 66, 70, 44, 55, 56 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(710, "バケッチャ", "大", new uint[] { 54, 66, 70, 44, 55, 46 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(710, "バケッチャ", "特大", new uint[] { 59, 66, 70, 44, 55, 41 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(711, "パンプジン", "普通", new uint[] { 65, 90, 122, 58, 75, 84 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(711, "パンプジン", "小", new uint[] { 55, 85, 122, 58, 75, 99 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(711, "パンプジン", "大", new uint[] { 75, 95, 122, 58, 75, 69 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(711, "パンプジン", "特大", new uint[] { 85, 100, 122, 58, 75, 54 }, (PokeType.Ghost, PokeType.Grass), new string[] { "ものひろい", "おみとおし", "ふみん" }, GenderRatio.M1F1));
            dexData.Add(new Species(712, "カチコール", "", new uint[] { 55, 69, 85, 32, 35, 28 }, (PokeType.Ice, PokeType.Non), new string[] { "マイペース", "アイスボディ", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(713, "クレベース", "", new uint[] { 95, 117, 184, 44, 46, 28 }, (PokeType.Ice, PokeType.Non), new string[] { "マイペース", "アイスボディ", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(714, "オンバット", "", new uint[] { 40, 30, 35, 45, 40, 55 }, (PokeType.Flying, PokeType.Dragon), new string[] { "おみとおし", "すりぬけ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(715, "オンバーン", "", new uint[] { 85, 70, 80, 97, 80, 123 }, (PokeType.Flying, PokeType.Dragon), new string[] { "おみとおし", "すりぬけ", "テレパシー" }, GenderRatio.M1F1));
            dexData.Add(new Species(716, "ゼルネアス", "", new uint[] { 126, 131, 95, 131, 98, 99 }, (PokeType.Fairy, PokeType.Non), new string[] { "フェアリーオーラ", "フェアリーオーラ", "フェアリーオーラ" }, GenderRatio.Genderless));
            dexData.Add(new Species(717, "イベルタル", "", new uint[] { 126, 131, 95, 131, 98, 99 }, (PokeType.Dark, PokeType.Flying), new string[] { "ダークオーラ", "ダークオーラ", "ダークオーラ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(718, "ジガルデ", "50%", new uint[] { 108, 100, 121, 81, 95, 95 }, (PokeType.Dragon, PokeType.Ground), new string[] { "オーラブレイク", "オーラブレイク", "スワームチェンジ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(718, "ジガルデ", "10%", new uint[] { 54, 100, 71, 61, 85, 115 }, (PokeType.Dragon, PokeType.Ground), new string[] { "オーラブレイク", "オーラブレイク", "スワームチェンジ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(718, "ジガルデ", "パーフェクト", new uint[] { 216, 100, 121, 91, 95, 85 }, (PokeType.Dragon, PokeType.Ground), new string[] { "スワームチェンジ", "スワームチェンジ", "スワームチェンジ" }, GenderRatio.Genderless));
            dexData.Add(new Species(719, "ディアンシー", "", new uint[] { 50, 100, 150, 100, 150, 50 }, (PokeType.Rock, PokeType.Fairy), new string[] { "クリアボディ", "クリアボディ", "クリアボディ" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(720, "フーパ", "戒め", new uint[] { 80, 110, 60, 150, 130, 70 }, (PokeType.Psychic, PokeType.Ghost), new string[] { "マジシャン", "マジシャン", "マジシャン" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(720, "フーパ", "解放", new uint[] { 80, 160, 60, 170, 130, 80 }, (PokeType.Psychic, PokeType.Dark), new string[] { "マジシャン", "マジシャン", "マジシャン" }, GenderRatio.Genderless));
            dexData.Add(new Species(721, "ボルケニオン", "", new uint[] { 80, 110, 120, 130, 90, 70 }, (PokeType.Fire, PokeType.Water), new string[] { "ちょすい", "ちょすい", "ちょすい" }, GenderRatio.Genderless));
            dexData.Add(new Species(722, "モクロー", "", new uint[] { 68, 55, 55, 50, 50, 42 }, (PokeType.Grass, PokeType.Flying), new string[] { "しんりょく", "しんりょく", "えんかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(723, "フクスロー", "", new uint[] { 78, 75, 75, 70, 70, 52 }, (PokeType.Grass, PokeType.Flying), new string[] { "しんりょく", "しんりょく", "えんかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(724, "ジュナイパー", "", new uint[] { 78, 107, 75, 100, 100, 70 }, (PokeType.Grass, PokeType.Ghost), new string[] { "しんりょく", "しんりょく", "えんかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(725, "ニャビー", "", new uint[] { 45, 65, 40, 60, 40, 70 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "いかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(726, "ニャヒート", "", new uint[] { 65, 85, 50, 80, 50, 90 }, (PokeType.Fire, PokeType.Non), new string[] { "もうか", "もうか", "いかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(727, "ガオガエン", "", new uint[] { 95, 115, 90, 80, 90, 60 }, (PokeType.Fire, PokeType.Dark), new string[] { "もうか", "もうか", "いかく" }, GenderRatio.M7F1));
            dexData.Add(new Species(728, "アシマリ", "", new uint[] { 50, 54, 54, 66, 56, 40 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "うるおいボイス" }, GenderRatio.M7F1));
            dexData.Add(new Species(729, "オシャマリ", "", new uint[] { 60, 69, 69, 91, 81, 50 }, (PokeType.Water, PokeType.Non), new string[] { "げきりゅう", "げきりゅう", "うるおいボイス" }, GenderRatio.M7F1));
            dexData.Add(new Species(730, "アシレーヌ", "", new uint[] { 80, 74, 74, 126, 116, 60 }, (PokeType.Water, PokeType.Fairy), new string[] { "げきりゅう", "げきりゅう", "うるおいボイス" }, GenderRatio.M7F1));
            dexData.Add(new Species(731, "ツツケラ", "", new uint[] { 35, 75, 30, 30, 30, 65 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "スキルリンク", "ものひろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(732, "ケララッパ", "", new uint[] { 55, 85, 50, 40, 50, 75 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "スキルリンク", "ものひろい" }, GenderRatio.M1F1));
            dexData.Add(new Species(733, "ドデカバシ", "", new uint[] { 80, 120, 75, 75, 75, 60 }, (PokeType.Normal, PokeType.Flying), new string[] { "するどいめ", "スキルリンク", "ちからずく" }, GenderRatio.M1F1));
            dexData.Add(new Species(734, "ヤングース", "", new uint[] { 48, 70, 30, 30, 30, 45 }, (PokeType.Normal, PokeType.Non), new string[] { "はりこみ", "がんじょうあご", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(735, "デカグース", "", new uint[] { 88, 110, 60, 55, 60, 45 }, (PokeType.Normal, PokeType.Non), new string[] { "はりこみ", "がんじょうあご", "てきおうりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(736, "アゴジムシ", "", new uint[] { 47, 62, 45, 55, 45, 46 }, (PokeType.Bug, PokeType.Non), new string[] { "むしのしらせ", "むしのしらせ", "むしのしらせ" }, GenderRatio.M1F1));
            dexData.Add(new Species(737, "デンヂムシ", "", new uint[] { 57, 82, 95, 55, 75, 36 }, (PokeType.Bug, PokeType.Electric), new string[] { "バッテリー", "バッテリー", "バッテリー" }, GenderRatio.M1F1));
            dexData.Add(new Species(738, "クワガノン", "", new uint[] { 77, 70, 90, 145, 75, 43 }, (PokeType.Bug, PokeType.Electric), new string[] { "ふゆう", "ふゆう", "ふゆう" }, GenderRatio.M1F1));
            dexData.Add(new Species(739, "マケンカニ", "", new uint[] { 47, 82, 57, 42, 47, 63 }, (PokeType.Fighting, PokeType.Non), new string[] { "かいりきバサミ", "てつのこぶし", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new Species(740, "ケケンカニ", "", new uint[] { 97, 132, 77, 62, 67, 43 }, (PokeType.Fighting, PokeType.Ice), new string[] { "かいりきバサミ", "てつのこぶし", "いかりのつぼ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(741, "オドリドリ", "めらめら", new uint[] { 75, 70, 70, 98, 70, 93 }, (PokeType.Fire, PokeType.Flying), new string[] { "おどりこ", "おどりこ", "おどりこ" }, GenderRatio.M1F3));
            dexData.Add(new AnotherForm(741, "オドリドリ", "ぱちぱち", new uint[] { 75, 70, 70, 98, 70, 93 }, (PokeType.Electric, PokeType.Flying), new string[] { "おどりこ", "おどりこ", "おどりこ" }, GenderRatio.M1F3));
            dexData.Add(new AnotherForm(741, "オドリドリ", "ふらふら", new uint[] { 75, 70, 70, 98, 70, 93 }, (PokeType.Psychic, PokeType.Flying), new string[] { "おどりこ", "おどりこ", "おどりこ" }, GenderRatio.M1F3));
            dexData.Add(new AnotherForm(741, "オドリドリ", "まいまい", new uint[] { 75, 70, 70, 98, 70, 93 }, (PokeType.Ghost, PokeType.Flying), new string[] { "おどりこ", "おどりこ", "おどりこ" }, GenderRatio.M1F3));
            dexData.Add(new Species(742, "アブリー", "", new uint[] { 40, 45, 40, 55, 40, 84 }, (PokeType.Bug, PokeType.Fairy), new string[] { "みつあつめ", "りんぷん", "スイートベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(743, "アブリボン", "", new uint[] { 60, 55, 60, 95, 70, 124 }, (PokeType.Bug, PokeType.Fairy), new string[] { "みつあつめ", "りんぷん", "スイートベール" }, GenderRatio.M1F1));
            dexData.Add(new Species(744, "イワンコ", "", new uint[] { 45, 65, 40, 30, 40, 60 }, (PokeType.Rock, PokeType.Non), new string[] { "するどいめ", "やるき", "ふくつのこころ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(744, "イワンコ", "夕", new uint[] { 45, 65, 40, 30, 40, 60 }, (PokeType.Rock, PokeType.Non), new string[] { "マイペース", "マイペース", "マイペース" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(745, "ルガルガン", "昼", new uint[] { 75, 115, 65, 55, 65, 112 }, (PokeType.Rock, PokeType.Non), new string[] { "するどいめ", "すなかき", "ふくつのこころ" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(745, "ルガルガン", "夜", new uint[] { 85, 115, 75, 55, 75, 82 }, (PokeType.Rock, PokeType.Non), new string[] { "するどいめ", "やるき", "ノーガード" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(745, "ルガルガン", "夕", new uint[] { 75, 117, 65, 55, 65, 110 }, (PokeType.Rock, PokeType.Non), new string[] { "かたいツメ", "かたいツメ", "かたいツメ" }, GenderRatio.M1F1));
            dexData.Add(new Species(746, "ヨワシ", "単独", new uint[] { 45, 20, 20, 25, 25, 40 }, (PokeType.Water, PokeType.Non), new string[] { "ぎょぐん", "ぎょぐん", "ぎょぐん" }, GenderRatio.M1F1));
            dexData.Add(new AnotherForm(746, "ヨワシ", "群れ", new uint[] { 45, 140, 130, 140, 135, 30 }, (PokeType.Water, PokeType.Non), new string[] { "ぎょぐん", "ぎょぐん", "ぎょぐん" }, GenderRatio.M1F1));
            dexData.Add(new Species(747, "ヒドイデ", "", new uint[] { 50, 53, 62, 43, 52, 45 }, (PokeType.Poison, PokeType.Water), new string[] { "ひとでなし", "じゅうなん", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(748, "ドヒドイデ", "", new uint[] { 50, 63, 152, 53, 142, 35 }, (PokeType.Poison, PokeType.Water), new string[] { "ひとでなし", "じゅうなん", "さいせいりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(749, "ドロバンコ", "", new uint[] { 70, 100, 70, 45, 55, 45 }, (PokeType.Ground, PokeType.Non), new string[] { "マイペース", "じきゅうりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(750, "バンバドロ", "", new uint[] { 100, 125, 100, 55, 85, 35 }, (PokeType.Ground, PokeType.Non), new string[] { "マイペース", "じきゅうりょく", "せいしんりょく" }, GenderRatio.M1F1));
            dexData.Add(new Species(751, "シズクモ", "", new uint[] { 38, 40, 52, 40, 72, 27 }, (PokeType.Water, PokeType.Bug), new string[] { "すいほう", "すいほう", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(752, "オニシズクモ", "", new uint[] { 68, 70, 92, 50, 132, 42 }, (PokeType.Water, PokeType.Bug), new string[] { "すいほう", "すいほう", "ちょすい" }, GenderRatio.M1F1));
            dexData.Add(new Species(753, "カリキリ", "", new uint[] { 40, 55, 35, 50, 35, 35 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "リーフガード", "あまのじゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(754, "ラランテス", "", new uint[] { 70, 105, 90, 80, 90, 45 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "リーフガード", "あまのじゃく" }, GenderRatio.M1F1));
            dexData.Add(new Species(755, "ネマシュ", "", new uint[] { 40, 35, 55, 65, 75, 15 }, (PokeType.Grass, PokeType.Fairy), new string[] { "はっこう", "ほうし", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(756, "マシェード", "", new uint[] { 60, 45, 80, 90, 100, 30 }, (PokeType.Grass, PokeType.Fairy), new string[] { "はっこう", "ほうし", "あめうけざら" }, GenderRatio.M1F1));
            dexData.Add(new Species(757, "ヤトウモリ", "", new uint[] { 48, 44, 40, 71, 40, 77 }, (PokeType.Poison, PokeType.Fire), new string[] { "ふしょく", "ふしょく", "どんかん" }, GenderRatio.M7F1));
            dexData.Add(new Species(758, "エンニュート", "", new uint[] { 68, 64, 60, 111, 60, 117 }, (PokeType.Poison, PokeType.Fire), new string[] { "ふしょく", "ふしょく", "どんかん" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(759, "ヌイコグマ", "", new uint[] { 70, 75, 50, 45, 50, 50 }, (PokeType.Normal, PokeType.Fighting), new string[] { "もふもふ", "ぶきよう", "メロメロボディ" }, GenderRatio.M1F1));
            dexData.Add(new Species(760, "キテルグマ", "", new uint[] { 120, 125, 80, 55, 60, 60 }, (PokeType.Normal, PokeType.Fighting), new string[] { "もふもふ", "ぶきよう", "きんちょうかん" }, GenderRatio.M1F1));
            dexData.Add(new Species(761, "アマカジ", "", new uint[] { 42, 30, 38, 30, 38, 32 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "どんかん", "スイートベール" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(762, "アママイコ", "", new uint[] { 52, 40, 48, 40, 48, 62 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "どんかん", "スイートベール" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(763, "アマージョ", "", new uint[] { 72, 120, 98, 50, 98, 72 }, (PokeType.Grass, PokeType.Non), new string[] { "リーフガード", "じょおうのいげん", "スイートベール" }, GenderRatio.FemaleOnly));
            dexData.Add(new Species(764, "キュワワー", "", new uint[] { 51, 52, 90, 82, 110, 100 }, (PokeType.Fairy, PokeType.Non), new string[] { "フラワーベール", "ヒーリングシフト", "しぜんかいふく" }, GenderRatio.M1F3));
            dexData.Add(new Species(765, "ヤレユータン", "", new uint[] { 90, 60, 80, 90, 110, 60 }, (PokeType.Normal, PokeType.Psychic), new string[] { "せいしんりょく", "テレパシー", "きょうせい" }, GenderRatio.M1F1));
            dexData.Add(new Species(766, "ナゲツケサル", "", new uint[] { 100, 120, 90, 40, 60, 80 }, (PokeType.Fighting, PokeType.Non), new string[] { "レシーバー", "レシーバー", "まけんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(767, "コソクムシ", "", new uint[] { 25, 35, 40, 20, 30, 80 }, (PokeType.Bug, PokeType.Water), new string[] { "にげごし", "にげごし", "にげごし" }, GenderRatio.M1F1));
            dexData.Add(new Species(768, "グソクムシャ", "", new uint[] { 75, 125, 140, 60, 90, 40 }, (PokeType.Bug, PokeType.Water), new string[] { "ききかいひ", "ききかいひ", "ききかいひ" }, GenderRatio.M1F1));
            dexData.Add(new Species(769, "スナバァ", "", new uint[] { 55, 55, 80, 70, 45, 15 }, (PokeType.Ghost, PokeType.Ground), new string[] { "みずがため", "みずがため", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(770, "シロデスナ", "", new uint[] { 85, 75, 110, 100, 75, 35 }, (PokeType.Ghost, PokeType.Ground), new string[] { "みずがため", "みずがため", "すながくれ" }, GenderRatio.M1F1));
            dexData.Add(new Species(771, "ナマコブシ", "", new uint[] { 55, 60, 130, 30, 130, 5 }, (PokeType.Water, PokeType.Non), new string[] { "とびだすなかみ", "とびだすなかみ", "てんねん" }, GenderRatio.M1F1));
            dexData.Add(new Species(772, "タイプ:ヌル", "", new uint[] { 95, 95, 95, 95, 95, 59 }, (PokeType.Normal, PokeType.Non), new string[] { "カブトアーマー", "カブトアーマー", "カブトアーマー" }, GenderRatio.Genderless));
            dexData.Add(new Species(773, "シルヴァディ", "", new uint[] { 95, 95, 95, 95, 95, 95 }, (PokeType.Normal, PokeType.Non), new string[] { "ARシステム", "ARシステム", "ARシステム" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(774, "メテノ", "流星", new uint[] { 60, 60, 100, 60, 100, 60 }, (PokeType.Rock, PokeType.Flying), new string[] { "リミットシールド", "リミットシールド", "リミットシールド" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(774, "メテノ", "コア", new uint[] { 60, 100, 60, 100, 60, 120 }, (PokeType.Rock, PokeType.Flying), new string[] { "リミットシールド", "リミットシールド", "リミットシールド" }, GenderRatio.Genderless));
            dexData.Add(new Species(775, "ネッコアラ", "", new uint[] { 65, 115, 65, 75, 95, 65 }, (PokeType.Normal, PokeType.Non), new string[] { "ぜったいねむり", "ぜったいねむり", "ぜったいねむり" }, GenderRatio.M1F1));
            dexData.Add(new Species(776, "バクガメス", "", new uint[] { 60, 78, 135, 91, 85, 36 }, (PokeType.Fire, PokeType.Dragon), new string[] { "シェルアーマー", "シェルアーマー", "シェルアーマー" }, GenderRatio.M1F1));
            dexData.Add(new Species(777, "トゲデマル", "", new uint[] { 65, 98, 63, 40, 73, 96 }, (PokeType.Electric, PokeType.Steel), new string[] { "てつのトゲ", "ひらいしん", "がんじょう" }, GenderRatio.M1F1));
            dexData.Add(new Species(778, "ミミッキュ", "", new uint[] { 55, 90, 80, 50, 105, 96 }, (PokeType.Ghost, PokeType.Fairy), new string[] { "ばけのかわ", "ばけのかわ", "ばけのかわ" }, GenderRatio.M1F1));
            dexData.Add(new Species(779, "ハギギシリ", "", new uint[] { 68, 105, 70, 70, 70, 92 }, (PokeType.Water, PokeType.Psychic), new string[] { "ビビッドボディ", "がんじょうあご", "ミラクルスキン" }, GenderRatio.M1F1));
            dexData.Add(new Species(780, "ジジーロン", "", new uint[] { 78, 60, 85, 135, 91, 36 }, (PokeType.Normal, PokeType.Dragon), new string[] { "ぎゃくじょう", "そうしょく", "ノーてんき" }, GenderRatio.M1F1));
            dexData.Add(new Species(781, "ダダリン", "", new uint[] { 70, 131, 100, 86, 90, 40 }, (PokeType.Ghost, PokeType.Grass), new string[] { "はがねつかい", "はがねつかい", "はがねつかい" }, GenderRatio.Genderless));
            dexData.Add(new Species(782, "ジャラコ", "", new uint[] { 45, 55, 65, 45, 45, 45 }, (PokeType.Dragon, PokeType.Non), new string[] { "ぼうだん", "ぼうおん", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(783, "ジャランゴ", "", new uint[] { 55, 75, 90, 65, 70, 65 }, (PokeType.Dragon, PokeType.Fighting), new string[] { "ぼうだん", "ぼうおん", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(784, "ジャラランガ", "", new uint[] { 75, 110, 125, 100, 105, 85 }, (PokeType.Dragon, PokeType.Fighting), new string[] { "ぼうだん", "ぼうおん", "ぼうじん" }, GenderRatio.M1F1));
            dexData.Add(new Species(785, "カプ・コケコ", "", new uint[] { 70, 115, 85, 95, 75, 130 }, (PokeType.Electric, PokeType.Fairy), new string[] { "エレキメイカー", "エレキメイカー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(786, "カプ・テテフ", "", new uint[] { 70, 85, 75, 130, 115, 95 }, (PokeType.Psychic, PokeType.Fairy), new string[] { "サイコメイカー", "サイコメイカー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(787, "カプ・ブルル", "", new uint[] { 70, 130, 115, 85, 95, 75 }, (PokeType.Grass, PokeType.Fairy), new string[] { "グラスメイカー", "グラスメイカー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(788, "カプ・レヒレ", "", new uint[] { 70, 75, 115, 95, 130, 85 }, (PokeType.Water, PokeType.Fairy), new string[] { "ミストメイカー", "ミストメイカー", "テレパシー" }, GenderRatio.Genderless));
            dexData.Add(new Species(789, "コスモッグ", "", new uint[] { 43, 29, 31, 29, 31, 37 }, (PokeType.Psychic, PokeType.Non), new string[] { "てんねん", "てんねん", "てんねん" }, GenderRatio.Genderless));
            dexData.Add(new Species(790, "コスモウム", "", new uint[] { 43, 29, 131, 29, 131, 37 }, (PokeType.Psychic, PokeType.Non), new string[] { "がんじょう", "がんじょう", "がんじょう" }, GenderRatio.Genderless));
            dexData.Add(new Species(791, "ソルガレオ", "", new uint[] { 137, 137, 107, 113, 89, 97 }, (PokeType.Psychic, PokeType.Steel), new string[] { "メタルプロテクト", "メタルプロテクト", "メタルプロテクト" }, GenderRatio.Genderless));
            dexData.Add(new Species(792, "ルナアーラ", "", new uint[] { 137, 113, 89, 137, 107, 97 }, (PokeType.Psychic, PokeType.Ghost), new string[] { "ファントムガード", "ファントムガード", "ファントムガード" }, GenderRatio.Genderless));
            dexData.Add(new Species(793, "ウツロイド", "", new uint[] { 109, 53, 47, 127, 131, 103 }, (PokeType.Rock, PokeType.Poison), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(794, "マッシブーン", "", new uint[] { 107, 139, 139, 53, 53, 79 }, (PokeType.Bug, PokeType.Fighting), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(795, "フェローチェ", "", new uint[] { 71, 137, 37, 137, 37, 151 }, (PokeType.Bug, PokeType.Fighting), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(796, "デンジュモク", "", new uint[] { 83, 89, 71, 173, 71, 83 }, (PokeType.Electric, PokeType.Non), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(797, "テッカグヤ", "", new uint[] { 97, 101, 103, 107, 101, 61 }, (PokeType.Steel, PokeType.Flying), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(798, "カミツルギ", "", new uint[] { 59, 181, 131, 59, 31, 109 }, (PokeType.Grass, PokeType.Steel), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(799, "アクジキング", "", new uint[] { 223, 101, 53, 97, 53, 43 }, (PokeType.Dark, PokeType.Dragon), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(800, "ネクロズマ", "", new uint[] { 97, 107, 101, 127, 89, 79 }, (PokeType.Psychic, PokeType.Non), new string[] { "プリズムアーマー", "プリズムアーマー", "プリズムアーマー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(800, "ネクロズマ", "たそがれ", new uint[] { 97, 157, 127, 113, 109, 77 }, (PokeType.Psychic, PokeType.Steel), new string[] { "プリズムアーマー", "プリズムアーマー", "プリズムアーマー" }, GenderRatio.Genderless));
            dexData.Add(new AnotherForm(800, "ネクロズマ", "あかつき", new uint[] { 97, 113, 109, 157, 127, 77 }, (PokeType.Psychic, PokeType.Ghost), new string[] { "プリズムアーマー", "プリズムアーマー", "プリズムアーマー" }, GenderRatio.Genderless));
            dexData.Add(new Species(801, "マギアナ", "", new uint[] { 80, 95, 115, 130, 115, 65 }, (PokeType.Steel, PokeType.Fairy), new string[] { "ソウルハート", "ソウルハート", "ソウルハート" }, GenderRatio.Genderless));
            dexData.Add(new Species(802, "マーシャドー", "", new uint[] { 90, 125, 80, 90, 90, 125 }, (PokeType.Fighting, PokeType.Ghost), new string[] { "テクニシャン", "テクニシャン", "テクニシャン" }, GenderRatio.Genderless));
            dexData.Add(new Species(803, "ベベノム", "", new uint[] { 67, 73, 67, 73, 67, 73 }, (PokeType.Poison, PokeType.Non), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(804, "アーゴヨン", "", new uint[] { 73, 73, 73, 127, 73, 121 }, (PokeType.Poison, PokeType.Dragon), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(805, "ツンデツンデ", "", new uint[] { 61, 131, 211, 53, 101, 13 }, (PokeType.Rock, PokeType.Steel), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(806, "ズガドーン", "", new uint[] { 53, 127, 53, 151, 79, 107 }, (PokeType.Fire, PokeType.Ghost), new string[] { "ビーストブースト", "ビーストブースト", "ビーストブースト" }, GenderRatio.Genderless));
            dexData.Add(new Species(807, "ゼラオラ", "", new uint[] { 88, 112, 75, 102, 80, 143 }, (PokeType.Electric, PokeType.Non), new string[] { "ちくでん", "ちくでん", "ちくでん" }, GenderRatio.Genderless));
            // DexDataを図鑑番号でDistinctする

            uniqueList = dexData.Distinct(new SpeciesComparer()).ToArray();
            uniqueDex = uniqueList.ToDictionary(_ => _.Name, _ => _);
            dexDictionary = dexData.ToDictionary(_ => _.Name + _.FormName, _ => _);
            formDex = dexData.ToLookup(_ => _.Name);
        }
        class SpeciesComparer : IEqualityComparer<Species>
        {
            public bool Equals(Species x, Species y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Name == y.Name;
            }

            public int GetHashCode(Species s) => s.Name.GetHashCode();
        }
    }
}
