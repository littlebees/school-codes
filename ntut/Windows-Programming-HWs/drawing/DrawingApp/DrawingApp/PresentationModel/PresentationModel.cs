using DrawingModel;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DrawingApp.PresentationModel
{
    class PresentationModel : PresentationModelBase
    {
        private IGraphics _graphics;
        private Canvas _canvas;

        public PresentationModel(Model model, Canvas canvas) : base(model, new AppSaver())
        {
            _canvas = canvas;
            _graphics = new WindowsStoreGraphicsAdaptor(canvas);
        }

        // 畫布的accessor
        public Canvas Canvas
        {
            get
            {
                return _canvas;
            }
        }

        // Adaptor的accessor
        public override IGraphics Adapter
        {
            get
            {
                return _graphics;
            }
        }
    }
}
