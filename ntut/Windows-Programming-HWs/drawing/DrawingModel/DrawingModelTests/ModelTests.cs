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
    public class ModelTests
    {
        private Model _model = new Model();
        private int _forFunctionTest;

        public Command.CommandHistory GetHistory()
        {
            PrivateObject po = new PrivateObject(_model);
            return (Command.CommandHistory)po.GetField("_history");
        }

        [TestMethod()]
        public void DoTest()
        {
            // 要有設定座標才會動
            _model.Do(new DrawCommand.Circle());
            _model.Do(new DrawCommand.Circle());
            int length = 0;
            foreach (var ignore in GetHistory())
                length++;
            Assert.AreEqual(2 + 1, length);

            _model.Do(new DrawCommand.Nothing());
        }

        [TestMethod()]
        public void ClearTest()
        {
            _model.Do(new DrawCommand.Circle());
            _model.Do(new DrawCommand.Circle());
            _model.Clear();
            int length = 0;
            foreach (var ignore in GetHistory())
                length++;
            Assert.AreEqual(1, length);
        }

        [TestMethod()]
        public void DrawTest()
        {
            DrawingModelTests.FakeGraphics item = new DrawingModelTests.FakeGraphics();
            _model.Draw(item);
            Assert.AreEqual(1, item.GetNumber());
        }

        [TestMethod()]
        public void StartTest()
        {
            _model.Start(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.Y);
            _model.Do(new DrawCommand.Circle());
            _model.Start(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.Y);
            _model.Do(new DrawCommand.Rectangle());
            _model.Start(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.Y);
            _model.Do(new DrawCommand.Triangle());
            _model.Start(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.StartPoint.Y);
        }

        [TestMethod()]
        public void MoveTest()
        {
            _model.Move(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.Y);
            _model.Do(new DrawCommand.Circle());
            _model.Move(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.Y);
            _model.Do(new DrawCommand.Triangle());
            _model.Move(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.Y);
            _model.Do(new DrawCommand.Rectangle());
            _model.Move(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.Y);
        }

        [TestMethod()]
        public void StopTest()
        {
            _model.Stop(0, 0);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.X);
            Assert.AreEqual(0, _model.NowDrawing.EndPoint.Y);
        }

        [TestMethod()]
        public void NotifyModelChangedTest()
        {
            _model._modelChanged += TestFunction;
            _model.NotifyModelChanged();
            Assert.AreEqual(10, _forFunctionTest);
        }

        [TestMethod()]
        public void RedoTest()
        {
            _model.Do(new DrawCommand.Circle());
            _model.Do(new DrawCommand.Circle());
            _model.Undo();
            _model.Undo();
            _model.Undo();
            _model.Redo();
            _model.Redo();
            _model.Redo();
            _model.Redo();
            Assert.AreEqual(3, GetHistory().GetValidLength());
        }

        [TestMethod()]
        public void UndoTest()
        {
            _model.Do(new DrawCommand.Circle());
            _model.Do(new DrawCommand.Circle());
            _model.Undo();
            _model.Undo();
            _model.Undo();
            int length = 0;
            foreach (var ignore in GetHistory())
                length++;
            Assert.AreEqual(1, length);
        }

        public void TestFunction()
        {
            _forFunctionTest = 10;
        }
    }
}