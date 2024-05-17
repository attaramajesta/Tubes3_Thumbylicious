using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PatternMatching
{
    public class Parser
    {
        public string ConvertImageToAscii(Image image, int Width, int Height)
        {
            using (Bitmap resizedImage = new Bitmap(image, Width, Height))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    // Menyimpan gambar yang telah diubah ukurannya sebagai JPEG
                    resizedImage.Save(stream, ImageFormat.Jpeg);
                    byte[] byteArray = stream.ToArray();

                    // Mengonversi array byte menjadi string ASCII
                    StringBuilder asciiString = new StringBuilder();
                    foreach (byte b in byteArray)
                    {
                        string byteString = Convert.ToString(b, 2).PadLeft(8, '0');
                        for (int i = 0; i < byteString.Length; i += 8)
                        {
                            if (i + 8 <= byteString.Length)
                            {
                                string bits = byteString.Substring(i, 8);
                                int asciiValue = Convert.ToInt32(bits, 2);
                                char asciiChar = (char)asciiValue;
                                asciiString.Append(asciiChar);
                            }
                        }
                    }
                    return asciiString.ToString();
                }
            }
        }
    }
}
