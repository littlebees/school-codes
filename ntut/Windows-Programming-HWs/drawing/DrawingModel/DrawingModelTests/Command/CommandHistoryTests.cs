using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingModel.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel.Command.Tests
{
    [TestClass()]
    public class CommandHistoryTests
    {
        private CommandHistory _history = new CommandHistory();
        public List<HistoryNode> GetList()
        {
            PrivateObject po = new PrivateObject(_history);
            return (List<HistoryNode>)po.GetField("_stack");
        }

        [TestMethod()]
        public void CommandHistoryTest()
        {
            Assert.AreEqual(GetList().Count, 1);
        }

        [TestMethod()]
        public void RedoTest()
        {
            Assert.AreEqual(_history.GetValidLength(), 1);
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Triangle());
            Assert.AreEqual(GetList().Count, 4);
            _history.Undo();
            _history.Undo();
            Assert.AreEqual(_history.GetValidLength(), 2);
            _history.Redo();
            _history.Redo();
            _history.Redo();
            Assert.AreEqual(_history.GetValidLength(), 4);
        }

        [TestMethod()]
        public void UndoTest()
        {
            Assert.AreEqual(GetList().Count, 1);
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Rectangle());
            Assert.AreEqual(_history.GetValidLength(), 4);
            _history.Undo();
            _history.Undo();
            Assert.AreEqual(_history.GetValidLength(), 2);
        }

        [TestMethod()]
        public void ClearTest()
        {
            _history.Clear();
            Assert.AreEqual(GetList().Count, 1);
        }

        [TestMethod()]
        public void NewTest()
        {
            Assert.AreEqual(GetList().Count, 1);
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Circle());
            _history.Insert(new DrawCommand.Circle());
            Assert.AreEqual(GetList().Count, 4);
            Assert.AreEqual(4, _history.GetValidLength());
            _history.Undo();
            _history.Undo();
            _history.Insert(new DrawCommand.Triangle());
            Assert.AreEqual(2 + 1, _history.GetValidLength());
        }
    }
}