using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PatternMatching
{
    public class Parser
    {
        public string ConvertImageToAscii(Image image)
        {
            #pragma warning disable CA1416 // Suppress platform-specific warnings
            // Convert image to byte array
            byte[] byteArray;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                byteArray = stream.ToArray();
            }
            #pragma warning restore CA1416 // Restore warnings

            // Convert byte array to bit string
            StringBuilder bitString = new StringBuilder();
            foreach (byte b in byteArray)
            {
                bitString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            // Convert bit string to ASCII string
            StringBuilder asciiString = new StringBuilder();
            string bitstring = bitString.ToString();
            for (int i = 0; i < bitstring.Length; i += 8)
            {
                if (i + 8 <= bitstring.Length)
                {
                    string byteString = bitstring.Substring(i, 8);
                    int asciiValue = Convert.ToInt32(byteString, 2);
                    char asciiChar = (char)asciiValue;
                    asciiString.Append(asciiChar);
                }
            }

            return asciiString.ToString();
        }
    }
}
