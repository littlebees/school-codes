using System.Windows.Forms;
using System.Drawing;
using DrawingModel;
using DrawingModel.Command;
using System;

namespace DrawingForm.PresentationModel
{
    class WindowsFormsGraphicsAdaptor : IGraphics
    {
        
        private Graphics _graphics;
        private FormDrawing _formDrawing;

        public WindowsFormsGraphicsAdaptor(Graphics graphics)
        {
            this._graphics = graphics;
            _formDrawing = new FormDrawing(_graphics);
        }

        // OnPaint時會自動清除畫面，因此不需實作
        public void ClearAll()
        {

        }

        // 畫圖，轉交給adaptor
        public void Draw(IDrawCommand command)
        {
            _formDrawing.Draw((dynamic)command);
        }
    }
}
