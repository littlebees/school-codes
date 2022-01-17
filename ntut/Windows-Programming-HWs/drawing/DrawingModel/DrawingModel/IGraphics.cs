using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public interface IGraphics
    {
        // 清空
        void ClearAll();
        
        // 畫圖
        void Draw(Command.IDrawCommand command);
    }
}
