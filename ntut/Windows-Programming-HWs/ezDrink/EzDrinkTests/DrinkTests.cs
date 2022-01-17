using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzDrink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EzDrink.Tests
{
    [TestClass()]
    public class DrinkTests
    {
        private Drink _drink;
        private PrivateObject _target;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _drink = new Drink(1, "");
            _target = new PrivateObject(_drink);
        }

        // 測試IsSame
        [TestMethod()]
        public void IsSameTest()
        {
            Drink drink = new Drink(1, "");
            Assert.AreEqual(drink.IsSame(_drink), true);
        }

        // 測試Price
        [TestMethod()]
        public void PriceTest()
        {
            Assert.AreEqual(_drink.Price, 1);
        }

        // 測試Title
        [TestMethod()]
        public void TitleTest()
        {
            Assert.AreEqual(_drink.Title, "");
        }
    }
}
