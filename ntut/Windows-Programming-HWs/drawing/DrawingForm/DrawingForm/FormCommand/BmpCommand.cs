using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingForm.FormCommand
{
    class BmpCommand : DrawingModel.BmpFile
    {
        public BmpCommand(int width, int height, String name)
        {
            Width = width;
            Height = height;
            FileName = name;
        }
    }
}
