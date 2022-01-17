using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public class HistoryNode
    {
        public HistoryNode(Command.IDrawCommand command)
        {
            Command = command;
            Visible = true;
        }

        // 指令
        public Command.IDrawCommand Command
        {
            get;
            set;
        }

        // 要畫到畫布上嗎
        public bool Visible
        {
            set;
            get;
        }
    }
}
