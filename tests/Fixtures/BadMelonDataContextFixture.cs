﻿using BadMelon.Data;
using BadMelon.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace BadMelon.Tests.Fixtures
{
    public class BadMelonDataContextFixture : IDisposable
    {
        public BadMelonDataContext BadMelonDataContext { get; }

        public BadMelonDataContextFixture()
        {
            BadMelonDataContext = new BadMelonDataContext(new DbContextOptionsBuilder<BadMelonDataContext>()
                                                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                              .Options);
        }

        public void Dispose()
        {
            BadMelonDataContext.Dispose();
        }

        public void WithSeedData()
        {
            AsyncHelper.RunSync(() => BadMelonDataContext.Seed());
        }
    }
}