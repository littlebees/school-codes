using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel;

namespace DrawingModelTests
{
    class FakePresentationModel : DrawingModel.PresentationModelBase
    {
        private FakeAdapter _counter;
        public FakePresentationModel(Model model, SaverBase saver) : base(model, saver)
        {
            _counter = new FakeAdapter();
        }

        public override IGraphics Adapter
        {
            get
            {
                return _counter;
            }
        }

        public int Count
        {
            get
            {
                return _counter.Count;
            }
        }
    }
}
