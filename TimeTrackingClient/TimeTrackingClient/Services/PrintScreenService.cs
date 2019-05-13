using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TimeTrackingClient.Services
{
    class PrintScreenService
    {
        public string imageBase64 = "";
        public byte[] imageBytes = null;

        public PrintScreenService()
        {
            System.IO.MemoryStream ms = new MemoryStream();

            // - минификациия изображения
            var encoder = GetEncoderInfo("image/jpeg");

            var parameters = new EncoderParameters(2);
            var quality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);
            var compression = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW);
            parameters.Param[0] = quality;
            parameters.Param[1] = compression;
            // --
            CaptureScreen().Save(ms, encoder, parameters);

            //CaptureScreen().Save(ms, ImageFormat.Jpeg);
            imageBytes = ms.ToArray();
            imageBase64 = Convert.ToBase64String(imageBytes);

            // data:image/jpeg;base64,
        }

        private static Bitmap CaptureScreen()
        {
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (var g = Graphics.FromImage(bmpScreenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmpScreenshot;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(n => n.MimeType == mimeType);
        }
    }
}
