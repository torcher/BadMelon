using BadMelon.Data.DTOs;
using System.Collections;
using System.Collections.Generic;

namespace BadMelon.Tests.Data
{
    public class BadRegistrationsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Registration { Username = "test", EmailAddress = "test1@test.com" } };
            yield return new object[] { new Registration { EmailAddress = "test1@test.com" } };

            yield return new object[] { new Registration { Username = "testing3" } };
            yield return new object[] { new Registration { Username = "testing4", EmailAddress = "test" } };

            yield return new object[] { new Registration { Username = "testing5", EmailAddress = "test1@test.com", Password = " " } };
            yield return new object[] { new Registration { Username = "testing6", EmailAddress = "test1@test.com", Password = "pass" } };
            yield return new object[] { new Registration { Username = "testing7", EmailAddress = "test1@test.com", Password = "apple" } };
            yield return new object[] { new Registration { Username = "testing8", EmailAddress = "test1@test.com", Password = "rootpassword" } };
            yield return new object[] { new Registration { Username = "testing9", EmailAddress = "test1@test.com", Password = "sdpoyktrjyltlfj1" } };
            yield return new object[] { new Registration { Username = "testing10", EmailAddress = "test1@test.com", Password = "smnxapodopjffl$" } };
            yield return new object[] { new Registration { Username = "testing11", EmailAddress = "test1@test.com", Password = "ldkfjld^3" } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}