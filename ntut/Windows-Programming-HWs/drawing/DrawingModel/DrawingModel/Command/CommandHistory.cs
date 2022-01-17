using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;

namespace DrawingModel.Command
{
    public class CommandHistory : IEnumerable<IDrawCommand>
    {
        private const int EMPTY_INDEX = 0;

        // 應該是可以undo的command才對
        private List<HistoryNode> _stack = new List<HistoryNode>();
        //public int _nowCursor = EMPTY_INDEX;
        private DrawCommand.Nothing _placeHolder = new DrawCommand.Nothing();

        public CommandHistory()
        {
            Clear();
        }

        // 現在在畫的指令
        public IDrawCommand NowDrawing
        {
            get
            {
                return _stack[GetNowIndex()].Command;

            }
        }

        // redo
        public IDrawCommand Redo()
        {
            if (GetNowIndex() + 1 < _stack.Count)
                _stack[GetNowIndex() + 1].Visible = true;
            return NowDrawing;
        }

        // undo
        public IDrawCommand Undo()
        {
            if (GetNowIndex() > 0)
                _stack[GetNowIndex()].Visible = false;
            return NowDrawing;
        }

        // 初始化
        public void Clear()
        {
            _stack.Clear();
            _stack.Add(new HistoryNode(_placeHolder));
        }

        // 新增指令
        public void Insert(IDrawCommand command)
        {
            int index = GetNowIndex();
            if (index != _stack.Count - 1 && index + 1 < _stack.Count)
            {

                _stack[index + 1].Command = command;
                _stack[index + 1].Visible = true;
                // 把後面的draw取消掉
                // 應該改成用 linked list實做才對.......
                const int TWO = 2;
                for (index += TWO; index < _stack.Count; index++)
                    _stack[index].Command = _placeHolder;
            }
            else
            {
                _stack.Add(new HistoryNode(command));
            }
        }

        // from IEnumerable<IDrawCommand>
        public IEnumerator<IDrawCommand> GetEnumerator()
        {
            List<IDrawCommand> list = new List<IDrawCommand>();
            foreach (var item in _stack.Take(GetValidLength()))
                list.Add(item.Command);
            return ((IEnumerable<IDrawCommand>)list).GetEnumerator();
        }

        // from IEnumerable<IDrawCommand>
        IEnumerator IEnumerable.GetEnumerator()
        {
            List<IDrawCommand> list = new List<IDrawCommand>();
            foreach (var item in _stack.Take(GetValidLength()))
                list.Add(item.Command);
            return ((IEnumerable<IDrawCommand>)_stack.Take(GetValidLength())).GetEnumerator();
        }

        // 現在指向哪個指令
        public int GetNowIndex()
        {
            int index = _stack.Count - 1;
            while (index > 0 && !_stack[index].Visible)
                index--;
            return index;
        }

        // 要畫的指令數量
        public int GetValidLength()
        {
            int index = 0;
            while (index < _stack.Count && _stack[index].Visible)
                index++;
            return index;
        }
    }
}
