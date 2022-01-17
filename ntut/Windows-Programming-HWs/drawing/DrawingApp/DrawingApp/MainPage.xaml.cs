using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//空白頁項目範本收錄在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DrawingApp
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TEST_FILE_NAME = "test.bmp";
        private DrawingModel.Model _model;
        private PresentationModel.PresentationModel _presentationModel;

        public MainPage()
        {
            this.InitializeComponent();
            _model = new DrawingModel.Model();
            _presentationModel = new PresentationModel.PresentationModel(_model, _canvas);
            _canvas.PointerPressed += HandleCanvasPressed;
            _canvas.PointerReleased += HandleCanvasReleased;
            _canvas.PointerMoved += HandleCanvasMoved;
            _model._modelChanged += HandleModelChanged;
        }

        // 當按下滑鼠時
        public void HandleCanvasPressed(object sender, PointerRoutedEventArgs e)
        {
            _presentationModel.HandleCanvasPressed((int)e.GetCurrentPoint(_canvas).Position.X, (int)e.GetCurrentPoint(_canvas).Position.Y);
        }

        // 當放開滑鼠時
        public void HandleCanvasReleased(object sender, PointerRoutedEventArgs e)
        {
            _presentationModel.HandleCanvasReleased((int)e.GetCurrentPoint(_canvas).Position.X, (int)e.GetCurrentPoint(_canvas).Position.Y);
        }

        // 當移動滑鼠時
        public void HandleCanvasMoved(object sender, PointerRoutedEventArgs e)
        {
            _presentationModel.HandleCanvasMoved((int)e.GetCurrentPoint(_canvas).Position.X, (int)e.GetCurrentPoint(_canvas).Position.Y);
        }

        // callback
        public void HandleModelChanged()
        {
            _presentationModel.Draw();
        }

        // 按下長方形
        private void ClickRectangle(object sender, RoutedEventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(AppCommand.RectangleCommand);
        }

        // 按下三角形
        private void ClickTriangle(object sender, RoutedEventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(AppCommand.TriangleCommand);
        }

        // 按下圓形
        private void ClickCircle(object sender, RoutedEventArgs e)
        {
            _presentationModel.CurrentDrawCommand = typeof(AppCommand.CircleCommand);
        }

        // 按下undo
        private void ClickUndo(object sender, RoutedEventArgs e)
        {
            _presentationModel.Undo();
        }

        // 按下redo
        private void ClickRedo(object sender, RoutedEventArgs e)
        {
            _presentationModel.Redo();
        }

        // 按下save
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            _presentationModel.Save(new AppCommand.BmpCommand(Convert.ToInt32(_canvas.ActualWidth), Convert.ToInt32(_canvas.ActualHeight), TEST_FILE_NAME));
        }
    }
}
