using BadMelon.Data.DTOs;
using System.Collections;
using System.Collections.Generic;

namespace BadMelon.Tests.Data
{
    internal class BadStepTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Step { }, 400 };
            yield return new object[] { new Step { Text = "Test", CookTime = "00:1000:10" }, 400 };
            yield return new object[] { new Step { Text = "Test", CookTime = "00-10-10" }, 400 };
            yield return new object[] { new Step { Text = "Test", CookTime = "lkjfsdfkj" }, 400 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}