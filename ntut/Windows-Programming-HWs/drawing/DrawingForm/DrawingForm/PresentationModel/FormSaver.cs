using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DrawingForm.PresentationModel
{
    class FormSaver : DrawingModel.SaverBase
    {
        private PresentationModel _model;

        // 原本叫初始化
        public override void SetPresentationModel(DrawingModel.PresentationModelBase model)
        {
            base.SetPresentationModel(model);
            if (model is PresentationModel)
                this._model = model as PresentationModel;
        }

        // 進入點
        public override void Save(ISaveCommand command)
        {
            SaveFile((dynamic)command);
        }

        // 存bmp
        public void SaveFile(FormCommand.BmpCommand command)
        {
            Bitmap image = new Bitmap(command.Width, command.Height);
            using (_model.Graphics = Graphics.FromImage(image))
            {
                PresentationModel.Model.Draw(PresentationModel.Adapter);
            }

            using (FileStream saveStream = new FileStream(command.FileName, FileMode.OpenOrCreate))
            {
                image.Save(saveStream, ImageFormat.Bmp);
            }
        }
    }
}
