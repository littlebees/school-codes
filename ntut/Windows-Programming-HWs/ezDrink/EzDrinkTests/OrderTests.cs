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
    public class OrderTests
    {
        private DateTime _now;
        private Order _order;
        private PrivateObject _target;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _now = DateTime.Now;
            _order = new Order(new List<OrderItem>(), _now);
            _target = new PrivateObject(_order);
        }

        // Time的測試
        [TestMethod()]
        public void TimeTest()
        {
            Assert.AreEqual(_order.Time, _now);
        }

        // Price的測試
        [TestMethod()]
        public void PriceTest()
        {
            _order.AddOrderItem(new OrderItem(new Drink(1, ""), new List<IngredientWhichCanBeAdded>()));
            Assert.AreEqual(_order.Price, 1);

        }

        // GetOrder的測試
        [TestMethod()]
        public void GetOrderTest()
        {
            Assert.AreEqual(_order.GetOrder().Count, 0);
        }
    }
}
