using System;
using System.Diagnostics;
using System.IO;

using System.Windows.Media.Imaging;

namespace ClientApp.Utility
{
    internal static class ImageConverter
    {
        public static BitmapFrame FromByteArray(byte[] imageBytes)
        {
            if (imageBytes == null) return null;
            try
            {
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return null;
            }
        }

        public static BitmapFrame FromFile(string file)
        {
            try
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    return BitmapFrame.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return null;
            }
        }

        public static byte[] CompressToByteArray(BitmapFrame bitmapFrame)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 80;
            encoder.Frames.Add(BitmapFrame.Create(bitmapFrame));
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.GetBuffer();
            }
        }


    }
}
