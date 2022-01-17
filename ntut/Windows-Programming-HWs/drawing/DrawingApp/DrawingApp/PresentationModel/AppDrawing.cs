using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DrawingApp.PresentationModel
{
    class AppDrawing : DrawingModel.IDrawer
    {
        private const double TRIANGLE_HEIGHT_RATIO = 2.5;
        private const double TRIANGLE_WIDTH_RATIO = 0.8;
        private SolidColorBrush _blueColor = new SolidColorBrush(Colors.Blue);
        private Canvas _canvas;

        public AppDrawing(Canvas canvas)
        {
            _canvas = canvas;
        }

        // 不做事
        public void Draw(DrawingModel.DrawCommand.Nothing command)
        {
            
        }

        // 畫圓形
        public void Draw(AppCommand.CircleCommand command)
        {
            Ellipse ellipse = new Ellipse();
            int xIndex = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
            int yIndex = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
            InitializeShape(ellipse, (int)command.StartPoint.X, (int)command.StartPoint.Y, xIndex, yIndex, _blueColor);
            _canvas.Children.Add(ellipse);
        }

        // 畫長方形
        public void Draw(AppCommand.RectangleCommand command)
        {
            Rectangle rectangle = new Rectangle();
            int xIndex = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
            int yIndex = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
            InitializeShape(rectangle, (int)command.StartPoint.X, (int)command.StartPoint.Y, xIndex, yIndex, _blueColor);
            _canvas.Children.Add(rectangle);

        }

        // 畫三角形
        public void Draw(AppCommand.TriangleCommand command)
        {
            Polygon polygon = new Polygon();
            int xIndex = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
            int yIndex = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
            Rectangle rectangle = new Rectangle();
            InitializeShape(rectangle, (int)command.StartPoint.X, (int)command.StartPoint.Y, xIndex, yIndex, _blueColor);
            PointCollection points = new PointCollection();
            var list = new List<Windows.Foundation.Point>();
            list.AddRange(BuildTrianglePoints(rectangle));
            foreach (var point in list)
                points.Add(point);
            polygon.Points = points;
            polygon.Fill = _blueColor;
            _canvas.Children.Add(polygon);
        }

        // 產生三角形的點
        private Windows.Foundation.Point[] BuildTrianglePoints(Rectangle bounds)
        {
            Windows.Foundation.Point[] points = new Windows.Foundation.Point[3];
            int upBoundWidth = (int)(bounds.Width * TRIANGLE_WIDTH_RATIO);
            const int HALF = 2;
            if (upBoundWidth % HALF == 1)
                upBoundWidth++;
            int upBoundHeight = (int)Math.Ceiling((upBoundWidth / HALF) * TRIANGLE_HEIGHT_RATIO);
            int leftWidth = (int)(bounds.Height * TRIANGLE_WIDTH_RATIO);
            if (leftWidth % HALF == 0)
                leftWidth++;
            int leftHeight = (int)Math.Ceiling((leftWidth / HALF) * TRIANGLE_HEIGHT_RATIO);
            points[0] = new Windows.Foundation.Point(0, upBoundHeight);
            points[1] = new Windows.Foundation.Point(upBoundWidth, upBoundHeight);
            points[HALF] = new Windows.Foundation.Point(upBoundWidth / HALF, 0);
            OffsetPoints(points, Convert.ToInt32((bounds.Margin.Left) + (bounds.Width - upBoundHeight) / HALF), Convert.ToInt32((bounds.Margin.Top) + (bounds.Height - upBoundWidth) / HALF));
            return points;
        }

        // 幫所有點加上offset
        private void OffsetPoints(Windows.Foundation.Point[] points, int xOffset, int yOffset)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += xOffset;
                points[i].Y += yOffset;
            }
        }

        // 設定shape的helper
        private Shape InitializeShape(Shape shape, int left, int top, int right, int bottom, SolidColorBrush fillColorBrush)
        {
            shape.Margin = new Thickness(left, top, right, bottom);
            shape.Width = right;
            shape.Height = bottom;
            shape.Fill = fillColorBrush;
            return shape;
        }
    }
}
