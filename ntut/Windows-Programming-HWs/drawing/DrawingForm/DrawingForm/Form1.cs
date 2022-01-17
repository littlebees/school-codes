using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingForm
{
    public partial class Form1 : Form
    {
        private const string TEST_FILE_NAME = "test.bmp";
        private PresentationModel.PresentationModel _presentationModel;

        public Form1()
        {
            InitializeComponent();
            DrawingModel.Model model = new DrawingModel.Model();
            model._modelChanged += HandleModelChanged;
            _presentationModel = new PresentationModel.PresentationModel(model, _canvas);
        }

        // 當按下滑鼠時
        public void HandleCanvasPressed(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _presentationModel.HandleCanvasPressed(e.X, e.Y);
        }

        // 當放開滑鼠時
        public void HandleCanvasReleased(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _presentationModel.HandleCanvasReleased(e.X, e.Y);
        }

        // 當移動滑鼠時
        public void HandleCanvasMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _presentationModel.HandleCanvasMoved(e.X, e.Y);
        }

        // OnPaint Event
        public void HandleCanvasPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            _presentationModel.Graphics = e.Graphics;
            _presentationModel.Draw();
        }

        // callback
        public void HandleModelChanged()
        {
            Invalidate(true);
        }

        // 按下長方形
        private void ClickRectangle(object sender, EventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(FormCommand.RectangleCommand);
        }

        // 按下三角形
        private void ClickTriangle(object sender, EventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(FormCommand.TriangleCommand);
        }

        // 按下圓形
        private void ClickCircle(object sender, EventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(FormCommand.CircleCommand);
        }

        // 按下undo
        private void ClickUndo(object sender, EventArgs e)
        {
            _presentationModel.Undo();
        }

        // 按下redo
        private void ClickRedo(object sender, EventArgs e)
        {
            _presentationModel.Redo();
        }

        // 按下save
        private void ClickSave(object sender, EventArgs e)
        {
            _presentationModel.Save(new FormCommand.BmpCommand(_canvas.Width, _canvas.Height, TEST_FILE_NAME));
        }
    }
}
