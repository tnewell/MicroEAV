﻿using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreTest
{
    [TestClass]
    public partial class EAVStoreTestHarness
    {
        private static TestContext myTestContext;
        private static Random rng = new Random((int)DateTime.Now.Ticks);

        EAV.Store.IStoreObjectFactory factory = new EAVStoreLibrary.StoreObjectFactory();

        [ClassInitialize]
        public static void SetupTestHarness(TestContext testContext)
        {
            myTestContext = testContext;

            int rngSeed = (int)DateTime.Now.Ticks;

            myTestContext.WriteLine("Seeding rng with {0}.", rngSeed);

            rng = new Random(rngSeed);
        }

        [ClassCleanup]
        public static void TeardownTestHarness()
        {
        }

        [TestInitialize]
        public void SetupTest()
        {
        }

        [TestCleanup]
        public void TeardownTest()
        {
        }
    }
}
