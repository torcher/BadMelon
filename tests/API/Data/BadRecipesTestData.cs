using BadMelon.API.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BadMelon.Tests.API.Data
{
    internal class BadRecipesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //No Ingredient
            yield return new object[] { new Recipe { Ingredients = null, Name = "Recipe", Steps = new List<Step> { new Step { Text = "Step" } } }, 400 };
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient>(), Name = "Recipe", Steps = new List<Step> { new Step { Text = "Step" } } }, 400 };

            //Ingredient weight negative
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = -1d } }, Name = "Recipe 2", Steps = new List<Step> { new Step { Text = "Step" } } }, 400 };

            //No Step
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = 1d } }, Name = "Recipe 2", Steps = new List<Step> { } }, 400 };
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = 1d } }, Name = "Recipe 2", Steps = null }, 400 };

            //Step Name blank
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = 1d } }, Name = "Recipe 2", Steps = new List<Step> { new Step { Order = 1 } } }, 400 };

            //Name missing
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = 1d } }, Steps = new List<Step> { new Step { Text = "Step" } } }, 400 };
            //Name too long
            yield return new object[] { new Recipe { Ingredients = new List<Ingredient> { new Ingredient { TypeID = Guid.NewGuid(), Weight = 1d } }, Name = "9593199516209679025812797603920067942433621455368833154703437170471937299297781127980602521923182724452953737290086266565205124609848427976243158750278219274397709017226307603797532513381373598437755064013598652831874306489161967976346701269629263899266860778076587216174646479626471119261180024281929718965926005740858672727517165546966877083342118685973141067264724628583006473231518427874557358292244830209790241824539642743673302861746439473585655401605893750101013955710000474560450328181547217093086756835698135240338848604149417252151074430332527546736199726135346617274240448327969726741652193312365971442008155589824791781845393398488867445571780558480326897008940975294307096750544241574909361767709708226836314257345656795884735502552304775298056554019515354000829463324228302882039644887862371009278423895459720422140808872547260048850488372933303131205960373326182556282565156907941141218134728157097997172005489865152921424761492087059795530451631869289624522302909298102713951549087030", Steps = new List<Step> { new Step { Text = "Step" } } }, 400 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}