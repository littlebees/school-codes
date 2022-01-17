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
    public class PresentationModelBaseTests
    {
        private DrawingModelTests.FakePresentationModel _model;
        private DrawingModelTests.FakeSaver _saver;

        public void DoNothing()
        {

        }

        [TestInitialize()]
        public void Initialize()
        {
            Model model = new Model();
            model._modelChanged += DoNothing;
            _saver = new DrawingModelTests.FakeSaver();
            _model = new DrawingModelTests.FakePresentationModel(model, _saver);
        }

        [TestMethod()]
        public void HandleCanvasPressedTest()
        {
            _model.CurrentDrawCommand = typeof(DrawCommand.Circle);
            _model.HandleCanvasPressed(0, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            Assert.AreEqual(1, _model.Count);
            _model.HandleCanvasPressed(1, 0);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            Assert.AreEqual(1, _model.Count);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            Assert.AreEqual(2, _model.Count);
        }

        [TestMethod()]
        public void HandleCanvasReleasedTest()
        {
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            Assert.AreEqual(1, _model.Count);
        }

        [TestMethod()]
        public void HandleCanvasMovedTest()
        {
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasMoved(2, 2);
            _model.Draw();
            Assert.AreEqual(1, _model.Count);
        }

        [TestMethod()]
        public void RedoTest()
        {
            _model.CurrentDrawCommand = typeof(DrawCommand.Circle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.CurrentDrawCommand = typeof(DrawCommand.Rectangle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Undo();
            _model.Redo();
            _model.Draw();
            Assert.AreEqual(3, _model.Count);
        }

        [TestMethod()]
        public void UndoTest()
        {
            _model.CurrentDrawCommand = typeof(DrawCommand.Circle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.CurrentDrawCommand = typeof(DrawCommand.Rectangle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Undo();
            _model.Draw();
            Assert.AreEqual(2, _model.Count);
        }

        [TestMethod()]
        public void ClearTest()
        {
            _model.CurrentDrawCommand = typeof(DrawCommand.Circle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.CurrentDrawCommand = typeof(DrawCommand.Rectangle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            _model.Clear();
            Assert.AreEqual(0, _model.Count);
        }

        [TestMethod()]
        public void DrawTest()
        {
            _model.CurrentDrawCommand = typeof(DrawCommand.Circle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.CurrentDrawCommand = typeof(DrawCommand.Rectangle);
            _model.HandleCanvasPressed(1, 1);
            _model.HandleCanvasReleased(2, 2);
            _model.Draw();
            Assert.AreEqual(3, _model.Count);

            // BmpFile
            BmpFile bmp = new BmpFile();
            bmp.Height = 1;
            bmp.Width = 1;
            bmp.FileName = "";
            Assert.AreEqual(1, bmp.Height);
            Assert.AreEqual(1, bmp.Width);
            Assert.AreEqual("", bmp.FileName);
        }

        [TestMethod()]
        public void SaveTest()
        {
            _model.Save(new BmpFile());
            Assert.IsTrue(_saver.IsSaved);
        }
    }
}