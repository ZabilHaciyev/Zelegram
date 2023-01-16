using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zelegram.Services
{
    static class ConvertByteToBitmapImage
    {
        public static ImageSource ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = memStream;
                img.EndInit();
                img.Freeze();
            }
            //Returning the path
            ImageSource source = img;
            return source;
        }
    }
}
