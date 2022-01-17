using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel.Command
{
    public interface IDrawCommand : ICommand
    {
        // 畫畫方法由使用者用delegate注入

        Point StartPoint
        {
            get;
            set;
        }

        Point EndPoint
        {
            get;
            set;
        }
    }
}
