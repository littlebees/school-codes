using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;

namespace DrawingModelTests
{
    class FakeGraphics : DrawingModel.IGraphics
    {
        private int _number;

        // 請空
        public void ClearAll()
        {
            _number = 0;
        }

        // 計數
        public void Draw(IDrawCommand command)
        {
            _number++;
        }

        // 數字的getter
        public int GetNumber()
        {
            return _number;
        }
    }
}
