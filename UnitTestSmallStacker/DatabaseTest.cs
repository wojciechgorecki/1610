using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallStacker.Utills.DatabaseUtill;
using System.Net.NetworkInformation;

namespace UnitTestSmallStacker
{
    [TestClass]
    public class DatabaseTest
    {
        DatabaseController dbController = new DatabaseController();
        [TestMethod]
        public void Test_NegativeCountUsers()
        {
            Assert.AreNotEqual(dbController.CountIDUsers(), 1);
        }

        [TestMethod]
        public void Test_PositiveConnectionToServer()
        {
            Assert.AreEqual(dbController.ConnectionToDatabase(), true);
        }

        [TestMethod]
        public void Test_PositiveSearchID()
        {
            Assert.AreEqual(dbController.IDExists(101), true);
        }

        [TestMethod]
        public void Test_NegativeSearchID()
        {
            Assert.AreNotEqual(dbController.IDExists(1), true);
        }

        [TestMethod]
        public void Test_PositiveSeachUserName()
        {
            Assert.AreEqual(dbController.UserNameExists("m.bors"), true);
        }
    }
}
