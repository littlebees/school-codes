using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;

namespace DrawingModelTests
{
    class FakeSaver : DrawingModel.SaverBase
    {
        public FakeSaver()
        {
            IsSaved = false;
        }

        // 檢驗有沒有呼叫到Save的變數
        public bool IsSaved
        {
            get;
            set;
        }

        // 實作ISaver
        public override void Save(ISaveCommand command)
        {
            IsSaved = true;
        }
    }
}
