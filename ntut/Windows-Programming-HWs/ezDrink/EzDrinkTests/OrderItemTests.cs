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
    public class OrderItemTests
    {
        private const String TEST_NAME = "123";
        private OrderItem _order;
        private PrivateObject _target;
        private IAdapter<IngredientWhichCanBeAdded> _adapter;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            List<IngredientWhichCanBeAdded> list = new List<IngredientWhichCanBeAdded>();
            list.Add(new IngredientWhichCanBeAdded(new Ingredient(0, TEST_NAME)));
            list.Add(new IngredientWhichCanBeAdded(new Ingredient(1, "")));
            _order = new OrderItem(new Drink(1, ""), list);
            _target = new PrivateObject(_order);
            _adapter = new FakeReceiver();
            _order.IngredientAdapter = _adapter;
        }

        // AddIngredient的測試
        [TestMethod()]
        public void AddIngredientTest()
        {
            _order.AddIngredient(new Ingredient(1, ""));
            Assert.AreEqual(((List<IngredientWhichCanBeAdded>)_target.GetFieldOrProperty("_ingredients"))[1].IsAdded, true);
        }

        // Price的測試
        [TestMethod()]
        public void PriceTest()
        {
            _order.Ingredients[0].IsAdded = true;
            Assert.AreEqual(_order.Price, 1);
        }

        // DrinkName的測試
        [TestMethod()]
        public void DrinkNameTest()
        {
            Assert.AreEqual(_order.DrinkName, "");
        }

        // Ingredient的測試
        [TestMethod()]
        public void IngredientTest()
        {
            _order.Ingredients[0].IsAdded = true;
            Assert.AreEqual(_order.Ingredient, TEST_NAME);
        }

        // Sweet的測試
        [TestMethod()]
        public void SweetTest()
        {
            const String NORMAL = "正常";
            const String HALF = "半糖";
            const String FEW = "微糖";
            _order.Sugar = Sweet.Normal;
            Assert.AreEqual(_order.SweetLevel, NORMAL);
            _order.Sugar = Sweet.Half;
            Assert.AreEqual(_order.SweetLevel, HALF);
            _order.Sugar = Sweet.Few;
            Assert.AreEqual(_order.SweetLevel, FEW);
        }

        // Cold的測試
        [TestMethod()]
        public void ColdTest()
        {
            const String NORMAL = "正常";
            const String HALF = "溫熱";
            const String FEW = "少冰";
            _order.Cold = Ice.Normal;
            Assert.AreEqual(_order.Temperature, NORMAL);
            _order.Cold = Ice.Half;
            Assert.AreEqual(_order.Temperature, HALF);
            _order.Cold = Ice.Few;
            Assert.AreEqual(_order.Temperature, FEW);
        }

        // OrderDescription的測試
        [TestMethod()]
        public void OrderDescriptionTest()
        {
            const String PRICE = "1";
            const int SWEET = 2;
            const int ICE = 3;
            const int INGREDIENT = 4;
            var list = _order.OrderDescription;
            Assert.AreEqual(list[0], "");
            Assert.AreEqual(list[1], PRICE);
            Assert.AreEqual(list[SWEET], "");
            Assert.AreEqual(list[ICE], "");
            Assert.AreEqual(list[INGREDIENT], "");
        }

        // ReceiveDataToApply的測試
        [TestMethod()]
        public void ReceiveDataToApplyTest()
        {
            int len = _order.Ingredients.Count + 1;
            List<Ingredient> list = new List<Ingredient>();
            list.Add(new Ingredient(1, ""));
            _order.ReceiveDataToApply(list);
            Assert.AreEqual(_order.Ingredients.Count, len);
        }

        // ReceiveDataToApply的測試
        [TestMethod()]
        public void ReceiveDataToApplyTest1()
        {
            int len = _order.Ingredients.Count - 1;
            List<IngredientWhichCanBeAdded> list = new List<IngredientWhichCanBeAdded>();
            list.Add(new IngredientWhichCanBeAdded(new Ingredient(1, "")));
            _order.ReceiveDataToApply(list);
            Assert.AreEqual(_order.Ingredients.Count, len);
        }
    }
}
