using DrawingModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DrawingModel.Command;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using System.Collections.Generic;

namespace DrawingApp.PresentationModel
{
    class WindowsStoreGraphicsAdaptor : IGraphics
    {
        private AppDrawing _appDrawing;
        private Canvas _canvas;

        public WindowsStoreGraphicsAdaptor(Canvas canvas)
        {
            this._canvas = canvas;
            _appDrawing = new AppDrawing(_canvas);
        }

        // 清除畫面
        public void ClearAll()
        {
            _canvas.Children.Clear();
        }

        // 畫圖，轉交給adaptor
        public void Draw(IDrawCommand command)
        {
            _appDrawing.Draw((dynamic)command);
        }
    }
}
