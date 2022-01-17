using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public interface IDrawer
    {
        // 實作一個不做事的draw
        void Draw(DrawingModel.DrawCommand.Nothing command);
    }
}
