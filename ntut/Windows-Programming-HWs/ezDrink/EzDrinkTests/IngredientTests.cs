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
    public class IngredientTests
    {
        private Ingredient _ingredient;
        private PrivateObject _target;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _ingredient = new Ingredient(1, "");
            _target = new PrivateObject(_ingredient);
        }

        // 測試IsSame
        [TestMethod()]
        public void IsSameTest()
        {
            Ingredient drink = new Ingredient(1, "");
            Assert.AreEqual(drink.IsSame(_ingredient), true);
        }

        // 測試Price
        [TestMethod()]
        public void PriceTest()
        {
            Assert.AreEqual(_ingredient.Price, 1);
        }

        // 測試Title
        [TestMethod()]
        public void TitleTest()
        {
            Assert.AreEqual(_ingredient.Title, "");
        }

        // 測試Clone
        [TestMethod()]
        public void CloneTest()
        {
            Assert.AreEqual(_ingredient.Clone(), _ingredient);
        }
    }
}
