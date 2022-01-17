using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzDrink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
namespace EzDrink.Tests
{
    [TestClass()]
    public class EzDrinkModelTests
    {
        private const String TEST_STRING = "烏龍 10";
        private const String TEST_NAME = "烏龍";
        private const int TEST_PRICE = 10;
        private EzDrinkModel _model;
        private PrivateObject _target;

        // 由String產生StreamReader
        public StreamReader GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return new StreamReader(stream);
        }

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _model = new EzDrinkModel();
            _target = new PrivateObject(_model);
        }

        // GetAllHistory的測試
        [TestMethod()]
        public void GetAllHistoryTest()
        {
            const String NAME = "_orders";
            Assert.AreEqual(_model.GetAllHistory(), _target.GetFieldOrProperty(NAME));
        }

        // GetAllDrink的測試
        [TestMethod()]
        public void GetAllDrinkTest()
        {
            const String NAME = "_drinks";
            Assert.AreEqual(_model.GetAllDrink(), ((DataContainer<Drink>)_target.GetFieldOrProperty(NAME)).Container);
        }

        // GetAllOrder的測試
        [TestMethod()]
        public void GetAllOrderTest()
        {
            const String NAME = "_order";
            Assert.AreEqual(_model.GetAllOrder(), _target.GetFieldOrProperty(NAME));
        }

        // GetAdapterCollection的測試
        [TestMethod()]
        public void GetAdapterCollectionTest()
        {
            const String NAME = "_allAdapter";
            Assert.AreEqual(_model.GetAdapterCollection(), _target.GetFieldOrProperty(NAME));
        }

        // MakeAnOrder的測試
        [TestMethod()]
        public void MakeAnOrderTest()
        {
            _model.MakeAnOrder(0);
            Assert.AreEqual(_model.GetAllOrder().Count, 1);
        }

        // SetSweetOf的測試
        [TestMethod()]
        public void SetSweetOfTest()
        {
            const String SWEET_STRING = "無糖";
            OrderItem order = _model.MakeAnOrder(0);
            _model.SetSweetOf(0, Sweet.No);
            Assert.AreEqual(order.SweetLevel, SWEET_STRING);
        }

        // SetIceOf的測試
        [TestMethod()]
        public void SetIceOfTest()
        {
            const String ICE_STRING = "去冰";
            OrderItem order = _model.MakeAnOrder(0);
            _model.SetIceOf(0, Ice.No);
            Assert.AreEqual(order.Temperature, ICE_STRING);
        }

        // AddIngredientOf的測試
        [TestMethod()]
        public void AddIngredientOfTest()
        {
            IngredientWhichCanBeAdded ingredient = _model.GetIngredient().Container[0];
            _model.MakeAnOrder(0);
            _model.AddIngredientOf(0, 0);
            Assert.IsTrue(ingredient.TheIngredient.IsSame((_model.GetAllOrder()[0]).Ingredients[0].TheIngredient));
        }

        // RemoveOrderOf的測試
        [TestMethod()]
        public void RemoveOrderOfTest()
        {
            _model.MakeAnOrder(0);
            _model.RemoveOrderOf(0);
            Assert.AreEqual(_model.GetAllOrder().Count, 0);
        }

        // SaveOrder的測試
        [TestMethod()]
        public void SaveOrderTest()
        {
            _model.MakeAnOrder(0);
            _model.SetSweetOf(0, Sweet.No);
            _model.SetIceOf(0, Ice.No);
            _model.SaveOrder();
            Assert.AreEqual(_model.GetAllHistory().Count, 1);
        }

        // AddNewItem的測試
        [TestMethod()]
        public void AddNewItemTest()
        {
            using (StreamReader s = GenerateStreamFromString(TEST_STRING))
            {
                DataContainer<Drink> drink = _model.AddNewItem<Drink>(s);
                Assert.IsTrue(drink.GetItem(0).IsSame(new Drink(TEST_PRICE, TEST_NAME)));
            }

            using (StreamReader s = GenerateStreamFromString(TEST_STRING))
            {
                DataContainer<IngredientWhichCanBeAdded> drink = _model.AddNewItem<IngredientWhichCanBeAdded>(s);
                Assert.IsTrue(drink.GetItem(0).TheIngredient.IsSame(new Ingredient(TEST_PRICE, TEST_NAME)));
            }

        }

        // TryAddDrink的測試
        [TestMethod()]
        public void TryAddDrinkTest()
        {
            int length = _model.GetAllDrink().Count;
            _model.TryAddDrink(TEST_NAME, TEST_PRICE);
            Assert.IsTrue(_model.GetAllDrink()[length].IsSame(new Drink(TEST_PRICE, TEST_NAME)));

            try
            {
                _model.TryAddDrink(TEST_NAME, TEST_PRICE);
            }
            catch (Exception e)
            {
                const String MESSAGE = "Duplicate Drink";
                Assert.AreEqual(e.Message, MESSAGE);
            }
        }

        // TryAddIngredient的測試
        [TestMethod()]
        public void TryAddIngredientTest()
        {
            int length = _model.GetIngredient().Container.Count;
            _model.TryAddIngredient(TEST_NAME, TEST_PRICE);
            Assert.IsTrue((_model.GetIngredient().Container)[length].TheIngredient.IsSame(new Ingredient(TEST_PRICE, TEST_NAME)));

            try
            {
                _model.TryAddIngredient(TEST_NAME, TEST_PRICE);
            }
            catch (Exception e)
            {
                const String MESSAGE = "Duplicate Ingredient";
                Assert.AreEqual(e.Message, MESSAGE);
            }
        }

        // IsAllOrderOK的測試
        [TestMethod()]
        public void IsAllOrderOKTest()
        {
            Assert.IsFalse(_model.IsAllOrderOK);
            _model.MakeAnOrder(0);
            Assert.IsFalse(_model.IsAllOrderOK);
        }

        // IsOrderEmpty的測試
        [TestMethod()]
        public void IsOrderEmptyTest()
        {
            Assert.IsTrue(_model.IsOrderEmpty);
        }

        // Price的測試
        [TestMethod()]
        public void PriceTest()
        {
            Assert.AreEqual(_model.Price, 0);
            _model.MakeAnOrder(0);
            const int PRICE = 20;
            Assert.AreEqual(_model.Price, PRICE);
        }

        // RemoveDrinkByIndex的測試
        [TestMethod()]
        public void RemoveDrinkByIndexTest()
        {
            int length = _model.GetAllDrink().Count - 1;
            _model.RemoveDrinkByIndex(0);
            Assert.AreEqual(_model.GetAllDrink().Count, length);
        }

        // RemoveIngredientByIndex的測試
        [TestMethod()]
        public void RemoveIngredientByIndexTest()
        {
            int length = _model.GetIngredient().Container.Count - 1;
            _model.RemoveIngredientByIndex(0);
            Assert.AreEqual(_model.GetIngredient().Container.Count, length);
        }

        // SetDrinkName的測試
        [TestMethod()]
        public void SetDrinkNameTest()
        {
            const String NAME = "123";
            _model.SetDrinkName(0, "123");
            Assert.AreEqual(_model.GetAllDrink()[0].Title, NAME);
        }

        // SetDrinkPrice的測試
        [TestMethod()]
        public void SetDrinkPriceTest()
        {
            _model.SetDrinkPrice(0, 1);
            Assert.AreEqual(_model.GetAllDrink()[0].Price, 1);
        }

        // SetIngredientName的測試
        [TestMethod()]
        public void SetIngredientNameTest()
        {
            const String NAME = "123";
            _model.SetIngredientName(0, "123");
            Assert.AreEqual((_model.GetIngredient().Container)[0].TheIngredient.Title, NAME);
        }

        // SetIngredientPrice的測試
        [TestMethod()]
        public void SetIngredientPriceTest()
        {
            _model.SetIngredientPrice(0, 1);
            Assert.AreEqual((_model.GetIngredient().Container)[0].TheIngredient.Price, 1);
        }
    }
}
