using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zelegram.Services
{
    static class ConvertImageToByte
    {
        public static byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
                filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] photo = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return photo;
        }
    }
}
