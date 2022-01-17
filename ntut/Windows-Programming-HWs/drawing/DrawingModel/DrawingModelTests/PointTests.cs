using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel.Tests
{
    [TestClass()]
    public class PointTests
    {
        [TestMethod()]
        public void PointTest()
        {
            Point p = new Point(0, 0);
            p.X = 1;
            p.Y = 1;
            Assert.AreEqual(p.X, 1);
            Assert.AreEqual(p.Y, 1);
        }
    }
}