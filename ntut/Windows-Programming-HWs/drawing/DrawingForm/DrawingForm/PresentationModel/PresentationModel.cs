using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DrawingForm.PresentationModel
{
    class PresentationModel : DrawingModel.PresentationModelBase
    {
        public PresentationModel(Model model, Control canvas) : base(model, new FormSaver())
        {

        }

        // Graphics的accessor
        public Graphics Graphics
        {
            set;
            get;
        }

        // Adaptor的accessor
        public override IGraphics Adapter
        {
            get
            {
                return new WindowsFormsGraphicsAdaptor(Graphics);
            }
        }
    }
}
