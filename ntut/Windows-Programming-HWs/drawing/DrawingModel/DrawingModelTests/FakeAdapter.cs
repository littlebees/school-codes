using DrawingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;

namespace DrawingModelTests
{
    class FakeAdapter : IGraphics
    {
        // 計數器
        public int Count
        {
            get;
            set;
        }

        public FakeAdapter()
        {
            Count = 0;
        }

        // 清空
        public void ClearAll()
        {
            Count = 0;
        }

        // 計數
        public void Draw(IDrawCommand command)
        {
            Count += 1;
        }
    }
}
