using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public abstract class SaverBase
    {
        private PresentationModelBase _model;

        // 需要Pmodel的參照，不然沒辦法畫圖
        public virtual void SetPresentationModel(PresentationModelBase model)
        {
            _model = model;
        }

        // PresentationModel的Getter
        public PresentationModelBase PresentationModel
        {
            get
            {
                return _model;
            }
        }

        // 指令的進入點
        public abstract void Save(Command.ISaveCommand command);
    }
}
