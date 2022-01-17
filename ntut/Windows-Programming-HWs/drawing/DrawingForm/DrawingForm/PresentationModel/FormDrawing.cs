using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingForm
{
    class FormDrawing : DrawingModel.IDrawer
    {
        private const double TRIANGLE_HEIGHT_RATIO = 2.5;
        private const double TRIANGLE_WIDTH_RATIO = 0.8;
        private Graphics _graphics;

        public FormDrawing(Graphics graph)
        {
            _graphics = graph;
        }

        // 畫圓形
        public void Draw(FormCommand.CircleCommand command)
        {
            using (SolidBrush brush = new SolidBrush(Color.BlueViolet))
            {
                int x = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
                int y = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
                _graphics.FillEllipse(brush, new Rectangle((int)command.StartPoint.X, (int)command.StartPoint.Y, x, y));
            }
        }

        // 不做事
        public void Draw(DrawingModel.DrawCommand.Nothing command)
        {

        }

        // 畫長方形
        public void Draw(FormCommand.RectangleCommand command)
        {
            using (SolidBrush brush = new SolidBrush(Color.BlueViolet))
            {
                int x = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
                int y = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
                _graphics.FillRectangle(brush, new Rectangle((int)command.StartPoint.X, (int)command.StartPoint.Y, x, y));
            }
        }

        // 畫三角形
        public void Draw(FormCommand.TriangleCommand command)
        {
            using (SolidBrush brush = new SolidBrush(Color.BlueViolet))
            {
                int x = (int)Math.Abs(command.EndPoint.X - command.StartPoint.X);
                int y = (int)Math.Abs(command.EndPoint.Y - command.StartPoint.Y);
                _graphics.FillPolygon(brush, BuildTrianglePoints(new Rectangle((int)command.StartPoint.X, (int)command.StartPoint.Y, x, y)));
            }
        }

        // 產生三角形的點
        private System.Drawing.Point[] BuildTrianglePoints(Rectangle bounds)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[3];
            int upBoundWidth = (int)(bounds.Width * TRIANGLE_WIDTH_RATIO);
            const int HALF = 2;
            if (upBoundWidth % HALF == 1)
                upBoundWidth++;
            int upBoundHeight = (int)Math.Ceiling((upBoundWidth / HALF) * TRIANGLE_HEIGHT_RATIO);
            int leftWidth = (int)(bounds.Height * TRIANGLE_WIDTH_RATIO);
            if (leftWidth % HALF == 0)
                leftWidth++;
            int leftHeight = (int)Math.Ceiling((leftWidth / HALF) * TRIANGLE_HEIGHT_RATIO);
            points[0] = new System.Drawing.Point(0, upBoundHeight);
            points[1] = new System.Drawing.Point(upBoundWidth, upBoundHeight);
            points[HALF] = new System.Drawing.Point(upBoundWidth / HALF, 0);
            OffsetPoints(points, bounds.X + (bounds.Width - upBoundHeight) / HALF, bounds.Y + (bounds.Height - upBoundWidth) / HALF);
            return points;
        }

        // 幫所有點加上offset
        private void OffsetPoints(System.Drawing.Point[] points, int xOffset, int yOffset)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += xOffset;
                points[i].Y += yOffset;
            }
        }
    }
}
