using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public class Model
    {
        public event ModelChangedEventHandler _modelChanged;
        public delegate void ModelChangedEventHandler();
        private Command.CommandHistory _history = new Command.CommandHistory();

        // 如果是NoThing就不做事
        public void Do(DrawCommand.Nothing command)
        {
            return;
        }
        
        // 把DrawCommand新增到History中
        public void Do(Command.IDrawCommand command)
        {
            _history.Insert(command);
        }

        // 現在在畫什麼
        public Command.IDrawCommand NowDrawing
        {
            get
            {
                return _history.NowDrawing;
            }
        }

        // 清除紀錄
        public void Clear()
        {
            _history.Clear();
            NotifyModelChanged();
        }
        
        // 在畫布上把history中的所有command都跑一遍
        public void Draw(IGraphics graphics)
        {
            graphics.ClearAll();
            foreach (Command.IDrawCommand aLine in _history)
                graphics.Draw(aLine);
        }

        // 開始畫畫
        public void Start(double xIndex, double yIndex)
        {
            NowDrawing.StartPoint = new Point(xIndex, yIndex);
        }

        // 畫畫中
        public void Move(double xIndex, double yIndex)
        {
            NowDrawing.EndPoint = new Point(xIndex, yIndex);
        }

        // 畫完了
        public void Stop(double xIndex, double yIndex)
        {
            NowDrawing.EndPoint = new Point(xIndex, yIndex);
        }

        // callback
        public void NotifyModelChanged()
        {
            if (_modelChanged != null)
                _modelChanged();
        }

        // redo
        public void Redo()
        {
            _history.Redo();
            NotifyModelChanged();
        }

        // undo
        public void Undo()
        {
            _history.Undo();
            NotifyModelChanged();
        }
    }
}
