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
    public class IngredientWhichCanBeAddedTests
    {
        private IngredientWhichCanBeAdded _ingredient;
        private PrivateObject _target;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _ingredient = new IngredientWhichCanBeAdded(new Ingredient(1, ""));
            _target = new PrivateObject(_ingredient);
        }

        // 測試Clone
        [TestMethod()]
        public void CloneTest()
        {
            Assert.AreNotEqual(_ingredient, _ingredient.Clone());
        }

        // 測試IsAdded
        [TestMethod()]
        public void IsAddedTest()
        {
            _ingredient.IsAdded = true;
            Assert.AreEqual(_ingredient.IsAdded, true);
        }

        // 測試Top
        [TestMethod()]
        public void TopTest()
        {
            Assert.AreEqual(_ingredient.Top, _ingredient.TheIngredient);
        }

        // 測試IsSame
        [TestMethod()]
        public void IsSameTest()
        {
            IngredientWhichCanBeAdded ingredient = new IngredientWhichCanBeAdded(new Ingredient(1, ""));
            Assert.AreEqual(_ingredient.IsSame(ingredient), true);
        }
    }
}
