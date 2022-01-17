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
    public class DataContainerTests
    {
        private const String TEST_STRING = "烏龍 10";
        private const String EMPTY_STRING = "";
        private const String TEST_RIGHT = "1 2";
        private const String TEST_WRONG = "1,2";
        private const String TEST_NAME = "烏龍";
        private const String ERROR_FORMAT = "Error format";
        private const String ERROR_INPUT = "price must be int";
        private const char SPLIT_STRING = ' ';
        private const int TEST_PRICE = 10;
        private DataContainer<Drink> _drinks;
        private DataContainer<IngredientWhichCanBeAdded> _ingredients;

        // 不做事
        public void DoNothing<T>(List<T> list)
        {

        }

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
            _drinks = new DataContainer<Drink>(DoNothing<Drink>, EMPTY_STRING);
            _ingredients = new DataContainer<IngredientWhichCanBeAdded>(DoNothing<IngredientWhichCanBeAdded>, "");
        }

        // GetItem的測試
        [TestMethod()]
        public void GetItemTest()
        {
            Drink drink = new Drink(1, EMPTY_STRING);
            _drinks.Add(drink);
            Assert.AreEqual(_drinks.GetItem(0), drink);
        }

        // SetItem的測試
        [TestMethod()]
        public void SetItemTest()
        {
            Drink drink = new Drink(1, EMPTY_STRING);
            Drink drink2 = new Drink(0, EMPTY_STRING);
            _drinks.Add(drink);
            _drinks.SetItem(0, drink2);
            Assert.AreEqual(_drinks.GetItem(0), drink2);
        }

        // RemoveAt的測試
        [TestMethod()]
        public void RemoveAtTest()
        {
            Drink drink = new Drink(1, EMPTY_STRING);
            _drinks.Add(drink);
            _drinks.RemoveAt(0);
            Assert.AreEqual(_drinks.Container.Count, 0);
        }

        // Add的測試
        [TestMethod()]
        public void AddTest()
        {
            Drink drink = new Drink(1, EMPTY_STRING);
            _drinks.Add(drink);
            Assert.AreEqual(_drinks.GetItem(0), drink);
        }

        // AddNewItem的測試
        [TestMethod()]
        public void AddNewItemTest()
        {
            using (StreamReader s = GenerateStreamFromString(TEST_STRING))
            {
                var data = _drinks.AddNewItem(s);
                Assert.AreEqual(data.Container[0].IsSame(new Drink(TEST_PRICE, TEST_NAME)), true);
            }

            using (StreamReader s = GenerateStreamFromString(TEST_STRING))
            {
                var data = _ingredients.AddNewItem(s);
                Assert.AreEqual(data.Container[0].IsSame(new IngredientWhichCanBeAdded(new Ingredient(TEST_PRICE, TEST_NAME))), true);
            }
        }

        // TryAddItem的測試
        [TestMethod()]
        public void TryAddItemTest()
        {
            int len = _drinks.Container.Count + 1;
            Drink drink = new Drink(1, EMPTY_STRING);
            _drinks.Add(drink);
            try
            {
                _drinks.TryAddItem(drink);
            }
            catch (Exception e)
            {

                Assert.AreEqual(e.Message, EMPTY_STRING);
            }
            const String NAME = "123";
            const int PRICE = 2;
            len++;
            Drink drink2 = new Drink(PRICE, NAME);
            _drinks.TryAddItem(drink2);
            Assert.AreEqual(_drinks.Container.Count, len);
        }

        // ProcessString的測試
        [TestMethod()]
        public void ProcessStringTest()
        {
            Drink drink = _drinks.ProcessString(TEST_STRING);
            Assert.AreEqual(drink.IsSame(new Drink(TEST_PRICE, TEST_NAME)), true);

            try
            {
                const String ERROR_STRING = "1,2";
                _drinks.ProcessString(ERROR_STRING);
            }
            catch (Exception e)
            {

                Assert.AreEqual(e.Message, ERROR_FORMAT);
            }
        }

        // IsLegalTokens的測試
        [TestMethod()]
        public void IsLegalTokensTest()
        {
            String[] test1 = TEST_RIGHT.Split(SPLIT_STRING);
            String[] test2 = TEST_WRONG.Split(SPLIT_STRING);
            Assert.AreEqual(_drinks.IsLegalTokens(test1), true);
            Assert.AreEqual(_drinks.IsLegalTokens(test2), false);
        }

        // TurnTo的測試
        [TestMethod()]
        public void TurnToTest()
        {
            String[] test1 = TEST_RIGHT.Split(SPLIT_STRING);
            String[] test2 = TEST_WRONG.Split(SPLIT_STRING);

            try
            {
                _drinks.TurnTo(test2);
            }
            catch (Exception e)
            {

                Assert.AreEqual(e.Message, ERROR_INPUT);
            }
            const String NAME = "1";
            const int PRICE = 2;
            Assert.AreEqual(_drinks.TurnTo(test1).IsSame(new Drink(PRICE, NAME)), true);
            Assert.IsTrue(_ingredients.TurnTo(test1).IsSame(new IngredientWhichCanBeAdded(new Ingredient(PRICE, NAME))));
        }
    }
}
