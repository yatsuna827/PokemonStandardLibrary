using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using PokemonStandardLibrary.PokeDex.Gen7;

using static UnitTest.GenderRatioData;

namespace UnitTest.Gen7
{
    [TestClass]
    public class PokeDexTest
    {
        [TestMethod]
        public void RatioMaleOnly()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.MaleOnly)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                MaleOnly.Gen1,
                MaleOnly.Gen2,
                MaleOnly.Gen3,
                MaleOnly.Gen4,
                MaleOnly.Gen5,
                MaleOnly.Gen6,
                MaleOnly.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach(var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach(var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
        [TestMethod]
        public void RatioM7F1()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.M7F1)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                M7F1.Gen1,
                M7F1.Gen2,
                M7F1.Gen3,
                M7F1.Gen4,
                M7F1.Gen5,
                M7F1.Gen6,
                M7F1.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach (var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach (var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
        [TestMethod]
        public void RatioM3F1()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.M3F1)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                M3F1.Gen1,
                M3F1.Gen2,
                M3F1.Gen3,
                M3F1.Gen4,
                M3F1.Gen5,
                M3F1.Gen6,
                M3F1.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach (var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach (var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
        [TestMethod]
        public void RatioFemaleOnly()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.FemaleOnly)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                FemaleOnly.Gen1,
                FemaleOnly.Gen2,
                FemaleOnly.Gen3,
                FemaleOnly.Gen4,
                FemaleOnly.Gen5,
                FemaleOnly.Gen6,
                FemaleOnly.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach (var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach (var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
        [TestMethod]
        public void RatioM1F3()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.M1F3)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                M1F3.Gen1,
                M1F3.Gen2,
                M1F3.Gen3,
                M1F3.Gen4,
                M1F3.Gen5,
                M1F3.Gen6,
                M1F3.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach (var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach (var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
        [TestMethod]
        public void RatioGenderless()
        {
            var sample = Pokemon.GetAllPokemonList()
                .Where(_ => _.GenderRatio == PokemonStandardLibrary.GenderRatio.Genderless)
                .Select(_ => _.Name);

            var dataSet = new string[][]
            {
                Genderless.Gen1,
                Genderless.Gen2,
                Genderless.Gen3,
                Genderless.Gen4,
                Genderless.Gen5,
                Genderless.Gen6,
                Genderless.Gen7,
            }.SelectMany(_ => _);

            // 含まれているべきデータが含まれているか
            foreach (var data in dataSet)
            {
                Assert.IsTrue(sample.Contains(data), data);
            }

            // 余計なデータが含まれていないか
            foreach (var data in sample)
            {
                Assert.IsTrue(dataSet.Contains(data), data);
            }
        }
    }
}
