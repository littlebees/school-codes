using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public abstract class PresentationModelBase
    {
        private Model _model;
        private bool _isPressed = false;
        private SaverBase _saver;

        public PresentationModelBase(Model model, SaverBase saver)
        {
            this._model = model;
            _saver = saver;
            _saver.SetPresentationModel(this);
            CurrentDrawCommand = typeof(DrawCommand.Nothing);
        }

        // 開始畫畫
        public void HandleCanvasPressed(int xIndex, int yIndex)
        {
            if (xIndex > 0 && yIndex > 0)
            {
                _model.Do((dynamic)Activator.CreateInstance(CurrentDrawCommand));
                _model.Start(xIndex, yIndex);
                _isPressed = true;
            }
        }

        // 畫完了
        public void HandleCanvasReleased(int xIndex, int yIndex)
        {
            if (_isPressed)
            {
                _isPressed = false;
                _model.Stop(xIndex, yIndex);
                _model.NotifyModelChanged();
            }
        }

        // 畫畫中
        public void HandleCanvasMoved(int xIndex, int yIndex)
        {
            if (_isPressed)
            {
                _model.Move(xIndex, yIndex);
                _model.NotifyModelChanged();
            }
        }

        // redo
        public void Redo()
        {
            _model.Redo();
            _isPressed = false;
        }

        // undo
        public void Undo()
        {
            _model.Undo();
            _isPressed = false;
        }

        // 把畫面清掉
        public void Clear()
        {
            _model.Clear();
            // 另外加的
            Adapter.ClearAll();
        }

        // 現在執行的DrawCommand
        public Type CurrentDrawCommand
        {
            get;
            set;
        }

        // model的getter
        public Model Model
        {
            get
            {
                return _model;
            }
        }

        // bypass到model的draw去
        public void Draw()
        {
            Model.Draw(Adapter);
        }

        // 各自的作畫的地方
        public abstract IGraphics Adapter
        {
            get;
        }

        // 存檔
        public void Save(Command.ISaveCommand command)
        {
            _saver.Save(command);
        }
    }
}
