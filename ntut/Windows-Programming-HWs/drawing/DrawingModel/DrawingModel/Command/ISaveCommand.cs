using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel.Command
{
    public interface ISaveCommand
    {
        int Width
        {
            get;
            set;
        }

        int Height
        {
            get;
            set;
        }

        String FileName
        {
            get;
            set;
        }
    }
}
