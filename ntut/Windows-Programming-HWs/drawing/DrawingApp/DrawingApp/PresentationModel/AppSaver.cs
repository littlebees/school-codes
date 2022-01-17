using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingModel.Command;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Diagnostics;
using Windows.Storage;

namespace DrawingApp.PresentationModel
{
    class AppSaver : DrawingModel.SaverBase
    {
        private PresentationModel _model;

        // 存檔進入點
        public override void Save(ISaveCommand command)
        {
            SaveFile((dynamic)command);
        }

        // 初始化
        public override void SetPresentationModel(DrawingModel.PresentationModelBase model)
        {
            base.SetPresentationModel(model);
            if (model is PresentationModel)
                this._model = model as PresentationModel;
        }

        // bmp存檔
        public async void SaveFile(AppCommand.BmpCommand command)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync(command.FileName, Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            Debug.WriteLine(sampleFile.Path);
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(_model.Canvas, command.Width, command.Height);
            IBuffer buffer = await renderTargetBitmap.GetPixelsAsync();
            SoftwareBitmap softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer, BitmapPixelFormat.Rgba8, command.Width, command.Height);
            SaveSoftwareBitmapToFile(softwareBitmap, sampleFile);
        }

        // 存檔的helper
        private async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {

            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                encoder.SetSoftwareBitmap(softwareBitmap);
                await encoder.FlushAsync();
            }
        }
    }
}
