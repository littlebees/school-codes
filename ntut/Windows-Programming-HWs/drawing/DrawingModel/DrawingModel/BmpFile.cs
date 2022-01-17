using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;

namespace DrawingModel
{
    public class BmpFile : Command.ISaveCommand
    {
        // 高度
        public int Height
        {
            get;
            set;
        }

        // 寬度
        public int Width
        {
            get;
            set;
        }

        // 檔名
        public String FileName
        {
            get;
            set;
        }
    }
}
